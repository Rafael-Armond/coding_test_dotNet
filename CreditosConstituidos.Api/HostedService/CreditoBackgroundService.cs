using Confluent.Kafka;
using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Interfaces;
using CreditosConstituidos.Application.UseCases;
using System.Text.Json;

namespace CreditosConstituidos.Api.HostedService
{
    public class CreditoBackgroundService : BackgroundService
    {
        private readonly ILogger<CreditoBackgroundService> _logger;
        private readonly IMessageBusConsumer _consumer;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string TopicName = "integrar-credito-constituido-entry";

        public CreditoBackgroundService(
            ILogger<CreditoBackgroundService> logger,
            IMessageBusConsumer consumer,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _consumer = consumer;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CreditoBackgroundService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var msg = await _consumer.TryConsumeAsync(TopicName, stoppingToken);

                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        var dto = JsonSerializer.Deserialize<CreditoRequestDto>(msg);

                        if (dto != null)
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var useCase = scope.ServiceProvider.GetRequiredService<ProcessarCreditoRecebidoUseCase>();
                            await useCase.ExecuteAsync(dto, stoppingToken);
                        }
                    }
                }
                catch (ConsumeException ex) when (
                    ex.Error.Code == ErrorCode.UnknownTopicOrPart ||
                    ex.Error.IsFatal == false)
                {
                    _logger.LogWarning("Kafka ainda não está pronto ou o tópico não existe.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado no background servcie.");
                }

                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
