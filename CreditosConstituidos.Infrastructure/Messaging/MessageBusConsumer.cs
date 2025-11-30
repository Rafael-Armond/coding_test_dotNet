using CreditosConstituidos.Application.Interfaces;

namespace CreditosConstituidos.Infrastructure.Messaging
{
    internal class MessageBusConsumer : IMessageBusConsumer
    {
        public Task<string?> TryConsumeAsync(string topic, CancellationToken ct = default)
            => Task.FromResult<string?>(null);
    }
}
