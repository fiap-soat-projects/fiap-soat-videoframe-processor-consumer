using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Extractors.Interfaces;
using Domain.UseCases.Interfaces;
using System.IO.Pipelines;

namespace Domain.UseCases;

public class FrameEditUseCase : IEditUseCase
{
    private readonly IVideoFrameExtractor _extractor;
    private readonly IStorageClient _storage;

    public EditType EditType => EditType.Frame;

    public FrameEditUseCase(IVideoFrameExtractor extractor, IStorageClient storage)
    {
        _extractor = extractor;
        _storage = storage;
    }

    public async Task ProcessAsync(VideoJob job, CancellationToken cancellationToken)
    {
        var pipe = new Pipe();

        var uploadTask = _storage.UploadAsync(
            job.OutputKey,
            pipe.Reader.AsStream(),
            cancellationToken);

        await _extractor.GenerateZipAsync(
            job.VideoUrl,
            pipe.Writer.AsStream(),
            cancellationToken);

        await uploadTask;
    }
}
