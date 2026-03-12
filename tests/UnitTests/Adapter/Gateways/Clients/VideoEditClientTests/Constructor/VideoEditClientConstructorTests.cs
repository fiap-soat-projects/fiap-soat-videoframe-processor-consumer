using Adapter.Gateways.Clients;
using FluentAssertions;
using Infrastructure.Clients.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.VideoEditClientTests.Constructor;

public class VideoEditClientConstructorTests : VideoEditClientDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var videoFrameClient = Substitute.For<IVideoFrameClient>();

        // Act
        var result = new VideoEditClient(videoFrameClient);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VideoEditClient>();
    }
}
