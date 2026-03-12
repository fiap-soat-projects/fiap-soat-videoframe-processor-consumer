using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.FrameEditUseCaseTests.Properties;

public class EditTypeTests : FrameEditUseCaseDependenciesMock
{
    [Fact]
    public void When_EditType_Accessed_Then_Return_Frame()
    {
        // Arrange & Act
        var result = _sut.EditType;

        // Assert
        result.Should().Be(EditType.Frame);
    }

    [Fact]
    public void When_EditType_Accessed_Multiple_Times_Then_Always_Return_Frame()
    {
        // Arrange & Act
        var result1 = _sut.EditType;
        var result2 = _sut.EditType;
        var result3 = _sut.EditType;

        // Assert
        result1.Should().Be(EditType.Frame);
        result2.Should().Be(EditType.Frame);
        result3.Should().Be(EditType.Frame);
    }
}
