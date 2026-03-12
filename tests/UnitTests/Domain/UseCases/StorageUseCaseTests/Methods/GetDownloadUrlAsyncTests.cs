using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.StorageUseCaseTests.Methods;

public class GetDownloadUrlAsyncTests : StorageUseCaseDependenciesMock
{
    [Theory]
    [InlineData("users/user123/Frame/edit456.zip")]
    [InlineData("files/video.mp4")]
    [InlineData("path/to/file.txt")]
    public async Task When_GetDownloadUrlAsync_Called_Then_Return_Url_From_Storage_Client(string path)
    {
        // Arrange
        var expectedUrl = $"https://storage.example.com/{path}";
        var cancellationToken = CancellationToken.None;

        _storageClient.GetDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        var result = await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_Then_Call_StorageClient_GetDownloadUrlAsync()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var expectedUrl = "https://storage.example.com/file.zip";
        var cancellationToken = CancellationToken.None;

        _storageClient.GetDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        await _storageClient.Received(1).GetDownloadUrlAsync(path, cancellationToken);
    }

    [Fact]
    public async Task When_GetDownloadUrlAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Storage_Client()
    {
        // Arrange
        var path = "users/user123/Frame/edit456.zip";
        var expectedUrl = "https://storage.example.com/file.zip";
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _storageClient.GetDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        await _storageClient.Received(1).GetDownloadUrlAsync(path, cancellationToken);
    }

    [Theory]
    [InlineData("")]
    [InlineData("simple-path")]
    [InlineData("complex/nested/deep/path/file.ext")]
    public async Task When_GetDownloadUrlAsync_Called_With_Different_Paths_Then_Return_Correct_Url(string path)
    {
        // Arrange
        var expectedUrl = $"https://cdn.example.com/{path}";
        var cancellationToken = CancellationToken.None;

        _storageClient.GetDownloadUrlAsync(path, cancellationToken)
            .Returns(expectedUrl);

        // Act
        var result = await _sut.GetDownloadUrlAsync(path, cancellationToken);

        // Assert
        result.Should().Be(expectedUrl);
        await _storageClient.Received(1).GetDownloadUrlAsync(path, cancellationToken);
    }
}
