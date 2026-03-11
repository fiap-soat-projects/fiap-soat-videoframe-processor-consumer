using Adapter.Controllers;
using Adapter.Controllers.Interfaces;
using Adapter.Gateways.Clients;
using Adapter.Gateways.Extractors;
using Adapter.Gateways.Producers;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Extractors.Interfaces;
using Domain.Gateways.Producers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Adapter;

[ExcludeFromCodeCoverage]
public static class AdapterExtensions
{
    public static IServiceCollection InjectAdapterDependencies(this IServiceCollection services)
    {
        return services
            .RegisterControllers()
            .RegisterGateways();
    }

    private static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        return services.AddSingleton<IVideoProcessingController, VideoProcessingController>();
    }

    public static IServiceCollection RegisterGateways(this IServiceCollection services)
    {
        services
            .AddSingleton<IStorageClient, StorageClient>()
            .AddSingleton<IVideoFrameExtractor, VideoFrameExtractor>()
            .AddSingleton<INotificationProducer, NotificationProducer>();

        return services;
    }
}
