using Domain.Gateways.Extractors.Interfaces;
using Infrastructure.Extractors;

namespace Adapter.Gateways.Extractors;

internal class VideoFrameExtractor : IVideoFrameExtractor
{
    public async Task GenerateZipAsync(string videoUrl, Stream output, CancellationToken cancellationToken)
    {
        await FfmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken);
    }
}
