using Adapter.Controllers.Interfaces;
using Adapter.Presenters.DTOs;
using Adapter.Presenters.Extensions;
using Domain.Entities;
using Domain.UseCases.Interfaces;
using Microsoft.Extensions.Logging;

namespace Adapter.Controllers;

internal class VideoProcessingController : IVideoProcessingController
{
    private readonly IEditUseCaseResolver _editUseCaseResolver;
    private readonly INotificationUseCase _notificationUseCase;
    private readonly IStorageUseCase _storageUseCase;
    private readonly ILogger<VideoProcessingController> _logger;

    public VideoProcessingController(
        IEditUseCaseResolver editUseCaseResolver,
        INotificationUseCase notificationUseCase,
        IStorageUseCase storageUseCase,
        ILogger<VideoProcessingController> logger)
    {
        _editUseCaseResolver = editUseCaseResolver;
        _notificationUseCase = notificationUseCase;
        _storageUseCase = storageUseCase;
        _logger = logger;
    }

    public async Task ProcessAsync(EditInput editInput, CancellationToken cancellationToken)
    {
        var edit = new Edit(
                editInput.EditId,
                editInput.UserId,
                editInput.UserName,
                editInput.VideoPath,
                editInput.EditType,
                editInput.NotificationTargets.Select(x => x.ToDomain()));

        var resultPath = _storageUseCase.GetEditPathAsync(edit);

        try
        {
            var videoUrl = await _storageUseCase.GetDownloadUrlAsync(edit.VideoPath!, cancellationToken);

            var videoJob = new VideoJob(videoUrl, resultPath);

            var editUseCase = _editUseCaseResolver.Resolve(editInput.EditType);

            await editUseCase.ProcessAsync(videoJob, cancellationToken);
        } 
        catch (Exception ex)
        {
            _logger.LogError("An error occouring in edit processment, {exeptionMessage}", ex.Message);
            await _notificationUseCase.SendErrorAsync(edit, ex.Message, cancellationToken);
            return;
        }

        var resultUrl = await _storageUseCase.GetDownloadUrlAsync(resultPath, cancellationToken);

        await _notificationUseCase.SendSucessAsync(edit, resultUrl, cancellationToken);
    }
}
