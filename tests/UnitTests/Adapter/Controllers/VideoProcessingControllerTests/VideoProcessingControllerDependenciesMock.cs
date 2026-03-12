using Adapter.Controllers;
using Adapter.Controllers.Interfaces;
using Domain.UseCases.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoProcessingControllerTests;

public abstract class VideoProcessingControllerDependenciesMock
{
    protected readonly IEditUseCaseResolver _editUseCaseResolver;
    protected readonly INotificationUseCase _notificationUseCase;
    protected readonly IStorageUseCase _storageUseCase;
    protected readonly IVideoProcessingController _sut;

    protected VideoProcessingControllerDependenciesMock()
    {
        _editUseCaseResolver = Substitute.For<IEditUseCaseResolver>();
        _notificationUseCase = Substitute.For<INotificationUseCase>();
        _storageUseCase = Substitute.For<IStorageUseCase>();
        var logger = Substitute.For<ILogger<VideoProcessingController>>();

        _sut = new VideoProcessingController(
            _editUseCaseResolver,
            _notificationUseCase,
            _storageUseCase,
            logger);
    }
}
