using Domain.Gateways.Clients.Interfaces;
using Infrastructure.Clients.Interfaces;

namespace Adapter.Gateways.Clients;

public class StorageClient : IStorageClient
{
    private readonly IS3BucketClient _s3BucketClient;

    public StorageClient(IS3BucketClient s3BucketClient)
    {
        _s3BucketClient = s3BucketClient;
    }

    public Task<string> GetDownloadUrlAsync(string path, CancellationToken cancellationToken)
    {
        return _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken);
    }

    public Task UploadAsync(string key, Stream data, CancellationToken cancellationToken)
    {
        return _s3BucketClient.UploadAsync(key, data, cancellationToken);
    }
}
