using Domain.Entities;

namespace Domain.UseCases.Interfaces;

public interface INotificationUseCase
{
    Task SendErrorAsync(Edit edit, string errorMessage, CancellationToken cancellationToken);
    Task SendSucessAsync(Edit edit, string editUrl, CancellationToken cancellationToken);
}
