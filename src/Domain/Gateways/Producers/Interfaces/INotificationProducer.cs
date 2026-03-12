using Domain.Gateways.Producers.DTOs;

namespace Domain.Gateways.Producers.Interfaces;

public interface INotificationProducer
{
    Task SendAsync(NotificationMessage notificationMessage, CancellationToken cancellationToken);
}
