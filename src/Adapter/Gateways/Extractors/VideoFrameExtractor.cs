using Domain.Gateways.Extractors.Interfaces;
using Infrastructure.Extractors.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Adapter.Gateways.Extractors;

[ExcludeFromCodeCoverage]
internal class VideoFrameExtractor : IVideoFrameExtractor
{
    private readonly IFfmpegFrameExtractor _ffmpegFrameExtractor;

    public VideoFrameExtractor(IFfmpegFrameExtractor ffmpegFrameExtractor)
    {
        _ffmpegFrameExtractor = ffmpegFrameExtractor;
    }

    public Task GenerateZipAsync(string videoUrl, Stream output, CancellationToken cancellationToken)
    {
        return _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken);
    }
}
