using Domain.Entities.Enums;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Extractors.Interfaces;
using Domain.UseCases;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.FrameEditUseCaseTests.Constructor;

public class FrameEditUseCaseConstructorTests : FrameEditUseCaseDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var extractor = Substitute.For<IVideoFrameExtractor>();
        var storage = Substitute.For<IStorageClient>();

        // Act
        var result = new FrameEditUseCase(extractor, storage);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<FrameEditUseCase>();
    }

    [Fact]
    public void When_Instance_Created_Then_EditType_Should_Be_Frame()
    {
        // Arrange & Act
        var result = _sut.EditType;

        // Assert
        result.Should().Be(EditType.Frame);
    }
}
