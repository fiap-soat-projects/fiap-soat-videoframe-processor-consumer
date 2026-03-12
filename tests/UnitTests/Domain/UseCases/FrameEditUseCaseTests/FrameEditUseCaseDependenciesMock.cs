using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Extractors.Interfaces;
using Domain.UseCases;
using NSubstitute;

namespace UnitTests.Domain.UseCases.FrameEditUseCaseTests;

public abstract class FrameEditUseCaseDependenciesMock
{
    protected readonly IVideoFrameExtractor _extractor;
    protected readonly IStorageClient _storage;
    protected readonly FrameEditUseCase _sut;

    protected FrameEditUseCaseDependenciesMock()
    {
        _extractor = Substitute.For<IVideoFrameExtractor>();
        _storage = Substitute.For<IStorageClient>();

        _sut = new FrameEditUseCase(_extractor, _storage);
    }
}
