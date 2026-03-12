using Domain.Entities.Enums;

namespace Domain.Gateways.Clients.Interfaces;

public interface IVideoEditClient
{
    Task UpdateAsync(
        string editId,
        string userId,
        EditStatus editStatus,
        CancellationToken cancellationToken);
}
