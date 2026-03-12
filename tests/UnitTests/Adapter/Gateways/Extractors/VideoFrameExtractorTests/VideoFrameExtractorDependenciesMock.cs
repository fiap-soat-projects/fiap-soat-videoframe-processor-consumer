using Adapter.Gateways.Extractors;
using Domain.Gateways.Extractors.Interfaces;
using Infrastructure.Extractors.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Extractors.VideoFrameExtractorTests;

public abstract class VideoFrameExtractorDependenciesMock
{
    protected readonly IFfmpegFrameExtractor _ffmpegFrameExtractor;
    protected readonly IVideoFrameExtractor _sut;

    protected VideoFrameExtractorDependenciesMock()
    {
        _ffmpegFrameExtractor = Substitute.For<IFfmpegFrameExtractor>();
        _sut = new VideoFrameExtractor(_ffmpegFrameExtractor);
    }
}
