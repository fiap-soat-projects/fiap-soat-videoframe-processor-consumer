namespace Domain.Gateways.Clients.Interfaces;

public interface IStorageClient
{
    Task<string> GetDownloadUrlAsync(string path, CancellationToken cancellationToken);
    Task UploadAsync(string key, Stream data, CancellationToken cancellationToken);
}
