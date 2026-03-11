using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Adapter.Presenters.Extensions;

public static class NotificationTargetInputExtensions
{
    extension(NotificationTargetInput notificationTargetInput)
    {
        public NotificationTarget ToDomain()
        {
            var isNotificationChannelValid = Enum.TryParse<NotificationChannel>(notificationTargetInput.Channel, out var channel);

            if (isNotificationChannelValid is false)
            {
                throw new Exception("Error while trying to parse edit input, channel is in wrong format");
            }

            var notificationTarget = new NotificationTarget(channel, notificationTargetInput.Target);
            return notificationTarget;
        }
    }
}
