using Adapter.Controllers;
using Domain.UseCases.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoProcessingControllerTests.Constructor;

public class VideoProcessingControllerConstructorTests : VideoProcessingControllerDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var editUseCaseResolver = Substitute.For<IEditUseCaseResolver>();
        var notificationUseCase = Substitute.For<INotificationUseCase>();
        var storageUseCase = Substitute.For<IStorageUseCase>();
        var logger = Substitute.For<ILogger<VideoProcessingController>>();

        // Act
        var result = new VideoProcessingController(
            editUseCaseResolver,
            notificationUseCase,
            storageUseCase,
            logger);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VideoProcessingController>();
    }
}
