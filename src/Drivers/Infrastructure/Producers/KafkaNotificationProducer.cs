using Confluent.Kafka;
using Domain.Gateways.Producers.DTOs;
using Infrastructure.Producers.Interfaces;
using Infrastructure.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Producers;

public class KafkaNotificationProducer : IKafkaNotificationProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topicName;
    private readonly JsonSerializerOptions _options;

    public KafkaNotificationProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
        _topicName = StaticEnvironmentVariableProvider.KafkaProduceTopicName;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        options.Converters.Add(new JsonStringEnumConverter());

        _options = options;
    }

    public async Task ProduceAsync(NotificationMessage message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var payload = JsonSerializer.Serialize(message, _options);

        var result = await _producer.ProduceAsync(
            _topicName,
            new Message<Null, string> { Value = payload },
            cancellationToken).ConfigureAwait(false);

        if (result.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception("Message not persisted");
        }
    }
}