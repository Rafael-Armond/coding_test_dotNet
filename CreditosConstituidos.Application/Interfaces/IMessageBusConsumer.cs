namespace CreditosConstituidos.Application.Interfaces
{
    public interface IMessageBusConsumer
    {
        Task<string?> TryConsumeAsync(string topic, CancellationToken ct = default);
    }
}
