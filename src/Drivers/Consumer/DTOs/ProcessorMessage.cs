namespace Consumer.DTOs;

public record ProcessorMessage(string EditId, string UserId, string UserName, string UserRecipient, string VideoPath, string EditType, IEnumerable<NotificationTargetMessage> NotificationTargets)
{
}
