using Domain.Gateways.Producers.DTOs;
using Domain.Gateways.Producers.Interfaces;
using Infrastructure.Producers.Interfaces;

namespace Adapter.Gateways.Producers;

internal class NotificationProducer : INotificationProducer
{
    private readonly IKafkaNotificationProducer _kafkaNotificationProducer;

    public NotificationProducer(IKafkaNotificationProducer kafkaNotificationProducer)
    {
        _kafkaNotificationProducer = kafkaNotificationProducer;
    }

    public Task SendAsync(NotificationMessage notificationMessage, CancellationToken cancellationToken)
    {
        return _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken);
    }
}
