using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.FrameEditUseCaseTests.Methods;

public class ProcessAsyncTests : FrameEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_ProcessAsync_Called_Then_Call_Extractor_GenerateZipAsync()
    {
        // Arrange
        var job = CreateValidVideoJob();
        var cancellationToken = CancellationToken.None;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await _extractor.Received(1).GenerateZipAsync(
            job.VideoUrl,
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Call_Storage_UploadAsync()
    {
        // Arrange
        var job = CreateValidVideoJob();
        var cancellationToken = CancellationToken.None;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await _storage.Received(1).UploadAsync(
            job.OutputKey,
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Pass_Correct_VideoUrl_To_Extractor()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var job = new VideoJob(videoUrl, "output/key.zip");
        var cancellationToken = CancellationToken.None;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await _extractor.Received(1).GenerateZipAsync(
            videoUrl,
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_Then_Pass_Correct_OutputKey_To_Storage()
    {
        // Arrange
        var outputKey = "users/user123/Frame/edit456.zip";
        var job = new VideoJob("https://example.com/video.mp4", outputKey);
        var cancellationToken = CancellationToken.None;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await _storage.Received(1).UploadAsync(
            outputKey,
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Called_With_Custom_CancellationToken_Then_Pass_Token_To_Dependencies()
    {
        // Arrange
        var job = CreateValidVideoJob();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await _extractor.Received(1).GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            cancellationToken);

        await _storage.Received(1).UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_GenerateZipAsync_Throws_Exception_Then_Rethrow_Exception()
    {
        // Arrange
        var job = CreateValidVideoJob();
        var cancellationToken = CancellationToken.None;
        var expectedException = new InvalidOperationException("Extraction failed");

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var act = async () => await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Extraction failed");
    }

    [Fact]
    public async Task When_GenerateZipAsync_Throws_Exception_Then_Still_Call_Storage_UploadAsync()
    {
        // Arrange
        var job = CreateValidVideoJob();
        var cancellationToken = CancellationToken.None;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new InvalidOperationException("Extraction failed")));

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        try
        {
            await _sut.ProcessAsync(job, cancellationToken);
        }
        catch
        {
            // Exception expected
        }

        // Assert
        await _storage.Received(1).UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("https://example.com/video1.mp4", "output/path1.zip")]
    [InlineData("https://storage.com/video2.mp4", "users/user123/Frame/edit456.zip")]
    [InlineData("s3://bucket/video3.mp4", "temp/output.zip")]
    public async Task When_ProcessAsync_Called_With_Different_Jobs_Then_Process_Correctly(
        string videoUrl, string outputKey)
    {
        // Arrange
        var job = new VideoJob(videoUrl, outputKey);
        var cancellationToken = CancellationToken.None;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        await _extractor.Received(1).GenerateZipAsync(
            videoUrl,
            Arg.Any<Stream>(),
            cancellationToken);

        await _storage.Received(1).UploadAsync(
            outputKey,
            Arg.Any<Stream>(),
            cancellationToken);
    }

    [Fact]
    public async Task When_ProcessAsync_Completes_Successfully_Then_Both_Operations_Complete()
    {
        // Arrange
        var job = CreateValidVideoJob();
        var cancellationToken = CancellationToken.None;
        var extractorCompleted = false;
        var uploadCompleted = false;

        _extractor.GenerateZipAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(async _ =>
            {
                await Task.Delay(10);
                extractorCompleted = true;
            });

        _storage.UploadAsync(
            Arg.Any<string>(),
            Arg.Any<Stream>(),
            Arg.Any<CancellationToken>())
            .Returns(async _ =>
            {
                await Task.Delay(10);
                uploadCompleted = true;
            });

        // Act
        await _sut.ProcessAsync(job, cancellationToken);

        // Assert
        extractorCompleted.Should().BeTrue();
        uploadCompleted.Should().BeTrue();
    }

    private VideoJob CreateValidVideoJob()
    {
        return new VideoJob(
            videoUrl: "https://example.com/test-video.mp4",
            outputKey: "users/user123/Frame/edit456.zip");
    }
}
