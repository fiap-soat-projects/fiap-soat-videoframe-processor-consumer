using Domain.Entities.Enums;
using Domain.UseCases.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Domain.UseCases.EditUseCaseResolverTests.Methods;

public class ResolveTests : EditUseCaseResolverDependenciesMock
{
    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public void When_EditType_Exists_Then_Return_Corresponding_UseCase(EditType editType)
    {
        // Arrange
        var mockUseCase = Substitute.For<IEditUseCase>();
        mockUseCase.EditType.Returns(editType);

        var editUseCases = new List<IEditUseCase> { mockUseCase };
        var sut = CreateSutWithEditUseCases(editUseCases);

        // Act
        var result = sut.Resolve(editType);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(mockUseCase);
        result.EditType.Should().Be(editType);
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public void When_EditType_Not_Found_Then_Throw_Exception(EditType editType)
    {
        // Arrange
        var editUseCases = new List<IEditUseCase>();
        var sut = CreateSutWithEditUseCases(editUseCases);

        // Act
        var act = () => sut.Resolve(editType);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("EditType not supported");
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public void When_EditType_Not_Found_Then_Log_Error(EditType editType)
    {
        // Arrange
        var editUseCases = new List<IEditUseCase>();
        var sut = CreateSutWithEditUseCases(editUseCases);

        // Act
        try
        {
            sut.Resolve(editType);
        }
        catch
        {
            // Exception is expected
        }

        // Assert
        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains("EditType not supported")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    [Fact]
    public void When_Multiple_UseCases_Exist_Then_Return_Correct_One()
    {
        // Arrange
        var frameUseCase = Substitute.For<IEditUseCase>();
        frameUseCase.EditType.Returns(EditType.Frame);

        var noneUseCase = Substitute.For<IEditUseCase>();
        noneUseCase.EditType.Returns(EditType.None);

        var editUseCases = new List<IEditUseCase> { frameUseCase, noneUseCase };
        var sut = CreateSutWithEditUseCases(editUseCases);

        // Act
        var result = sut.Resolve(EditType.Frame);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(frameUseCase);
        result.EditType.Should().Be(EditType.Frame);
    }

    [Fact]
    public void When_Empty_EditUseCases_Collection_Then_Throw_Exception()
    {
        // Arrange
        var editUseCases = new List<IEditUseCase>();
        var sut = CreateSutWithEditUseCases(editUseCases);

        // Act
        var act = () => sut.Resolve(EditType.Frame);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("EditType not supported");
    }
}
