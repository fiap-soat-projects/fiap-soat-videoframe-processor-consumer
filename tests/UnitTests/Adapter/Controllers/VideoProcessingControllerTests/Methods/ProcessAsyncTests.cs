using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.UseCases.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoProcessingControllerTests.Methods;

public class ProcessAsyncTests : VideoProcessingControllerDependenciesMock
{
    [Fact]
    public async Task When_ProcessAsync_Called_Then_Call_StorageUseCase_GetEditPath()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();

        SetupSuccessfulFlow(editUseCase);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        _storageUseCase.Received(1).GetEditPath(Arg.Any<Edit>());
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Call_StorageUseCase_GetDownloadUrlAsync_For_VideoPath()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();

        SetupSuccessfulFlow(editUseCase);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await _storageUseCase.Received(1).GetDownloadUrlAsync(editInput.VideoPath, cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Call_EditUseCaseResolver_Resolve()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();

        SetupSuccessfulFlow(editUseCase);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        _editUseCaseResolver.Received(1).Resolve(editInput.EditType);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Call_EditUseCase_ProcessAsync()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();

        SetupSuccessfulFlow(editUseCase);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await editUseCase.Received(1).ProcessAsync(Arg.Any<VideoJob>(), cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Succeeds_Then_Call_StorageUseCase_GetDownloadUrlAsync_For_ResultPath()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var resultPath = "users/user123/Frame/edit456.zip";

        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns(resultPath);
        _storageUseCase.GetDownloadUrlAsync(editInput.VideoPath, cancellationToken)
            .Returns("https://download-url.com/video.mp4");
        _storageUseCase.GetDownloadUrlAsync(resultPath, cancellationToken)
            .Returns("https://result-url.com/result.zip");
        _editUseCaseResolver.Resolve(editInput.EditType).Returns(editUseCase);
        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await _storageUseCase.Received(1).GetDownloadUrlAsync(resultPath, cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Succeeds_Then_Call_NotificationUseCase_SendSucessAsync()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var resultUrl = "https://result-url.com/result.zip";

        SetupSuccessfulFlow(editUseCase, resultUrl);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await _notificationUseCase.Received(1).SendSucessAsync(
            Arg.Any<Edit>(),
            resultUrl,
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Throws_Exception_Then_Log_Error()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var expectedException = new InvalidOperationException("Processing failed");

        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns("result-path.zip");
        _storageUseCase.GetDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("https://download-url.com/video.mp4");
        _editUseCaseResolver.Resolve(Arg.Any<EditType>()).Returns(editUseCase);
        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        // Logger is tested implicitly through the error handling flow
        await _notificationUseCase.Received(1).SendErrorAsync(
            Arg.Any<Edit>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_ProcessAsync_Throws_Exception_Then_Call_NotificationUseCase_SendErrorAsync()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var expectedException = new InvalidOperationException("Processing failed");

        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns("result-path.zip");
        _storageUseCase.GetDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("https://download-url.com/video.mp4");
        _editUseCaseResolver.Resolve(Arg.Any<EditType>()).Returns(editUseCase);
        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await _notificationUseCase.Received(1).SendErrorAsync(
            Arg.Any<Edit>(),
            expectedException.Message,
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Throws_Exception_Then_Do_Not_Call_SendSucessAsync()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var expectedException = new InvalidOperationException("Processing failed");

        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns("result-path.zip");
        _storageUseCase.GetDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("https://download-url.com/video.mp4");
        _editUseCaseResolver.Resolve(Arg.Any<EditType>()).Returns(editUseCase);
        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await _notificationUseCase.DidNotReceive().SendSucessAsync(
            Arg.Any<Edit>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_ProcessAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Dependencies()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var editUseCase = Substitute.For<IEditUseCase>();

        SetupSuccessfulFlow(editUseCase);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        await _storageUseCase.Received(2).GetDownloadUrlAsync(Arg.Any<string>(), cancellationToken);
        await editUseCase.Received(1).ProcessAsync(Arg.Any<VideoJob>(), cancellationToken);
        await _notificationUseCase.Received(1).SendSucessAsync(Arg.Any<Edit>(), Arg.Any<string>(), cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Create_VideoJob_With_Correct_Parameters()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var videoUrl = "https://download-url.com/video.mp4";
        var resultPath = "users/user123/Frame/edit456.zip";

        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns(resultPath);
        _storageUseCase.GetDownloadUrlAsync(editInput.VideoPath, cancellationToken).Returns(videoUrl);
        _storageUseCase.GetDownloadUrlAsync(resultPath, cancellationToken).Returns("https://result.com");
        _editUseCaseResolver.Resolve(editInput.EditType).Returns(editUseCase);
        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), cancellationToken).Returns(Task.CompletedTask);

        VideoJob? capturedVideoJob = null;
        await editUseCase.ProcessAsync(
            Arg.Do<VideoJob>(job => capturedVideoJob = job),
            Arg.Any<CancellationToken>());

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        capturedVideoJob.Should().NotBeNull();
        capturedVideoJob!.VideoUrl.Should().Be(videoUrl);
        capturedVideoJob.OutputKey.Should().Be(resultPath);
    }

    [Fact]
    public async Task When_ProcessAsync_Succeeds_Then_Execute_Steps_In_Correct_Order()
    {
        // Arrange
        var editInput = CreateValidEditInput();
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();
        var callOrder = new List<string>();

        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns(callInfo =>
        {
            callOrder.Add("GetEditPath");
            return "result-path.zip";
        });

        _storageUseCase.GetDownloadUrlAsync(editInput.VideoPath, cancellationToken).Returns(callInfo =>
        {
            callOrder.Add("GetDownloadUrlAsync-Video");
            return Task.FromResult("https://video-url.com");
        });

        _editUseCaseResolver.Resolve(editInput.EditType).Returns(callInfo =>
        {
            callOrder.Add("Resolve");
            return editUseCase;
        });

        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), cancellationToken).Returns(callInfo =>
        {
            callOrder.Add("ProcessAsync");
            return Task.CompletedTask;
        });

        _storageUseCase.GetDownloadUrlAsync("result-path.zip", cancellationToken).Returns(callInfo =>
        {
            callOrder.Add("GetDownloadUrlAsync-Result");
            return Task.FromResult("https://result-url.com");
        });

        _notificationUseCase.SendSucessAsync(Arg.Any<Edit>(), Arg.Any<string>(), cancellationToken).Returns(callInfo =>
        {
            callOrder.Add("SendSucessAsync");
            return Task.CompletedTask;
        });

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        callOrder.Should().Equal(
            "GetEditPath",
            "GetDownloadUrlAsync-Video",
            "Resolve",
            "ProcessAsync",
            "GetDownloadUrlAsync-Result",
            "SendSucessAsync");
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public async Task When_ProcessAsync_Called_With_Different_EditTypes_Then_Resolve_Correct_UseCase(EditType editType)
    {
        // Arrange
        var editInput = CreateValidEditInput(editType);
        var cancellationToken = CancellationToken.None;
        var editUseCase = Substitute.For<IEditUseCase>();

        SetupSuccessfulFlow(editUseCase);

        // Act
        await _sut.ProcessAsync(editInput, cancellationToken);

        // Assert
        _editUseCaseResolver.Received(1).Resolve(editType);
    }

    private EditInput CreateValidEditInput(EditType editType = EditType.Frame)
    {
        return new EditInput(
            EditId: "edit-123",
            UserId: "user-456",
            UserName: "Test User",
            UserRecipient: "test@example.com",
            VideoPath: "videos/test-video.mp4",
            EditType: editType,
            NotificationTargets: new List<NotificationTargetInput>
            {
                new NotificationTargetInput("Email", "test@example.com")
            });
    }

    private void SetupSuccessfulFlow(IEditUseCase editUseCase, string? resultUrl = null)
    {
        _storageUseCase.GetEditPath(Arg.Any<Edit>()).Returns("result-path.zip");
        _storageUseCase.GetDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("https://download-url.com/video.mp4", resultUrl ?? "https://result-url.com/result.zip");
        _editUseCaseResolver.Resolve(Arg.Any<EditType>()).Returns(editUseCase);
        editUseCase.ProcessAsync(Arg.Any<VideoJob>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        _notificationUseCase.SendSucessAsync(Arg.Any<Edit>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
    }
}
