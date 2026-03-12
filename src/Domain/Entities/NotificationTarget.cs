using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public class NotificationTarget : IDomainEntity
{
    public NotificationTarget(NotificationChannel channel, string? target)
    {
        Channel = channel;
        Target = target;
    }

    public NotificationChannel Channel { get; set; }

    public string? Target
    {
        get;
        set
        {
            InvalidEntityPropertyException<NotificationTarget>.ThrowIfNullOrWhiteSpace(value, nameof(Target));
            field = value;
        }
    }
}
