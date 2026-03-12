using Adapter.Controllers.Interfaces;
using Adapter.Presenters.DTOs;
using Consumer.DTOs;
using Infrastructure.Providers;
using Infrastructure.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Consumer;

[ExcludeFromCodeCoverage]
public class Worker : BackgroundService
{
    private readonly IKafkaService _kafkaService;
    private readonly ILogger<Worker> _logger;
    private readonly string _topicName;
    private readonly JsonSerializerOptions _options;


    private readonly IVideoProcessingController _videoProcessingController;

    public Worker(
        IKafkaService kafkaService,
        IVideoProcessingController videoProcessingController,
        ILogger<Worker> logger)
    {
        _kafkaService = kafkaService;
        _videoProcessingController = videoProcessingController;
        _logger = logger;
        _topicName = StaticEnvironmentVariableProvider.KafkaConsumeTopicName;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        options.Converters.Add(new JsonStringEnumConverter());

        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        _kafkaService.Subscribe(_topicName);

        while (stoppingToken.IsCancellationRequested is false)
        {

            _logger.LogInformation("Waiting Messages");
            var consumeResult = _kafkaService.Consume(stoppingToken);

            var message = consumeResult.Message;

            try
            {
                var processorMessage = JsonSerializer.Deserialize<EditInput>(message.Value, _options);

                if (processorMessage == null)
                {
                    throw new Exception("Error during deserilizing message");
                }

                _logger.LogInformation("Starting Process");

                await _videoProcessingController.ProcessAsync(processorMessage, stoppingToken);

                _logger.LogInformation("Finilize Process");
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error during message processment, {exceptionMessage}", ex.Message);
                continue;
            }

            _kafkaService.Commit();
        }

        Environment.Exit(0);
    }
}
