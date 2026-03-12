using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.NotificationTargetTests.Properties;

public class ChannelPropertyTests : NotificationTargetDependenciesMock
{
    [Fact]
    public void When_Set_Channel_To_Email_Then_Property_Should_Be_Email()
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act
        notificationTarget.Channel = NotificationChannel.Email;

        // Assert
        notificationTarget.Channel.Should().Be(NotificationChannel.Email);
    }

    [Fact]
    public void When_Set_Channel_To_Webhook_Then_Property_Should_Be_Webhook()
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act
        notificationTarget.Channel = NotificationChannel.Webhook;

        // Assert
        notificationTarget.Channel.Should().Be(NotificationChannel.Webhook);
    }

    [Theory]
    [InlineData(NotificationChannel.Email)]
    [InlineData(NotificationChannel.Webhook)]
    public void When_Set_Different_Channels_Then_Property_Should_Be_Set_Correctly(NotificationChannel channel)
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act
        notificationTarget.Channel = channel;

        // Assert
        notificationTarget.Channel.Should().Be(channel);
    }

    [Fact]
    public void When_Change_Channel_From_Email_To_Webhook_Then_Property_Should_Be_Updated()
    {
        // Arrange
        var notificationTarget = new NotificationTarget(NotificationChannel.Email, "test@example.com");

        // Act
        notificationTarget.Channel = NotificationChannel.Webhook;

        // Assert
        notificationTarget.Channel.Should().Be(NotificationChannel.Webhook);
    }

    [Fact]
    public void When_Change_Channel_From_Webhook_To_Email_Then_Property_Should_Be_Updated()
    {
        // Arrange
        var notificationTarget = new NotificationTarget(NotificationChannel.Webhook, "https://webhook.example.com");

        // Act
        notificationTarget.Channel = NotificationChannel.Email;

        // Assert
        notificationTarget.Channel.Should().Be(NotificationChannel.Email);
    }

    private NotificationTarget CreateValidNotificationTarget()
    {
        return new NotificationTarget(NotificationChannel.Email, "test@example.com");
    }
}
