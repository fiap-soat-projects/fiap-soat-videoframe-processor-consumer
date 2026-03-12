using Domain.Entities;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoJobTests.Properties;

public class OutputKeyPropertyTests : VideoJobDependenciesMock
{
    [Fact]
    public void When_OutputKey_Is_Set_Via_Constructor_Then_Property_Should_Return_Correct_Value()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var outputKey = "users/user123/Frame/edit456.zip";

        // Act
        var videoJob = new VideoJob(videoUrl, outputKey);

        // Assert
        videoJob.OutputKey.Should().Be(outputKey);
    }

    [Theory]
    [InlineData("output/path1.zip")]
    [InlineData("users/user456/Frame/edit789.zip")]
    [InlineData("temp/output.zip")]
    [InlineData("deeply/nested/folder/structure/file.zip")]
    public void When_Different_OutputKeys_Then_Property_Should_Return_Correct_Values(string outputKey)
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";

        // Act
        var videoJob = new VideoJob(videoUrl, outputKey);

        // Assert
        videoJob.OutputKey.Should().Be(outputKey);
    }

    [Fact]
    public void When_OutputKey_Property_Accessed_Multiple_Times_Then_Always_Return_Same_Value()
    {
        // Arrange
        var videoUrl = "https://example.com/video.mp4";
        var outputKey = "users/user123/Frame/edit456.zip";
        var videoJob = new VideoJob(videoUrl, outputKey);

        // Act
        var result1 = videoJob.OutputKey;
        var result2 = videoJob.OutputKey;
        var result3 = videoJob.OutputKey;

        // Assert
        result1.Should().Be(outputKey);
        result2.Should().Be(outputKey);
        result3.Should().Be(outputKey);
        result1.Should().Be(result2);
        result2.Should().Be(result3);
    }

    [Fact]
    public void When_OutputKey_Is_Read_Only_Then_Should_Not_Have_Setter()
    {
        // Arrange
        var propertyInfo = typeof(VideoJob).GetProperty(nameof(VideoJob.OutputKey));

        // Act & Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo!.CanRead.Should().BeTrue();
        propertyInfo.CanWrite.Should().BeFalse();
    }
}
