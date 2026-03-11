using Domain.Gateways.Producers.DTOs;

namespace Infrastructure.Producers.Interfaces;

public interface IKafkaNotificationProducer
{
    Task ProduceAsync(NotificationMessage message, CancellationToken cancellationToken);
}
