using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Providers;

[ExcludeFromCodeCoverage]
internal static class StaticEnvironmentVariableProvider
{
    private const string KAFKA_HOST_ENV_VARIABLE_NAME = "KAFKA_HOST";
    private const string KAFKA_CONSUMER_GROUP_ENV_VARIABLE_NAME = "KAFKA_CONSUMER_GROUP";
    private const string KAFKA_NOTIFICAITON_TOPIC_ENV_VARIABLE_NAME = "NOTIFICATION_TOPIC";
    private const string S3_BUCKET_BASE_URL = "S3BucketBaseUrl";
    private const string S3_BUCKET_USER = "S3BucketUser";
    private const string S3_BUCKET_PASSWORD = "S3BucketPassword";
    private const string S3_BUCKET_NAME = "S3BucketName";

    internal static readonly string KafkaHost;
    internal static readonly string KafkaConsumerGroup;
    internal static readonly string KafkaProduceTopicName;
    internal static readonly string S3BucketBaseUrl;
    internal static readonly string S3BucketUser;
    internal static readonly string S3BucketPassword;
    internal static readonly string S3BucketName;

    static StaticEnvironmentVariableProvider()
    {
        KafkaHost = GetRequiredEnvironmentVariable(KAFKA_HOST_ENV_VARIABLE_NAME);
        KafkaConsumerGroup = GetRequiredEnvironmentVariable(KAFKA_CONSUMER_GROUP_ENV_VARIABLE_NAME);
        KafkaProduceTopicName = GetRequiredEnvironmentVariable(KAFKA_NOTIFICAITON_TOPIC_ENV_VARIABLE_NAME);
        S3BucketBaseUrl = GetRequiredEnvironmentVariable(S3_BUCKET_BASE_URL);
        S3BucketUser = GetRequiredEnvironmentVariable(S3_BUCKET_USER);
        S3BucketPassword = GetRequiredEnvironmentVariable(S3_BUCKET_PASSWORD);
        S3BucketName = GetRequiredEnvironmentVariable(S3_BUCKET_NAME);
    }

    internal static void Init() { }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Environment variable '{variableName}' is not set.");
        }

        return value;
    }
}
