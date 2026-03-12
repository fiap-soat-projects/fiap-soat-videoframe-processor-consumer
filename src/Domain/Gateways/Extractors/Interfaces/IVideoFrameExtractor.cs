namespace Domain.Gateways.Extractors.Interfaces;

public interface IVideoFrameExtractor
{
    Task GenerateZipAsync(
        string videoUrl,
        Stream output,
        CancellationToken cancellationToken);
}
