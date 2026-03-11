using Infrastructure.Providers;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        StaticEnvironmentVariableProvider.Init();

        services.AddSingleton<IKafkaService, KafkaService>();

        return services;
    }
}
