using Adapter.Gateways.Clients;
using FluentAssertions;
using Infrastructure.Clients.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.StorageClientTests.Constructor;

public class StorageClientConstructorTests : StorageClientDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var s3BucketClient = Substitute.For<IS3BucketClient>();

        // Act
        var result = new StorageClient(s3BucketClient);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<StorageClient>();
    }
}
