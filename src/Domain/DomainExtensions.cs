using Domain.UseCases;
using Domain.UseCases.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Domain;

[ExcludeFromCodeCoverage]
public static class DomainExtensions
{
    public static IServiceCollection InjectDomainDependencies(this IServiceCollection services)
    {
        return services.RegisterUseCases();
    }

    private static IServiceCollection RegisterUseCases(this IServiceCollection services)
    {
        return services       
         .AddSingleton<IEditUseCase, FrameEditUseCase>()
         .AddSingleton<IEditUseCaseResolver, EditUseCaseResolver>()
         .AddSingleton<INotificationUseCase, NotificationUseCase>()
         .AddSingleton<IStorageUseCase, StorageUseCase>();
    }
}
