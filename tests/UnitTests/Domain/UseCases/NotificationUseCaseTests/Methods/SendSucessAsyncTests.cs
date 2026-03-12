using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Producers.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.NotificationUseCaseTests.Methods;

public class SendSucessAsyncTests : NotificationUseCaseDependenciesMock
{
    [Fact]
    public async Task When_SendSucessAsync_Called_Then_Send_Notification_With_Success_Type()
    {
        // Arrange
        var edit = CreateValidEdit();
        var editUrl = "https://storage.example.com/edited-video.mp4";
        var cancellationToken = CancellationToken.None;

        NotificationMessage? capturedMessage = null;
        await _notificationProducer.SendAsync(Arg.Do<NotificationMessage>(msg => capturedMessage = msg), Arg.Any<CancellationToken>());

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        capturedMessage.Should().NotBeNull();
        capturedMessage!.EditId.Should().Be(edit.Id);
        capturedMessage.UserId.Should().Be(edit.UserId);
        capturedMessage.UserName.Should().Be(edit.UserName);
        capturedMessage.Type.Should().Be(NotificationType.Success);
        capturedMessage.NotificationTargets.Should().BeEquivalentTo(edit.NotificationTargets);
        capturedMessage.FileUrl.Should().Be(editUrl);
        capturedMessage.Error.Should().BeNull();
    }

    [Fact]
    public async Task When_SendSucessAsync_Called_Then_Call_NotificationProducer_SendAsync()
    {
        // Arrange
        var edit = CreateValidEdit();
        var editUrl = "https://storage.example.com/edited-video.mp4";
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        await _notificationProducer.Received(1).SendAsync(
            Arg.Is<NotificationMessage>(msg =>
                msg.EditId == edit.Id &&
                msg.UserId == edit.UserId &&
                msg.UserName == edit.UserName &&
                msg.Type == NotificationType.Success &&
                msg.FileUrl == editUrl),
            cancellationToken);
    }

    [Fact]
    public async Task When_SendSucessAsync_Called_Then_Call_VideoEditClient_UpdateAsync_With_Processed_Status()
    {
        // Arrange
        var edit = CreateValidEdit();
        var editUrl = "https://storage.example.com/edited-video.mp4";
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        await _videoEditClient.Received(1).UpdateAsync(
            edit.Id!,
            edit.UserId!,
            EditStatus.Processed,
            cancellationToken);
    }

    [Fact]
    public async Task When_SendSucessAsync_Called_Then_Execute_In_Correct_Order()
    {
        // Arrange
        var edit = CreateValidEdit();
        var editUrl = "https://storage.example.com/edited-video.mp4";
        var cancellationToken = CancellationToken.None;

        var callOrder = new List<string>();

        _notificationProducer.SendAsync(Arg.Any<NotificationMessage>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(_ => callOrder.Add("SendAsync"));

        _videoEditClient.UpdateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<EditStatus>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(_ => callOrder.Add("UpdateAsync"));

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        callOrder.Should().HaveCount(2);
        callOrder[0].Should().Be("SendAsync");
        callOrder[1].Should().Be("UpdateAsync");
    }

    [Theory]
    [InlineData("https://storage.example.com/video1.mp4")]
    [InlineData("https://cdn.example.com/processed/video2.mp4")]
    [InlineData("s3://bucket/video3.mp4")]
    public async Task When_SendSucessAsync_Called_With_Different_Urls_Then_Send_Correct_Url(string editUrl)
    {
        // Arrange
        var edit = CreateValidEdit();
        var cancellationToken = CancellationToken.None;

        NotificationMessage? capturedMessage = null;
        await _notificationProducer.SendAsync(Arg.Do<NotificationMessage>(msg => capturedMessage = msg), Arg.Any<CancellationToken>());

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        capturedMessage.Should().NotBeNull();
        capturedMessage!.FileUrl.Should().Be(editUrl);
    }

    [Fact]
    public async Task When_SendSucessAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Dependencies()
    {
        // Arrange
        var edit = CreateValidEdit();
        var editUrl = "https://storage.example.com/edited-video.mp4";
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        await _notificationProducer.Received(1).SendAsync(Arg.Any<NotificationMessage>(), cancellationToken);
        await _videoEditClient.Received(1).UpdateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<EditStatus>(), cancellationToken);
    }

    [Fact]
    public async Task When_SendSucessAsync_Called_With_Multiple_Notification_Targets_Then_Include_All_Targets()
    {
        // Arrange
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "user1@example.com"),
            new NotificationTarget(NotificationChannel.Webhook, "https://webhook.example.com/notify")
        };

        var edit = new Edit(
            id: "edit-123",
            userId: "user-456",
            userName: "Test User",
            videoPath: "/path/to/video.mp4",
            type: EditType.Frame,
            notificationTargets: notificationTargets);

        var editUrl = "https://storage.example.com/edited-video.mp4";
        var cancellationToken = CancellationToken.None;

        NotificationMessage? capturedMessage = null;
        await _notificationProducer.SendAsync(Arg.Do<NotificationMessage>(msg => capturedMessage = msg), Arg.Any<CancellationToken>());

        // Act
        await _sut.SendSucessAsync(edit, editUrl, cancellationToken);

        // Assert
        capturedMessage.Should().NotBeNull();
        capturedMessage!.NotificationTargets.Should().HaveCount(2);
        capturedMessage.NotificationTargets.Should().BeEquivalentTo(notificationTargets);
    }

    private Edit CreateValidEdit()
    {
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "test@example.com")
        };

        return new Edit(
            id: "edit-123",
            userId: "user-456",
            userName: "Test User",
            videoPath: "/path/to/video.mp4",
            type: EditType.Frame,
            notificationTargets: notificationTargets);
    }
}
