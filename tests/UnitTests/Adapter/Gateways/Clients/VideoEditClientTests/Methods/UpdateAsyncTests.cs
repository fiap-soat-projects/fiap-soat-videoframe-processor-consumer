using Domain.Entities.Enums;
using Infrastructure.Clients.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.VideoEditClientTests.Methods;

public class UpdateAsyncTests : VideoEditClientDependenciesMock
{
    [Fact]
    public async Task When_UpdateAsync_Called_Then_Call_VideoFrameClient_UpdateEditStatusAsync()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        await _videoFrameClient.Received(1).UpdateEditStatusAsync(
            editId,
            Arg.Any<UpdateEditionStatusRequest>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_UpdateAsync_Called_Then_Pass_Correct_EditId_To_VideoFrameClient()
    {
        // Arrange
        var editId = "edit-789";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        await _videoFrameClient.Received(1).UpdateEditStatusAsync(
            editId,
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_UpdateAsync_Called_Then_Create_UpdateEditionStatusRequest_With_Correct_UserId()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-999";
        var editStatus = EditStatus.Error;
        var cancellationToken = CancellationToken.None;

        UpdateEditionStatusRequest? capturedRequest = null;
        await _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Do<UpdateEditionStatusRequest>(req => capturedRequest = req),
            Arg.Any<CancellationToken>());

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.userId.Should().Be(userId);
    }

    [Fact]
    public async Task When_UpdateAsync_Called_Then_Create_UpdateEditionStatusRequest_With_Correct_Status()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;

        UpdateEditionStatusRequest? capturedRequest = null;
        await _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Do<UpdateEditionStatusRequest>(req => capturedRequest = req),
            Arg.Any<CancellationToken>());

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Status.Should().Be(editStatus);
    }

    [Theory]
    [InlineData(EditStatus.None)]
    [InlineData(EditStatus.Created)]
    [InlineData(EditStatus.Processing)]
    [InlineData(EditStatus.Processed)]
    [InlineData(EditStatus.Error)]
    public async Task When_UpdateAsync_Called_With_Different_EditStatus_Then_Pass_Correct_Status(EditStatus editStatus)
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        UpdateEditionStatusRequest? capturedRequest = null;
        await _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Do<UpdateEditionStatusRequest>(req => capturedRequest = req),
            Arg.Any<CancellationToken>());

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Status.Should().Be(editStatus);
    }

    [Fact]
    public async Task When_UpdateAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_VideoFrameClient()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        await _videoFrameClient.Received(1).UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_VideoFrameClient_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;
        var expectedException = new InvalidOperationException("API call failed");

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("API call failed");
    }

    [Fact]
    public async Task When_UpdateAsync_Completes_Successfully_Then_Return_Completed_Task()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var result = _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        await result;
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task When_UpdateAsync_Called_Multiple_Times_Then_Call_VideoFrameClient_Each_Time()
    {
        // Arrange
        var editId1 = "edit-111";
        var editId2 = "edit-222";
        var editId3 = "edit-333";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(editId1, userId, editStatus, cancellationToken);
        await _sut.UpdateAsync(editId2, userId, editStatus, cancellationToken);
        await _sut.UpdateAsync(editId3, userId, editStatus, cancellationToken);

        // Assert
        await _videoFrameClient.Received(3).UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_UpdateAsync_Called_Then_Pass_All_Parameters_Correctly()
    {
        // Arrange
        var editId = "edit-specific-123";
        var userId = "user-specific-456";
        var editStatus = EditStatus.Error;
        var cancellationToken = CancellationToken.None;

        string? capturedEditId = null;
        UpdateEditionStatusRequest? capturedRequest = null;
        CancellationToken? capturedToken = null;

        await _videoFrameClient.UpdateEditStatusAsync(
            Arg.Do<string>(id => capturedEditId = id),
            Arg.Do<UpdateEditionStatusRequest>(req => capturedRequest = req),
            Arg.Do<CancellationToken>(token => capturedToken = token));

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        capturedEditId.Should().Be(editId);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.userId.Should().Be(userId);
        capturedRequest.Status.Should().Be(editStatus);
        capturedToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task When_UpdateAsync_Called_With_Processed_Status_Then_Update_Successfully()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Processed;
        var cancellationToken = CancellationToken.None;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        await _videoFrameClient.Received(1).UpdateEditStatusAsync(
            editId,
            Arg.Is<UpdateEditionStatusRequest>(req => 
                req.userId == userId && 
                req.Status == EditStatus.Processed),
            cancellationToken);
    }

    [Fact]
    public async Task When_UpdateAsync_Called_With_Error_Status_Then_Update_Successfully()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-456";
        var editStatus = EditStatus.Error;
        var cancellationToken = CancellationToken.None;

        _videoFrameClient.UpdateEditStatusAsync(
            Arg.Any<string>(),
            Arg.Any<UpdateEditionStatusRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(editId, userId, editStatus, cancellationToken);

        // Assert
        await _videoFrameClient.Received(1).UpdateEditStatusAsync(
            editId,
            Arg.Is<UpdateEditionStatusRequest>(req => 
                req.userId == userId && 
                req.Status == EditStatus.Error),
            cancellationToken);
    }
}
