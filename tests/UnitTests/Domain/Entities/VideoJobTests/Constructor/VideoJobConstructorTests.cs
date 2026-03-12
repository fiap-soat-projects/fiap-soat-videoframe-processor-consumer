using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoJobTests.Constructor;

public class VideoJobConstructorTests : VideoJobDependenciesMock
{
    [Fact]
    public void When_Valid_Parameters_Then_Create_Instance_Successfully()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var outputKey = "users/user123/Frame/edit456.zip";

        // Act
        var result = new VideoJob(videoUrl, outputKey);

        // Assert
        result.Should().NotBeNull();
        result.VideoUrl.Should().Be(videoUrl);
        result.OutputKey.Should().Be(outputKey);
    }

    [Theory]
    [InlineData("https://storage.example.com/video1.mp4", "output/path1.zip")]
    [InlineData("https://cdn.example.com/video2.mp4", "users/user456/Frame/edit789.zip")]
    [InlineData("s3://bucket/video3.mp4", "temp/output.zip")]
    public void When_Different_Valid_Parameters_Then_Create_Instance_With_Correct_Values(
        string videoUrl, string outputKey)
    {
        // Arrange & Act
        var result = new VideoJob(videoUrl, outputKey);

        // Assert
        result.VideoUrl.Should().Be(videoUrl);
        result.OutputKey.Should().Be(outputKey);
    }

    [Fact]
    public void When_VideoUrl_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? videoUrl = null;
        var outputKey = "users/user123/Frame/edit456.zip";

        // Act
        var act = () => new VideoJob(videoUrl!, outputKey);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<VideoJob>>()
            .WithMessage("The property VideoUrl of VideoJob is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_VideoUrl_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string videoUrl)
    {
        // Arrange
        var outputKey = "users/user123/Frame/edit456.zip";

        // Act
        var act = () => new VideoJob(videoUrl, outputKey);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<VideoJob>>()
            .WithMessage("The property VideoUrl of VideoJob is invalid");
    }

    [Fact]
    public void When_OutputKey_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        string? outputKey = null;

        // Act
        var act = () => new VideoJob(videoUrl, outputKey!);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<VideoJob>>()
            .WithMessage("The property OutputKey of VideoJob is invalid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void When_OutputKey_Is_Empty_Or_Whitespace_Then_Throw_InvalidEntityPropertyException(string outputKey)
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";

        // Act
        var act = () => new VideoJob(videoUrl, outputKey);

        // Assert
        act.Should().Throw<InvalidEntityPropertyException<VideoJob>>()
            .WithMessage("The property OutputKey of VideoJob is invalid");
    }

    [Fact]
    public void When_Both_VideoUrl_And_OutputKey_Are_Valid_Then_Create_Sealed_Instance()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var outputKey = "users/user123/Frame/edit456.zip";

        // Act
        var result = new VideoJob(videoUrl, outputKey);

        // Assert
        result.Should().BeAssignableTo<VideoJob>();
        result.GetType().IsSealed.Should().BeTrue();
    }

    [Theory]
    [InlineData("http://example.com/video.mp4")]
    [InlineData("https://example.com/video.mp4")]
    [InlineData("ftp://example.com/video.mp4")]
    [InlineData("s3://bucket/video.mp4")]
    [InlineData("/local/path/to/video.mp4")]
    public void When_Different_VideoUrl_Formats_Then_Create_Instance_Successfully(string videoUrl)
    {
        // Arrange
        var outputKey = "output/key.zip";

        // Act
        var result = new VideoJob(videoUrl, outputKey);

        // Assert
        result.VideoUrl.Should().Be(videoUrl);
    }

    [Theory]
    [InlineData("output.zip")]
    [InlineData("path/to/output.zip")]
    [InlineData("users/user123/Frame/edit456.zip")]
    [InlineData("deeply/nested/folder/structure/file.zip")]
    public void When_Different_OutputKey_Formats_Then_Create_Instance_Successfully(string outputKey)
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";

        // Act
        var result = new VideoJob(videoUrl, outputKey);

        // Assert
        result.OutputKey.Should().Be(outputKey);
    }
}
