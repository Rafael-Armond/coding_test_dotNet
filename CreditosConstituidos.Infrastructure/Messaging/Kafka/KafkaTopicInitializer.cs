using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;

namespace CreditosConstituidos.Infrastructure.Messaging.Kafka;

public class KafkaTopicInitializer
{
    private readonly KafkaSettings _settings;

    public KafkaTopicInitializer(IOptions<KafkaSettings> options)
    {
        _settings = options.Value;
    }

    public async Task EnsureTopicExistsAsync(string topicName, int partitions = 1, short replicationFactor = 1)
    {
        using var admin = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = _settings.BootstrapServers
        }).Build();

        try
        {
            await admin.CreateTopicsAsync(new[]
            {
                new TopicSpecification
                {
                    Name = topicName,
                    NumPartitions = partitions,
                    ReplicationFactor = replicationFactor
                }
            });
        }
        catch (CreateTopicsException e)
        {
            if (e.Results.Any(r => r.Error.Code != ErrorCode.TopicAlreadyExists))
                throw;
        }
    }
}
