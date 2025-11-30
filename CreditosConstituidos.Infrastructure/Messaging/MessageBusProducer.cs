using CreditosConstituidos.Application.Interfaces;

namespace CreditosConstituidos.Infrastructure.Messaging
{
    public class MessageBusProducer : IMessageBusProducer
    {
        public Task PublishAsync(string topic, string message, CancellationToken ct = default)
            => Task.CompletedTask;
    }
}
