using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Producers.Interfaces;
using Domain.UseCases;
using Domain.UseCases.Interfaces;
using NSubstitute;

namespace UnitTests.Domain.UseCases.NotificationUseCaseTests;

public abstract class NotificationUseCaseDependenciesMock
{
    protected readonly INotificationProducer _notificationProducer;
    protected readonly IVideoEditClient _videoEditClient;
    protected readonly INotificationUseCase _sut;

    protected NotificationUseCaseDependenciesMock()
    {
        _notificationProducer = Substitute.For<INotificationProducer>();
        _videoEditClient = Substitute.For<IVideoEditClient>();

        _sut = new NotificationUseCase(_notificationProducer, _videoEditClient);
    }
}
