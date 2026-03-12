using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.EditTests.Properties;

public class TypePropertyTests : EditDependenciesMock
{
    [Fact]
    public void When_Set_Type_To_Frame_Then_Property_Should_Be_Frame()
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        edit.Type = EditType.Frame;

        // Assert
        edit.Type.Should().Be(EditType.Frame);
    }

    [Fact]
    public void When_Set_Type_To_None_Then_Property_Should_Be_None()
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        edit.Type = EditType.None;

        // Assert
        edit.Type.Should().Be(EditType.None);
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public void When_Set_Different_Types_Then_Property_Should_Be_Set_Correctly(EditType type)
    {
        // Arrange
        var edit = CreateValidEdit();

        // Act
        edit.Type = type;

        // Assert
        edit.Type.Should().Be(type);
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
