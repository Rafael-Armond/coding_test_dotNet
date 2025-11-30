using Confluent.Kafka;
using CreditosConstituidos.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace CreditosConstituidos.Infrastructure.Messaging.Kafka
{
    public class KafkaMessageBusProducer : IMessageBusProducer
    {
        private readonly KafkaSettings _settings;

        public KafkaMessageBusProducer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
        }

        public async Task PublishAsync(string topic, string message, CancellationToken ct = default)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                Acks = Acks.All,
                MessageTimeoutMs = 5000,
                RetryBackoffMs = 100,
                EnableIdempotence = true,
                MaxInFlight = 5
            };

            try
            {
                using var producer = new ProducerBuilder<Null, string>(config).Build();

                var dr = await producer.ProduceAsync(
                    topic,
                    new Message<Null, string> { Value = message },
                    ct
                );

                producer.Flush(TimeSpan.FromSeconds(5));
            }
            catch (OperationCanceledException)
            {
                throw; 
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"[Kafka Error] Falha ao publicar no tópico '{topic}': {ex.Error.Reason}");

                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[Kafka Unexpected Error] Tópico '{topic}' | Mensagem truncada: '{Truncate(message, 120)}' | Erro: {ex.Message}"
                );

                throw;
            }
        }
        private static string Truncate(string text, int maxLen)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLen ? text : text.Substring(0, maxLen) + "...";
        }

    }
}
