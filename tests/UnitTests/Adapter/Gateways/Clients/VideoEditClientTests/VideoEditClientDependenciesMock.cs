using Adapter.Gateways.Clients;
using Domain.Gateways.Clients.Interfaces;
using Infrastructure.Clients.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.VideoEditClientTests;

public abstract class VideoEditClientDependenciesMock
{
    protected readonly IVideoFrameClient _videoFrameClient;
    protected readonly IVideoEditClient _sut;

    protected VideoEditClientDependenciesMock()
    {
        _videoFrameClient = Substitute.For<IVideoFrameClient>();
        _sut = new VideoEditClient(_videoFrameClient);
    }
}
