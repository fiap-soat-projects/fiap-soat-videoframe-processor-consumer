using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Properties;

public class VideoPathPropertyTests : EditDependenciesMock
{
    [Fact]
    public void When_Set_Valid_VideoPath_Then_Property_Should_Be_Set()
    {
        // Arrange
        var edit = CreateValidEdit();
        var newVideoPath = "/new/path/to/video.mp4";

        // Act
        edit.VideoPath = newVideoPath;

        // Assert
        edit.VideoPath.Should().Be(newVideoPath);
    }

    [Fact]
    public void When_Set_Null_VideoPath_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.VideoPath = null;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property VideoPath of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Set_Empty_Or_Whitespace_VideoPath_Then_Throw_InvalidEntityPropertyException(string videoPath)
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.VideoPath = videoPath;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property VideoPath of Edit is invalid");
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
