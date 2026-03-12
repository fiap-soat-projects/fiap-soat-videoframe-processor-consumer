using Domain.Entities;
using Domain.Gateways.Clients.Interfaces;
using Domain.UseCases.Interfaces;

namespace Domain.UseCases;

internal class StorageUseCase : IStorageUseCase
{
    private readonly IStorageClient _storageClient;

    public StorageUseCase(IStorageClient storageClient)
    {
        _storageClient = storageClient;
    }

    public Task<string> GetDownloadUrlAsync(string path, CancellationToken cancellationToken)
    {
        return _storageClient.GetDownloadUrlAsync(path, cancellationToken);
    }

    public string GetEditPath(Edit edit)
    {
        return $"users/{edit.UserId}/{edit.Type}/{Guid.NewGuid()}.zip";
    }
}
