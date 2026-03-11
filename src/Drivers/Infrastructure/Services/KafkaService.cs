using Confluent.Kafka;
using Infrastructure.Providers;
using Infrastructure.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Services;

[ExcludeFromCodeCoverage]
internal class KafkaService : IKafkaService, IDisposable
{
    private readonly IConsumer<Ignore, string> _consumer;

    public KafkaService()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = StaticEnvironmentVariableProvider.KafkaHost,
            GroupId = StaticEnvironmentVariableProvider.KafkaConsumerGroup,
            SecurityProtocol = SecurityProtocol.Plaintext,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
    }

    public ConsumeResult<Ignore, string> Consume(CancellationToken cancellationToken)
    {
        return _consumer.Consume(cancellationToken);
    }

    public void Commit()
    {
        _consumer.Commit();
    }

    public void Dispose()
    {
        _consumer.Close();
    }
}