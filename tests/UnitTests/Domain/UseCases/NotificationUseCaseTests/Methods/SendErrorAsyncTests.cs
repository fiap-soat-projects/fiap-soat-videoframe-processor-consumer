using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Producers.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.NotificationUseCaseTests.Methods;

public class SendErrorAsyncTests : NotificationUseCaseDependenciesMock
{
    [Fact]
    public async Task When_SendErrorAsync_Called_Then_Send_Notification_With_Error_Type()
    {
        // Arrange
        var edit = CreateValidEdit();
        var errorMessage = "Test error message";
        var cancellationToken = CancellationToken.None;

        NotificationMessage? capturedMessage = null;
        await _notificationProducer.SendAsync(Arg.Do<NotificationMessage>(msg => capturedMessage = msg), Arg.Any<CancellationToken>());

        // Act
        await _sut.SendErrorAsync(edit, errorMessage, cancellationToken);

        // Assert
        capturedMessage.Should().NotBeNull();
        capturedMessage!.EditId.Should().Be(edit.Id);
        capturedMessage.UserId.Should().Be(edit.UserId);
        capturedMessage.UserName.Should().Be(edit.UserName);
        capturedMessage.Type.Should().Be(NotificationType.Error);
        capturedMessage.NotificationTargets.Should().BeEquivalentTo(edit.NotificationTargets);
        capturedMessage.Error.Should().Be(errorMessage);
        capturedMessage.FileUrl.Should().BeNull();
    }

    [Fact]
    public async Task When_SendErrorAsync_Called_Then_Call_NotificationProducer_SendAsync()
    {
        // Arrange
        var edit = CreateValidEdit();
        var errorMessage = "Test error message";
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.SendErrorAsync(edit, errorMessage, cancellationToken);

        // Assert
        await _notificationProducer.Received(1).SendAsync(
            Arg.Is<NotificationMessage>(msg =>
                msg.EditId == edit.Id &&
                msg.UserId == edit.UserId &&
                msg.UserName == edit.UserName &&
                msg.Type == NotificationType.Error &&
                msg.Error == errorMessage),
            cancellationToken);
    }

    [Fact]
    public async Task When_SendErrorAsync_Called_Then_Call_VideoEditClient_UpdateAsync_With_Error_Status()
    {
        // Arrange
        var edit = CreateValidEdit();
        var errorMessage = "Test error message";
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.SendErrorAsync(edit, errorMessage, cancellationToken);

        // Assert
        await _videoEditClient.Received(1).UpdateAsync(
            edit.Id!,
            edit.UserId!,
            EditStatus.Error,
            cancellationToken);
    }

    [Fact]
    public async Task When_SendErrorAsync_Called_Then_Execute_In_Correct_Order()
    {
        // Arrange
        var edit = CreateValidEdit();
        var errorMessage = "Test error message";
        var cancellationToken = CancellationToken.None;

        var callOrder = new List<string>();

        _notificationProducer.SendAsync(Arg.Any<NotificationMessage>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(_ => callOrder.Add("SendAsync"));

        _videoEditClient.UpdateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<EditStatus>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(_ => callOrder.Add("UpdateAsync"));

        // Act
        await _sut.SendErrorAsync(edit, errorMessage, cancellationToken);

        // Assert
        callOrder.Should().HaveCount(2);
        callOrder[0].Should().Be("SendAsync");
        callOrder[1].Should().Be("UpdateAsync");
    }

    [Theory]
    [InlineData("Error processing video")]
    [InlineData("Timeout occurred")]
    [InlineData("Invalid format")]
    public async Task When_SendErrorAsync_Called_With_Different_Error_Messages_Then_Send_Correct_Message(string errorMessage)
    {
        // Arrange
        var edit = CreateValidEdit();
        var cancellationToken = CancellationToken.None;

        NotificationMessage? capturedMessage = null;
        await _notificationProducer.SendAsync(Arg.Do<NotificationMessage>(msg => capturedMessage = msg), Arg.Any<CancellationToken>());

        // Act
        await _sut.SendErrorAsync(edit, errorMessage, cancellationToken);

        // Assert
        capturedMessage.Should().NotBeNull();
        capturedMessage!.Error.Should().Be(errorMessage);
    }

    [Fact]
    public async Task When_SendErrorAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Dependencies()
    {
        // Arrange
        var edit = CreateValidEdit();
        var errorMessage = "Test error";
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Act
        await _sut.SendErrorAsync(edit, errorMessage, cancellationToken);

        // Assert
        await _notificationProducer.Received(1).SendAsync(Arg.Any<NotificationMessage>(), cancellationToken);
        await _videoEditClient.Received(1).UpdateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<EditStatus>(), cancellationToken);
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
