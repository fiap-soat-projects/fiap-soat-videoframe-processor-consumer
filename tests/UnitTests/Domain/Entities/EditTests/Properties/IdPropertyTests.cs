using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Properties;

public class IdPropertyTests : EditDependenciesMock
{
    [Fact]
    public void When_Set_Valid_Id_Then_Property_Should_Be_Set()
    {
        // Arrange
        var edit = CreateValidEdit();
        var newId = "new-edit-id-789";

        // Act
        edit.Id = newId;

        // Assert
        edit.Id.Should().Be(newId);
    }

    [Fact]
    public void When_Set_Null_Id_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.Id = null;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property Id of Edit is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_Set_Empty_Or_Whitespace_Id_Then_Throw_InvalidEntityPropertyException(string id)
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        var act = () => edit.Id = id;

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<Edit>>()
            .WithMessage("The property Id of Edit is invalid");
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
