namespace CreditosConstituidos.Application.Interfaces
{
    public interface IMessageBusProducer
    {
        Task PublishAsync(string topic, string message, CancellationToken ct = default);
    }
}
