using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public class Edit : IDomainEntity
{
    public string? Id
    {
        get;
        set
        {
            InvalidEntityPropertyException<Edit>.ThrowIfNullOrWhiteSpace(value, nameof(Id));
            field = value;
        }
    }

    public string? UserId
    {
        get;
        set
        {
            InvalidEntityPropertyException<Edit>.ThrowIfNullOrWhiteSpace(value, nameof(UserId));
            field = value;
        }
    }

    public string? UserName
    {
        get;
        set
        {
            InvalidEntityPropertyException<Edit>.ThrowIfNullOrWhiteSpace(value, nameof(UserName));
            field = value;
        }
    }

    public string? VideoPath
    {
        get;
        set
        {
            InvalidEntityPropertyException<Edit>.ThrowIfNullOrWhiteSpace(value, nameof(VideoPath));
            field = value;
        }
    }

    public EditType Type { get; set; }
    public IEnumerable<NotificationTarget> NotificationTargets { get; set; }

    public Edit(
        string? id, 
        string? userId, 
        string? userName, 
        string? videoPath, 
        EditType type,
        IEnumerable<NotificationTarget> notificationTargets)
    {
        Id = id;
        UserId = userId;
        UserName = userName;
        VideoPath = videoPath;
        Type = type;
        NotificationTargets = notificationTargets;
    }
}
