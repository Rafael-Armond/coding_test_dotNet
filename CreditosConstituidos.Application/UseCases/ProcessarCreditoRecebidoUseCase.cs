using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Factories;
using CreditosConstituidos.Application.Interfaces;

namespace CreditosConstituidos.Application.UseCases
{
    public class ProcessarCreditoRecebidoUseCase
    {
        private readonly ICreditoRepository _repo;

        public ProcessarCreditoRecebidoUseCase(ICreditoRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(CreditoRequestDto dto, CancellationToken ct)
        {
            var exists = await _repo.ExistsByNumeroCreditoAsync(dto.NumeroCredito, ct);
            if (exists) return;

            var entity = CreditoFactory.FromDto(dto);

            await _repo.AddAsync(entity, ct);
        }
    }
}
