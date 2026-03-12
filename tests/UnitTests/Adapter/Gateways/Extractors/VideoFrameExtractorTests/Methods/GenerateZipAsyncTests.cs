using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Extractors.VideoFrameExtractorTests.Methods;

public class GenerateZipAsyncTests : VideoFrameExtractorDependenciesMock
{
    [Fact]
    public async Task When_GenerateZipAsync_Called_Then_Call_FfmpegFrameExtractor_GenerateZipAsync()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        await _ffmpegFrameExtractor.Received(1).GenerateZipAsync(videoUrl, output, cancellationToken);
    }

    [Theory]
    [InlineData("https://storage.example.com/video1.mp4")]
    [InlineData("https://cdn.example.com/video2.mp4")]
    [InlineData("s3://bucket/video3.mp4")]
    public async Task When_GenerateZipAsync_Called_With_Different_VideoUrls_Then_Pass_To_Ffmpeg(string videoUrl)
    {
        // Arrange
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        await _ffmpegFrameExtractor.Received(1).GenerateZipAsync(
            videoUrl,
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_GenerateZipAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Ffmpeg()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var output = new MemoryStream();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        await _ffmpegFrameExtractor.Received(1).GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_GenerateZipAsync_Called_Then_Pass_Correct_Stream_To_Ffmpeg()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        await _ffmpegFrameExtractor.Received(1).GenerateZipAsync(
            videoUrl,
            output,
            cancellationToken);
    }

    [Fact]
    public async Task When_FfmpegFrameExtractor_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;
        var expectedException = new InvalidOperationException("FFmpeg extraction failed");

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("FFmpeg extraction failed");
    }

    [Fact]
    public async Task When_GenerateZipAsync_Completes_Successfully_Then_Return_Completed_Task()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        var result = _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        await result;
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task When_GenerateZipAsync_Called_Multiple_Times_Then_Call_Ffmpeg_Each_Time()
    {
        // Arrange
        var videoUrl1 = "https://example.com/video1.mp4";
        var videoUrl2 = "https://example.com/video2.mp4";
        var videoUrl3 = "https://example.com/video3.mp4";
        using var output1 = new MemoryStream();
        using var output2 = new MemoryStream();
        using var output3 = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _ffmpegFrameExtractor.GenerateZipAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.GenerateZipAsync(videoUrl1, output1, cancellationToken);
        await _sut.GenerateZipAsync(videoUrl2, output2, cancellationToken);
        await _sut.GenerateZipAsync(videoUrl3, output3, cancellationToken);

        // Assert
        await _ffmpegFrameExtractor.Received(3).GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_GenerateZipAsync_Called_Then_Pass_All_Parameters_Correctly()
    {
        // Arrange
        var videoUrl = "https://storage.example.com/test-video.mp4";
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        string? capturedVideoUrl = null;
        Stream? capturedStream = null;
        CancellationToken? capturedToken = null;

        await _ffmpegFrameExtractor.GenerateZipAsync(
            Arg.Do<string>(v => capturedVideoUrl = v),
            Arg.Do<Stream>(s => capturedStream = s),
            Arg.Do<CancellationToken>(t => capturedToken = t));

        // Act
        await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        capturedVideoUrl.Should().Be(videoUrl);
        capturedStream.Should().BeSameAs(output);
        capturedToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task When_FfmpegFrameExtractor_Throws_TaskCanceledException_Then_Propagate_Exception()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var output = new MemoryStream();
        var cancellationToken = CancellationToken.None;
        var expectedException = new TaskCanceledException("Operation was cancelled");

        _ffmpegFrameExtractor.GenerateZipAsync(videoUrl, output, cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.GenerateZipAsync(videoUrl, output, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>()
            .WithMessage("Operation was cancelled");
    }

    [Fact]
    public async Task When_GenerateZipAsync_Called_With_Different_Streams_Then_Pass_Correct_Stream_To_Ffmpeg()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        using var stream1 = new MemoryStream();
        using var stream2 = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _ffmpegFrameExtractor.GenerateZipAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.GenerateZipAsync(videoUrl, stream1, cancellationToken);
        await _sut.GenerateZipAsync(videoUrl, stream2, cancellationToken);

        // Assert
        await _ffmpegFrameExtractor.Received(1).GenerateZipAsync(videoUrl, stream1, cancellationToken);
        await _ffmpegFrameExtractor.Received(1).GenerateZipAsync(videoUrl, stream2, cancellationToken);
    }
}
