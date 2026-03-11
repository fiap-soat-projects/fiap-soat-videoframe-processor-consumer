using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Gateways.Producers.DTOs;

public record NotificationMessage
{
    public required string EditId { get; init; }
    public required string UserId { get; init; }
    public required string UserName { get; init; }
    public required NotificationType Type { get; init; }
    public IEnumerable<NotificationTarget> NotificationTargets { get; init; } = [];
    public string? FileUrl { get; init; }
    public string? ErrorMessage { get; init; }
}
