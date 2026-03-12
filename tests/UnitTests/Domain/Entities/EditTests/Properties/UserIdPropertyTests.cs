using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Properties;

public class UserIdPropertyTests : EditDependenciesMock
{
    [Fact]
    public void When_Set_Valid_UserId_Then_Property_Should_Be_Set()
    {
        // Arrange
        var edit = CreateValidEdit();
        var newUserId = "new-user-id-789";

        // Act
        edit.UserId = newUserId;

        // Assert
        edit.UserId.Should().Be(newUserId);
    }

    [Fact]
    public void When_Set_Null_UserId_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.UserId = null;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserId of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Set_Empty_Or_Whitespace_UserId_Then_Throw_InvalidEntityPropertyException(string userId)
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.UserId = userId;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserId of Edit is invalid");
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
