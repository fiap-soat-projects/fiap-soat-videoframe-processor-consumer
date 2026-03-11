namespace Infrastructure.Clients.Interfaces;

public interface IS3BucketClient
{
    Task<string> GetPreSignedDownloadUrlAsync(string path, CancellationToken cancellationToken);
    Task UploadAsync(string key, Stream data, CancellationToken cancellationToken);
}
