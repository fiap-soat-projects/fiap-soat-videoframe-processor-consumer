using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Infrastructure.Clients.Interfaces;
using System.Reflection.Metadata;

namespace Infrastructure.Clients;

internal class S3BucketClient : IS3BucketClient
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _client;

    public S3BucketClient(AmazonS3Client amazonS3Client, string bucketName)
    {
        _client = amazonS3Client;
        _bucketName = bucketName;
    }

    public async Task<string> GetPreSignedDownloadUrlAsync(string path, CancellationToken cancellationToken)
    {
        var downloadRequest = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = path,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddHours(24)
        };

        var url = await _client.GetPreSignedURLAsync(downloadRequest);
            
        return url.Replace("https", "http");
    }

    public async Task UploadAsync(string key, Stream data, CancellationToken cancellationToken)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        if (data.CanSeek)
        {
            data.Position = 0;
        }
        var transfer = new TransferUtility(_client);

        var putRequest = new TransferUtilityUploadRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = data,
            ContentType = "application/zip",
        };

        await transfer.UploadAsync(putRequest, cancellationToken);
    }
}
