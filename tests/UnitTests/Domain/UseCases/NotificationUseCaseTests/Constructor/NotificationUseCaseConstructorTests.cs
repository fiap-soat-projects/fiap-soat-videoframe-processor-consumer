using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Producers.Interfaces;
using Domain.UseCases;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.NotificationUseCaseTests.Constructor;

public class NotificationUseCaseConstructorTests : NotificationUseCaseDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var notificationProducer = Substitute.For<INotificationProducer>();
        var videoEditClient = Substitute.For<IVideoEditClient>();

        // Act
        var result = new NotificationUseCase(notificationProducer, videoEditClient);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NotificationUseCase>();
    }
}
