using Adapter.Gateways.Producers;
using FluentAssertions;
using Infrastructure.Producers.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Producers.NotificationProducerTests.Constructor;

public class NotificationProducerConstructorTests : NotificationProducerDependenciesMock
{
    [Fact]
    public void When_Valid_Dependencies_Then_Create_Instance_Successfully()
    {
        // Arrange
        var kafkaNotificationProducer = Substitute.For<IKafkaNotificationProducer>();

        // Act
        var result = new NotificationProducer(kafkaNotificationProducer);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NotificationProducer>();
    }
}
