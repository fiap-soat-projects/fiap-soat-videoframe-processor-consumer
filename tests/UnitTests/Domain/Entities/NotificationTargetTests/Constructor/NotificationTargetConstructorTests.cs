using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.NotificationTargetTests.Constructor;

public class NotificationTargetConstructorTests : NotificationTargetDependenciesMock
{
    [Fact]
    public void When_Valid_Parameters_Then_Create_Instance_Successfully()
    {
        // Arrange
        var channel = NotificationChannel.Email;
        var target = "test@example.com";

        // Act
        var result = new NotificationTarget(channel, target);

        // Assert
        result.Should().NotBeNull();
        result.Channel.Should().Be(channel);
        result.Target.Should().Be(target);
    }

    [Fact]
    public void When_Channel_Is_Email_Then_Create_Instance_With_Email_Channel()
    {
        // Arrange
        var channel = NotificationChannel.Email;
        var target = "email@example.com";

        // Act
        var result = new NotificationTarget(channel, target);

        // Assert
        result.Channel.Should().Be(NotificationChannel.Email);
        result.Target.Should().Be(target);
    }

    [Fact]
    public void When_Channel_Is_Webhook_Then_Create_Instance_With_Webhook_Channel()
    {
        // Arrange
        var channel = NotificationChannel.Webhook;
        var target = "https://webhook.example.com";

        // Act
        var result = new NotificationTarget(channel, target);

        // Assert
        result.Channel.Should().Be(NotificationChannel.Webhook);
        result.Target.Should().Be(target);
    }

    [Theory]
    [InlineData(NotificationChannel.Email, "user@example.com")]
    [InlineData(NotificationChannel.Webhook, "https://webhook.example.com/notify")]
    public void When_Different_Channels_And_Targets_Then_Create_Instance_Correctly(
        NotificationChannel channel, string target)
    {
        // Arrange & Act
        var result = new NotificationTarget(channel, target);

        // Assert
        result.Channel.Should().Be(channel);
        result.Target.Should().Be(target);
    }

    [Fact]
    public void When_Target_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var channel = NotificationChannel.Email;
        string? target = null;

        // Act
        var act = () => new NotificationTarget(channel, target);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<NotificationTarget>>()
            .WithMessage("The property Target of NotificationTarget is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Target_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string target)
    {
        // Arrange
        var channel = NotificationChannel.Email;

        // Act
        var act = () => new NotificationTarget(channel, target);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<NotificationTarget>>()
            .WithMessage("The property Target of NotificationTarget is invalid");
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.com")]
    [InlineData("admin@company.org")]
    public void When_Email_Channel_With_Different_Email_Addresses_Then_Create_Instance_Successfully(string target)
    {
        // Arrange
        var channel = NotificationChannel.Email;

        // Act
        var result = new NotificationTarget(channel, target);

        // Assert
        result.Channel.Should().Be(NotificationChannel.Email);
        result.Target.Should().Be(target);
    }

    [Theory]
    [InlineData("https://webhook.example.com")]
    [InlineData("https://api.example.com/notify")]
    [InlineData("http://localhost:8080/webhook")]
    public void When_Webhook_Channel_With_Different_Urls_Then_Create_Instance_Successfully(string target)
    {
        // Arrange
        var channel = NotificationChannel.Webhook;

        // Act
        var result = new NotificationTarget(channel, target);

        // Assert
        result.Channel.Should().Be(NotificationChannel.Webhook);
        result.Target.Should().Be(target);
    }
}
