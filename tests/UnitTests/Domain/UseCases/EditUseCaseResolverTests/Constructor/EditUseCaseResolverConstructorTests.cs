using Domain.UseCases;
using Domain.UseCases.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Domain.UseCases.EditUseCaseResolverTests.Constructor;

public class EditUseCaseResolverConstructorTests : EditUseCaseResolverDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var logger = Substitute.For<ILogger<EditUseCaseResolver>>();
        var editUseCases = Substitute.For<IEnumerable<IEditUseCase>>();

        // Act
        var result = new EditUseCaseResolver(logger, editUseCases);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<EditUseCaseResolver>();
    }
}
