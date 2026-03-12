using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.StorageClientTests.Methods;

public class UploadAsyncTests : StorageClientDependenciesMock
{
    [Fact]
    public async Task When_UploadAsync_Called_Then_Call_S3BucketClient_UploadAsync()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).UploadAsync(key, data, cancellationToken);
    }

    [Theory]
    [InlineData("users/user1/Frame/edit1.zip")]
    [InlineData("output/video.mp4")]
    [InlineData("temp/file.txt")]
    public async Task When_UploadAsync_Called_With_Different_Keys_Then_Pass_Correct_Key_To_S3(string key)
    {
        // Arrange
        using var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).UploadAsync(key, Arg.Any<Stream>(), cancellationToken);
    }

    [Fact]
    public async Task When_UploadAsync_Called_Then_Pass_Correct_Stream_To_S3()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).UploadAsync(
            key,
            data,
            cancellationToken);
    }

    [Fact]
    public async Task When_UploadAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_S3()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var data = new MemoryStream();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_S3BucketClient_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;
        var expectedException = new InvalidOperationException("S3 upload failed");

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("S3 upload failed");
    }

    [Fact]
    public async Task When_UploadAsync_Completes_Successfully_Then_Return_Completed_Task()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        var result = _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        await result;
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task When_UploadAsync_Called_Multiple_Times_Then_Call_S3_Each_Time()
    {
        // Arrange
        var key1 = "key1.zip";
        var key2 = "key2.zip";
        var key3 = "key3.zip";
        using var data1 = new MemoryStream();
        using var data2 = new MemoryStream();
        using var data3 = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key1, data1, cancellationToken);
        await _sut.UploadAsync(key2, data2, cancellationToken);
        await _sut.UploadAsync(key3, data3, cancellationToken);

        // Assert
        await _s3BucketClient.Received(3).UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_UploadAsync_Called_Then_Pass_All_Parameters_Correctly()
    {
        // Arrange
        var key = "specific-key/file.zip";
        using var data = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        var cancellationToken = CancellationToken.None;

        string? capturedKey = null;
        Stream? capturedStream = null;
        CancellationToken? capturedToken = null;

        await _s3BucketClient.UploadAsync(
            Arg.Do<string>(k => capturedKey = k),
            Arg.Do<Stream>(s => capturedStream = s),
            Arg.Do<CancellationToken>(t => capturedToken = t));

        // Act
        await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        capturedKey.Should().Be(key);
        capturedStream.Should().BeSameAs(data);
        capturedToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task When_UploadAsync_Called_With_Different_Streams_Then_Pass_Correct_Stream_To_S3()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var stream1 = new MemoryStream(new byte[] { 1, 2, 3 });
        using var stream2 = new MemoryStream(new byte[] { 4, 5, 6 });
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key, stream1, cancellationToken);
        await _sut.UploadAsync(key, stream2, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).UploadAsync(key, stream1, cancellationToken);
        await _s3BucketClient.Received(1).UploadAsync(key, stream2, cancellationToken);
    }

    [Fact]
    public async Task When_UploadAsync_Called_With_Empty_Stream_Then_Pass_Empty_Stream_To_S3()
    {
        // Arrange
        var key = "users/user123/Frame/edit456.zip";
        using var data = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClient.UploadAsync(key, data, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UploadAsync(key, data, cancellationToken);

        // Assert
        await _s3BucketClient.Received(1).UploadAsync(key, data, cancellationToken);
    }
}
