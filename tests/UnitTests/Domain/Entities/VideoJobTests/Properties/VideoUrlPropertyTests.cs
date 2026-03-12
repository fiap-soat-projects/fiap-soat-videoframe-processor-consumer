using Domain.Entities;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoJobTests.Properties;

public class VideoUrlPropertyTests : VideoJobDependenciesMock
{
    [Fact]
    public void When_VideoUrl_Is_Set_Via_Constructor_Then_Property_Should_Return_Correct_Value()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var outputKey = "output/key.zip";

        // Act
        var videoJob = new VideoJob(videoUrl, outputKey);

        // Assert
        videoJob.VideoUrl.Should().Be(videoUrl);
    }

    [Theory]
    [InlineData("https://storage.example.com/video1.mp4")]
    [InlineData("https://cdn.example.com/video2.mp4")]
    [InlineData("s3://bucket/video3.mp4")]
    [InlineData("http://localhost:8080/video.mp4")]
    public void When_Different_VideoUrls_Then_Property_Should_Return_Correct_Values(string videoUrl)
    {
        // Arrange
        var outputKey = "output/key.zip";

        // Act
        var videoJob = new VideoJob(videoUrl, outputKey);

        // Assert
        videoJob.VideoUrl.Should().Be(videoUrl);
    }

    [Fact]
    public void When_VideoUrl_Property_Accessed_Multiple_Times_Then_Always_Return_Same_Value()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var outputKey = "output/key.zip";
        var videoJob = new VideoJob(videoUrl, outputKey);

        // Act
        var result1 = videoJob.VideoUrl;
        var result2 = videoJob.VideoUrl;
        var result3 = videoJob.VideoUrl;

        // Assert
        result1.Should().Be(videoUrl);
        result2.Should().Be(videoUrl);
        result3.Should().Be(videoUrl);
        result1.Should().Be(result2);
        result2.Should().Be(result3);
    }

    [Fact]
    public void When_VideoUrl_Is_Read_Only_Then_Should_Not_Have_Setter()
    {
        // Arrange
        var propertyInfo = typeof(VideoJob).GetProperty(nameof(VideoJob.VideoUrl));

        // Act & Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo!.CanRead.Should().BeTrue();
        propertyInfo.CanWrite.Should().BeFalse();
    }
}
