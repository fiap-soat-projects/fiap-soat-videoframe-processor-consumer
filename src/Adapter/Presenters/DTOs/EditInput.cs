using Domain.Entities.Enums;

namespace Adapter.Presenters.DTOs;

public record EditInput(
    string EditId, 
    string UserId, 
    string UserName,
    string UserRecipient, 
    string VideoPath,
    EditType EditType, 
    IEnumerable<NotificationTargetInput> NotificationTargets)
{
}
