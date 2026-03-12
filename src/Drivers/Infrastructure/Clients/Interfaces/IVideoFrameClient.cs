using Domain.Entities.Enums;
using Infrastructure.Clients.DTOs;

namespace Infrastructure.Clients.Interfaces;

public interface IVideoFrameClient
{
    Task UpdateEditStatusAsync(
        string editId,
        UpdateEditionStatusRequest req,
        CancellationToken cancellationToken);
}
