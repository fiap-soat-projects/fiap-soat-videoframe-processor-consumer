using Domain.Entities.Exceptions;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public sealed class VideoJob : IDomainEntity
{
    public string VideoUrl { get; }
    public string OutputKey { get; }

    public VideoJob(string videoUrl, string outputKey)
    {
        InvalidEntityPropertyException<VideoJob>.ThrowIfNullOrWhiteSpace(videoUrl, nameof(VideoUrl));
        InvalidEntityPropertyException<VideoJob>.ThrowIfNullOrWhiteSpace(outputKey, nameof(OutputKey));

        VideoUrl = videoUrl;
        OutputKey = outputKey;
    }
}