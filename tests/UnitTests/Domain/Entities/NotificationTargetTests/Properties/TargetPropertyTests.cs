using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.NotificationTargetTests.Properties;

public class TargetPropertyTests : NotificationTargetDependenciesMock
{
    [Fact]
    public void When_Set_Valid_Target_Then_Property_Should_Be_Set()
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();
        var newTarget = "newemail@example.com";

        // Act
        notificationTarget.Target = newTarget;

        // Assert
        notificationTarget.Target.Should().Be(newTarget);
    }

    [Fact]
    public void When_Set_Null_Target_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act
        var act = () => notificationTarget.Target = null;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<NotificationTarget>>()
            .WithMessage("The property Target of NotificationTarget is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Set_Empty_Or_Whitespace_Target_Then_Throw_InvalidEntityPropertyException(string target)
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act
        var act = () => notificationTarget.Target = target;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<NotificationTarget>>()
            .WithMessage("The property Target of NotificationTarget is invalid");
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("admin@company.org")]
    [InlineData("contact@domain.net")]
    public void When_Set_Different_Email_Targets_Then_Property_Should_Be_Set_Correctly(string target)
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act
        notificationTarget.Target = target;

        // Assert
        notificationTarget.Target.Should().Be(target);
    }

    [Theory]
    [InlineData("https://webhook1.example.com")]
    [InlineData("https://api.example.com/notify")]
    [InlineData("http://localhost:8080/webhook")]
    public void When_Set_Different_Webhook_Targets_Then_Property_Should_Be_Set_Correctly(string target)
    {
        // Arrange
        var notificationTarget = new NotificationTarget(NotificationChannel.Webhook, "https://initial.com");

        // Act
        notificationTarget.Target = target;

        // Assert
        notificationTarget.Target.Should().Be(target);
    }

    [Fact]
    public void When_Change_Target_Multiple_Times_Then_Property_Should_Always_Reflect_Latest_Value()
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();

        // Act & Assert
        notificationTarget.Target = "first@example.com";
        notificationTarget.Target.Should().Be("first@example.com");

        notificationTarget.Target = "second@example.com";
        notificationTarget.Target.Should().Be("second@example.com");

        notificationTarget.Target = "third@example.com";
        notificationTarget.Target.Should().Be("third@example.com");
    }

    [Fact]
    public void When_Set_Target_With_Special_Characters_Then_Property_Should_Be_Set()
    {
        // Arrange
        var notificationTarget = CreateValidNotificationTarget();
        var target = "user+tag@example.com";

        // Act
        notificationTarget.Target = target;

        // Assert
        notificationTarget.Target.Should().Be(target);
    }

    private NotificationTarget CreateValidNotificationTarget()
    {
        return new NotificationTarget(NotificationChannel.Email, "test@example.com");
    }
}
