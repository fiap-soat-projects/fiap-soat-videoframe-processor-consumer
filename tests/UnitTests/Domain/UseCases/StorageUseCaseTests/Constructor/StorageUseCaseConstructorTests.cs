using Domain.Gateways.Clients.Interfaces;
using Domain.UseCases;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.StorageUseCaseTests.Constructor;

public class StorageUseCaseConstructorTests : StorageUseCaseDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var storageClient = Substitute.For<IStorageClient>();

        // Act
        var result = new StorageUseCase(storageClient);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<StorageUseCase>();
    }
}
