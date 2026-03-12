using Adapter.Gateways.Extractors;
using FluentAssertions;
using Infrastructure.Extractors.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Extractors.VideoFrameExtractorTests.Constructor;

public class VideoFrameExtractorConstructorTests : VideoFrameExtractorDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var ffmpegFrameExtractor = Substitute.For<IFfmpegFrameExtractor>();

        // Act
        var result = new VideoFrameExtractor(ffmpegFrameExtractor);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VideoFrameExtractor>();
    }
}
