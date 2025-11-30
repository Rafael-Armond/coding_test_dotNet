using Confluent.Kafka;
using CreditosConstituidos.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace CreditosConstituidos.Infrastructure.Messaging.Kafka
{
    public class KafkaMessageBusConsumer : IMessageBusConsumer
    {
        private readonly KafkaSettings _settings;
        private IConsumer<Ignore, string>? _consumer;

        public KafkaMessageBusConsumer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
        }

        private IConsumer<Ignore, string> GetConsumer(string topic)
        {
            if (_consumer != null) return _consumer;

            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = _settings.EnableAutoCommit
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(topic);

            return _consumer;
        }

        public Task<string?> TryConsumeAsync(string topic, CancellationToken ct = default)
        {
            var consumer = GetConsumer(topic);
            var result = consumer.Consume(TimeSpan.FromMilliseconds(50));

            return Task.FromResult(result?.Message?.Value);
        }
    }
}
