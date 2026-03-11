using Adapter.Controllers.Interfaces;
using Adapter.Presenters.DTOs;
using Consumer.DTOs;
using Infrastructure.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Consumer;

[ExcludeFromCodeCoverage]
public class Worker : BackgroundService
{
    private readonly IKafkaService _kafkaService;
    private readonly ILogger<Worker> _logger;

    private readonly IVideoProcessingController _videoProcessingController;

    public Worker(
        IKafkaService kafkaService,
        IVideoProcessingController videoProcessingController,
        ILogger<Worker> logger)
    {
        _kafkaService = kafkaService;
        _videoProcessingController = videoProcessingController;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        const string VIDEOFRAME_NOTIFICATION_TOPIC_NAME = "processor-consumer";

        _kafkaService.Subscribe(VIDEOFRAME_NOTIFICATION_TOPIC_NAME);

        while (stoppingToken.IsCancellationRequested is false)
        {
            var consumeResult = _kafkaService.Consume(stoppingToken);

            var message = consumeResult.Message;

            try
            {
                var processorMessage = JsonSerializer.Deserialize<EditInput>(message.Value);

                if (processorMessage == null)
                {
                    
                    throw new Exception("Error during deserilizing message");
                }

                await _videoProcessingController.ProcessAsync(processorMessage, stoppingToken);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error during message processment, {exceptionMessage}", ex.Message);
                continue;
            }

            _kafkaService.Commit();
        }
    }
}
