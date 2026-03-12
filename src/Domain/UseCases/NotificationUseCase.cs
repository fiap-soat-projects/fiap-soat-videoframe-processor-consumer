using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Producers.DTOs;
using Domain.Gateways.Producers.Interfaces;
using Domain.UseCases.Interfaces;

namespace Domain.UseCases;

internal class NotificationUseCase : INotificationUseCase
{
    private readonly INotificationProducer _notificationProducer;
    private readonly IVideoEditClient _videoEditClient;

    public NotificationUseCase(INotificationProducer notificationProducer, IVideoEditClient videoEditClient)
    {
        _notificationProducer = notificationProducer;
        _videoEditClient = videoEditClient;
    }

    public async Task SendErrorAsync(Edit edit, string errorMessage, CancellationToken cancellationToken)
    {
        var notificationMessage = new NotificationMessage
        {
            EditId = edit.Id!,
            UserId = edit.UserId!,
            UserName = edit.UserName!,
            Type = NotificationType.Error,
            NotificationTargets = edit.NotificationTargets,
            Error = errorMessage,
        };

        await _notificationProducer.SendAsync(notificationMessage, cancellationToken);
        await _videoEditClient.UpdateAsync(edit.Id!, edit.UserId!, EditStatus.Error, cancellationToken);
    }

    public async Task SendSucessAsync(Edit edit, string editUrl, CancellationToken cancellationToken)
    {
        var notificationMessage = new NotificationMessage
        {
            EditId = edit.Id!,
            UserId = edit.UserId!,
            UserName = edit.UserName!,
            Type = NotificationType.Success,
            NotificationTargets = edit.NotificationTargets,
            FileUrl = editUrl,
        };

        await _notificationProducer.SendAsync(notificationMessage, cancellationToken);
        await _videoEditClient.UpdateAsync(edit.Id!, edit.UserId!, EditStatus.Processed, cancellationToken);
    }
}
