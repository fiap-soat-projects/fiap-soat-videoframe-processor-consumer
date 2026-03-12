using Adapter.Gateways.Clients;
using Infrastructure.Clients.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.StorageClientTests;

public abstract class StorageClientDependenciesMock
{
    protected readonly IS3BucketClient _s3BucketClient;
    protected readonly StorageClient _sut;

    protected StorageClientDependenciesMock()
    {
        _s3BucketClient = Substitute.For<IS3BucketClient>();
        _sut = new StorageClient(_s3BucketClient);
    }
}
