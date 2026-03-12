using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Properties;

public class NotificationTargetsPropertyTests : EditDependenciesMock
{
    [Fact]
    public void When_Set_NotificationTargets_Then_Property_Should_Be_Set()
    {
        // Arrange
        var edit = CreateValidEdit();
        var newTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "new@example.com")
        };

        // Act
        edit.NotificationTargets = newTargets;

        // Assert
        edit.NotificationTargets.Should().BeEquivalentTo(newTargets);
    }

    [Fact]
    public void When_Set_Empty_NotificationTargets_Then_Property_Should_Be_Empty()
    {
        // Arrange
        var edit = CreateValidEdit();
        var emptyTargets = new List<NotificationTarget>();

        // Act
        edit.NotificationTargets = emptyTargets;

        // Assert
        edit.NotificationTargets.Should().BeEmpty();
    }

    [Fact]
    public void When_Set_Multiple_NotificationTargets_Then_Property_Should_Contain_All()
    {
        // Arrange
        var edit = CreateValidEdit();
        var targets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "email1@example.com"),
            new NotificationTarget(NotificationChannel.Email, "email2@example.com"),
            new NotificationTarget(NotificationChannel.Webhook, "https://webhook.example.com")
        };

        // Act
        edit.NotificationTargets = targets;

        // Assert
        edit.NotificationTargets.Should().HaveCount(3);
        edit.NotificationTargets.Should().BeEquivalentTo(targets);
    }

    [Fact]
    public void When_Set_NotificationTargets_With_Different_Channels_Then_Property_Should_Contain_All()
    {
        // Arrange
        var edit = CreateValidEdit();
        var targets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "test@example.com"),
            new NotificationTarget(NotificationChannel.Webhook, "https://webhook.example.com")
        };

        // Act
        edit.NotificationTargets = targets;

        // Assert
        edit.NotificationTargets.Should().HaveCount(2);
        edit.NotificationTargets.Should().Contain(t => t.Channel == NotificationChannel.Email);
        edit.NotificationTargets.Should().Contain(t => t.Channel == NotificationChannel.Webhook);
    }

    private Edit CreateValidEdit()
    {
        return new Edit(
            id: "edit-123",
            userId: "user-456",
            userName: "Test User",
            videoPath: "/path/to/video.mp4",
            type: EditType.Frame,
            notificationTargets: new List<NotificationTarget>());
    }
}
