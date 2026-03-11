using Confluent.Kafka;

namespace Infrastructure.Services.Interfaces;

public interface IKafkaService
{
    void Subscribe(string topic);
    ConsumeResult<Ignore, string> Consume(CancellationToken cancellationToken);
    void Commit();
}
