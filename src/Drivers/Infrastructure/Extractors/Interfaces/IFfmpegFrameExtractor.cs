namespace Infrastructure.Extractors.Interfaces;

public interface IFfmpegFrameExtractor
{
    Task GenerateZipAsync(
        string videoUrl,
        Stream output,
        CancellationToken cancellationToken);
}
