using Domain.Gateways.Clients.Interfaces;
using Domain.UseCases;
using Domain.UseCases.Interfaces;
using NSubstitute;

namespace UnitTests.Domain.UseCases.StorageUseCaseTests;

public abstract class StorageUseCaseDependenciesMock
{
    protected readonly IStorageClient _storageClient;
    protected readonly IStorageUseCase _sut;

    protected StorageUseCaseDependenciesMock()
    {
        _storageClient = Substitute.For<IStorageClient>();
        _sut = new StorageUseCase(_storageClient);
    }
}
