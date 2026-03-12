using Adapter.Gateways.Producers;
using Domain.Gateways.Producers.Interfaces;
using Infrastructure.Producers.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Producers.NotificationProducerTests;

public abstract class NotificationProducerDependenciesMock
{
    protected readonly IKafkaNotificationProducer _kafkaNotificationProducer;
    protected readonly INotificationProducer _sut;

    protected NotificationProducerDependenciesMock()
    {
        _kafkaNotificationProducer = Substitute.For<IKafkaNotificationProducer>();
        _sut = new NotificationProducer(_kafkaNotificationProducer);
    }
}
