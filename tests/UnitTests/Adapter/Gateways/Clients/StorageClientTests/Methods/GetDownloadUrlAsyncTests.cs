using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.StorageClientTests.Methods;

public class GetDownloadUrlAsyncTests : StorageClientDependenciesMock
{
    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_Then_Call_S3BucketClient_GetPreSignedDownloadUrlAsync()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var expectedUrl = "https://s3.amazonaws.com/bucket/presigned-url";
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).GetPreSignedDownloadUrlAsync(path, cancellationToken);
    }

    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_Then_Return_Url_From_S3BucketClient()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var expectedUrl = "https://s3.amazonaws.com/bucket/presigned-url?token=abc123";
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        var result = await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("users/user1/Frame/edit1.zip")]
    [InlineData("files/video.mp4")]
    [InlineData("path/to/file.txt")]
    public async Task When_GetDownloadUrlAsync_Called_With_Different_Paths_Then_Pass_Correct_Path_To_S3(string path)
    {
        // Arrange
        var expectedUrl = $"https://s3.amazonaws.com/bucket/{path}?presigned=true";
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).GetPreSignedDownloadUrlAsync(path, cancellationToken);
    }

    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_S3()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var expectedUrl = "https://s3.amazonaws.com/bucket/presigned-url";
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).GetPreSignedDownloadUrlAsync(
            Arg.Any<string>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_S3BucketClient_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var cancellationToken = CancellationToken.None;
        var expectedException = new InvalidOperationException("S3 service unavailable");

        _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken)
            .Returns(Task.FromException<string>(expectedException));

        // Act
        var act = async () => await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("S3 service unavailable");
    }

    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_Multiple_Times_Then_Call_S3_Each_Time()
    {
        // Arrange
        var path1 = "path1.zip";
        var path2 = "path2.zip";
        var path3 = "path3.zip";
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.GetPreSignedDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("https://s3.amazonaws.com/url");

        // Act
        await _sut.GetDownloadUrlAsync(path1, cancellationToken);
        await _sut.GetDownloadUrlAsync(path2, cancellationToken);
        await _sut.GetDownloadUrlAsync(path3, cancellationToken);

        // Assert
        await _s3BucketClient.Received(3).GetPreSignedDownloadUrlAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_Then_Return_Task_With_String_Result()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var expectedUrl = "https://s3.amazonaws.com/bucket/presigned-url";
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.GetPreSignedDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        var result = _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Task<string>>();
        var url = await result;
        url.Should().Be(expectedUrl);
    }
}
