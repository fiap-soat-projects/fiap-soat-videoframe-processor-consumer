using Amazon.S3;
using Confluent.Kafka;
using Infrastructure.Clients;
using Infrastructure.Clients.Interfaces;
using Infrastructure.Producers;
using Infrastructure.Producers.Interfaces;
using Infrastructure.Providers;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
    public static IServiceCollection InjectInfrastructureDependencies(this IServiceCollection services)
    {
        StaticEnvironmentVariableProvider.Init();

        services
            .AddSingleton<IKafkaService, KafkaService>()
            .RegisterClients()
            .RegisterProducers();

        return services;
    }

    public static IServiceCollection RegisterClients(this IServiceCollection services)
    {
        var s3url = StaticEnvironmentVariableProvider.S3BucketBaseUrl;
        var s3user = StaticEnvironmentVariableProvider.S3BucketUser;
        var s3password = StaticEnvironmentVariableProvider.S3BucketPassword;
        var s3BucketName = StaticEnvironmentVariableProvider.S3BucketName;
        var videoFrameApiUrl = StaticEnvironmentVariableProvider.VideoFrameApiUrl;
        var videoFrameApiUri = new Uri(videoFrameApiUrl, UriKind.Absolute);

        var config = new AmazonS3Config
        {
            ServiceURL = s3url,
            ForcePathStyle = true
        };

        var amazonS3Client = new AmazonS3Client(s3user, s3password, config);
        var s3BucketClient = new S3BucketClient(amazonS3Client, s3BucketName);

        services.AddSingleton<IS3BucketClient>(s3BucketClient);
        services.AddHttpClient<IVideoFrameClient, VideoFrameClient>(x =>
        {
            x.BaseAddress = videoFrameApiUri;
        });

        return services;
    }

    public static IServiceCollection RegisterProducers(this IServiceCollection services)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = StaticEnvironmentVariableProvider.KafkaHost
        };

        var producer = new ProducerBuilder<Null, string>(producerConfig).Build();


        services.AddSingleton(producer);
        services.AddSingleton<IKafkaNotificationProducer, KafkaNotificationProducer>();

        return services;
    }
}
