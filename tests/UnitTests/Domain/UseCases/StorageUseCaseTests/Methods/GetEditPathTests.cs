using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.UseCases.StorageUseCaseTests.Methods;

public class GetEditPathTests : StorageUseCaseDependenciesMock
{
    [Fact]
    public void When_GetEditPath_Called_With_Frame_EditType_Then_Return_Correct_Path()
    {
        // Arrange
        var edit = CreateValidEdit("user123", "edit456", EditType.Frame);
        var expectedPath = "users/user123/Frame/edit456.zip";

        // Act
        var result = _sut.GetEditPath(edit);

        // Assert
        result.Should().Be(expectedPath);
    }

    [Fact]
    public void When_GetEditPath_Called_With_None_EditType_Then_Return_Correct_Path()
    {
        // Arrange
        var edit = CreateValidEdit("user789", "edit999", EditType.None);
        var expectedPath = "users/user789/None/edit999.zip";

        // Act
        var result = _sut.GetEditPath(edit);

        // Assert
        result.Should().Be(expectedPath);
    }

    [Theory]
    [InlineData("user1", "edit1", EditType.Frame, "users/user1/Frame/edit1.zip")]
    [InlineData("user2", "edit2", EditType.None, "users/user2/None/edit2.zip")]
    [InlineData("user-abc", "edit-xyz", EditType.Frame, "users/user-abc/Frame/edit-xyz.zip")]
    public void When_GetEditPath_Called_With_Different_Edits_Then_Return_Correct_Formatted_Path(
        string userId, string editId, EditType editType, string expectedPath)
    {
        // Arrange
        var edit = CreateValidEdit(userId, editId, editType);

        // Act
        var result = _sut.GetEditPath(edit);

        // Assert
        result.Should().Be(expectedPath);
    }

    [Fact]
    public void When_GetEditPath_Called_Then_Path_Should_Follow_Pattern_Users_UserId_Type_EditId_Zip()
    {
        // Arrange
        var userId = "test-user-123";
        var editId = "test-edit-456";
        var editType = EditType.Frame;
        var edit = CreateValidEdit(userId, editId, editType);

        // Act
        var result = _sut.GetEditPath(edit);

        // Assert
        result.Should().StartWith("users/");
        result.Should().Contain(userId);
        result.Should().Contain(editType.ToString());
        result.Should().Contain(editId);
        result.Should().EndWith(".zip");
    }

    [Fact]
    public void When_GetEditPath_Called_Then_Path_Components_Should_Be_In_Correct_Order()
    {
        // Arrange
        var userId = "user-abc";
        var editId = "edit-xyz";
        var editType = EditType.Frame;
        var edit = CreateValidEdit(userId, editId, editType);

        // Act
        var result = _sut.GetEditPath(edit);

        // Assert
        var parts = result.Split('/');
        parts.Should().HaveCount(4);
        parts[0].Should().Be("users");
        parts[1].Should().Be(userId);
        parts[2].Should().Be(editType.ToString());
        parts[3].Should().Be($"{editId}.zip");
    }

    private Edit CreateValidEdit(string userId, string editId, EditType editType)
    {
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "test@example.com")
        };

        return new Edit(
            id: editId,
            userId: userId,
            userName: "Test User",
            videoPath: "/path/to/video.mp4",
            type: editType,
            notificationTargets: notificationTargets);
    }
}
