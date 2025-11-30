using CreditosConstituidos.Application.Common;
using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Interfaces;
using CreditosConstituidos.Application.Validation;
using System.Text.Json;

namespace CreditosConstituidos.Application.UseCases
{
    public class IntegrarCreditosConstituidosUseCase
    {
        private readonly ICreditoRepository _repo;
        private readonly IMessageBusProducer _producer;
        private const string Topic = "integrar-credito-constituido-entry";

        public IntegrarCreditosConstituidosUseCase(
            ICreditoRepository repo,
            IMessageBusProducer producer)
        {
            _repo = repo;
            _producer = producer;
        }

        public async Task<Result> ExecuteAsync(List<CreditoRequestDto> creditos, CancellationToken ct)
        {
            var errors = CreditoRequestValidator.Validate(creditos);
            if (errors.Count > 0)
                return Result.BadRequest(errors);

            foreach (var dto in creditos)
            {
                var exists = await _repo.ExistsByNumeroCreditoAsync(dto.NumeroCredito, ct);
                if (exists)
                    return Result.Conflict($"O crédito {dto.NumeroCredito} já existe na base.");
            }

            foreach (var dto in creditos)
            {
                var json = JsonSerializer.Serialize(dto);
                await _producer.PublishAsync(Topic, json, ct);
            }

            return Result.Accepted();
        }
    }
}
