namespace CreditosConstituidos.Infrastructure.Messaging.Kafka
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = null!;
        public string GroupId { get; set; } = "creditos-consumer";
        public bool EnableAutoCommit { get; set; } = true;
    }
}
