using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Properties;

public class UserNamePropertyTests : EditDependenciesMock
{
    [Fact]
    public void When_Set_Valid_UserName_Then_Property_Should_Be_Set()
    {
        // Arrange
        var edit = CreateValidEdit();
        var newUserName = "New User Name";

        // Act
        edit.UserName = newUserName;

        // Assert
        edit.UserName.Should().Be(newUserName);
    }

    [Fact]
    public void When_Set_Null_UserName_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.UserName = null;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserName of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Set_Empty_Or_Whitespace_UserName_Then_Throw_InvalidEntityPropertyException(string userName)
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.UserName = userName;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property UserName of Edit is invalid");
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
