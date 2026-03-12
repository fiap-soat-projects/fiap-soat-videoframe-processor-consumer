using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Producers.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Producers.NotificationProducerTests.Methods;

public class SendAsyncTests : NotificationProducerDependenciesMock
{
    [Fact]
    public async Task When_SendAsync_Called_Then_Call_KafkaProducer_ProduceAsync()
    {
        // Arrange
        var notificationMessage = CreateValidNotificationMessage();
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(1).ProduceAsync(notificationMessage, cancellationToken);
    }

    [Fact]
    public async Task When_SendAsync_Called_With_Success_Notification_Then_Forward_To_Kafka()
    {
        // Arrange
        var notificationMessage = new NotificationMessage
        {
            EditId = "edit-123",
            UserId = "user-456",
            UserName = "Test User",
            Type = NotificationType.Success,
            NotificationTargets = new List<NotificationTarget>
            {
                new NotificationTarget(NotificationChannel.Email, "test@example.com")
            },
            FileUrl = "https://storage.example.com/file.zip"
        };
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(1).ProduceAsync(
            Arg.Is<NotificationMessage>(m =>
                m.EditId == notificationMessage.EditId &&
                m.UserId == notificationMessage.UserId &&
                m.UserName == notificationMessage.UserName &&
                m.Type == NotificationType.Success &&
                m.FileUrl == notificationMessage.FileUrl),
            cancellationToken);
    }

    [Fact]
    public async Task When_SendAsync_Called_With_Error_Notification_Then_Forward_To_Kafka()
    {
        // Arrange
        var notificationMessage = new NotificationMessage
        {
            EditId = "edit-123",
            UserId = "user-456",
            UserName = "Test User",
            Type = NotificationType.Error,
            NotificationTargets = new List<NotificationTarget>
            {
                new NotificationTarget(NotificationChannel.Email, "test@example.com")
            },
            Error = "Processing failed"
        };
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(1).ProduceAsync(
            Arg.Is<NotificationMessage>(m =>
                m.EditId == notificationMessage.EditId &&
                m.UserId == notificationMessage.UserId &&
                m.UserName == notificationMessage.UserName &&
                m.Type == NotificationType.Error &&
                m.Error == notificationMessage.Error),
            cancellationToken);
    }

    [Fact]
    public async Task When_SendAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Kafka()
    {
        // Arrange
        var notificationMessage = CreateValidNotificationMessage();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(1).ProduceAsync(
            Arg.Any<NotificationMessage>(),
            cancellationToken);
    }

    [Theory]
    [InlineData(NotificationType.Success)]
    [InlineData(NotificationType.Error)]
    public async Task When_SendAsync_Called_With_Different_Notification_Types_Then_Forward_Correctly(
        NotificationType notificationType)
    {
        // Arrange
        var notificationMessage = new NotificationMessage
        {
            EditId = "edit-123",
            UserId = "user-456",
            UserName = "Test User",
            Type = notificationType,
            NotificationTargets = new List<NotificationTarget>()
        };
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(1).ProduceAsync(
            Arg.Is<NotificationMessage>(m => m.Type == notificationType),
            cancellationToken);
    }

    [Fact]
    public async Task When_SendAsync_Called_With_Multiple_Notification_Targets_Then_Forward_All_Targets()
    {
        // Arrange
        var targets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "user1@example.com"),
            new NotificationTarget(NotificationChannel.Webhook, "https://webhook.example.com"),
            new NotificationTarget(NotificationChannel.Email, "user2@example.com")
        };

        var notificationMessage = new NotificationMessage
        {
            EditId = "edit-123",
            UserId = "user-456",
            UserName = "Test User",
            Type = NotificationType.Success,
            NotificationTargets = targets,
            FileUrl = "https://storage.example.com/file.zip"
        };
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(1).ProduceAsync(
            Arg.Is<NotificationMessage>(m => m.NotificationTargets.Count() == 3),
            cancellationToken);
    }

    [Fact]
    public async Task When_KafkaProducer_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var notificationMessage = CreateValidNotificationMessage();
        var cancellationToken = CancellationToken.None;
        var expectedException = new InvalidOperationException("Kafka producer failed");

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Kafka producer failed");
    }

    [Fact]
    public async Task When_SendAsync_Completes_Successfully_Then_Return_Completed_Task()
    {
        // Arrange
        var notificationMessage = CreateValidNotificationMessage();
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(notificationMessage, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        var result = _sut.SendAsync(notificationMessage, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        await result;
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task When_SendAsync_Called_Multiple_Times_Then_Call_Kafka_Each_Time()
    {
        // Arrange
        var notificationMessage1 = CreateValidNotificationMessage();
        var notificationMessage2 = CreateValidNotificationMessage();
        var notificationMessage3 = CreateValidNotificationMessage();
        var cancellationToken = CancellationToken.None;

        _kafkaNotificationProducer.ProduceAsync(Arg.Any<NotificationMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.SendAsync(notificationMessage1, cancellationToken);
        await _sut.SendAsync(notificationMessage2, cancellationToken);
        await _sut.SendAsync(notificationMessage3, cancellationToken);

        // Assert
        await _kafkaNotificationProducer.Received(3).ProduceAsync(
            Arg.Any<NotificationMessage>(),
            cancellationToken);
    }

    private NotificationMessage CreateValidNotificationMessage()
    {
        return new NotificationMessage
        {
            EditId = "edit-123",
            UserId = "user-456",
            UserName = "Test User",
            Type = NotificationType.Success,
            NotificationTargets = new List<NotificationTarget>
            {
                new NotificationTarget(NotificationChannel.Email, "test@example.com")
            },
            FileUrl = "https://storage.example.com/file.zip"
        };
    }
}
