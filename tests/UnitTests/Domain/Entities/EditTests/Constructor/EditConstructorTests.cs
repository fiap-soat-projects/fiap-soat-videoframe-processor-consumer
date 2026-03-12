using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Constructor;

public class EditConstructorTests : EditDependenciesMock
{
    [Fact]
    public void When_Valid_Parameters_Then_Create_Instance_Successfully()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "test@example.com")
        };

        // Act
        var result = new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
        result.UserName.Should().Be(userName);
        result.VideoPath.Should().Be(videoPath);
        result.Type.Should().Be(type);
        result.NotificationTargets.Should().BeEquivalentTo(notificationTargets);
    }

    [Fact]
    public void When_Id_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? id = null;
        var userId = "user-456";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property Id of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Id_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string id)
    {
        // Arrange
        var userId = "user-456";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property Id of Edit is invalid");
    }

    [Fact]
    public void When_UserId_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var id = "edit-123";
        string? userId = null;
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserId of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_UserId_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string userId)
    {
        // Arrange
        var id = "edit-123";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserId of Edit is invalid");
    }

    [Fact]
    public void When_UserName_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        string? userName = null;
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserName of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_UserName_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string userName)
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserName of Edit is invalid");
    }

    [Fact]
    public void When_VideoPath_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var userName = "Test User";
        string? videoPath = null;
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property VideoPath of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_VideoPath_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string videoPath)
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var userName = "Test User";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var act = () => new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property VideoPath of Edit is invalid");
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public void When_Different_EditTypes_Then_Create_Instance_With_Correct_Type(EditType type)
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var result = new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        result.Type.Should().Be(type);
    }

    [Fact]
    public void When_Empty_NotificationTargets_Then_Create_Instance_Successfully()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var result = new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        result.NotificationTargets.Should().BeEmpty();
    }

    [Fact]
    public void When_Multiple_NotificationTargets_Then_Create_Instance_With_All_Targets()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var userName = "Test User";
        var videoPath = "/path/to/video.mp4";
        var type = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "email@example.com"),
            new NotificationTarget(NotificationChannel.Webhook, "https://webhook.example.com")
        };

        // Act
        var result = new Edit(id, userId, userName, videoPath, type, notificationTargets);

        // Assert
        result.NotificationTargets.Should().HaveCount(2);
        result.NotificationTargets.Should().BeEquivalentTo(notificationTargets);
    }
}
