using CreditosConstituidos.Application.Common;
using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Factories;
using CreditosConstituidos.Application.Interfaces;

namespace CreditosConstituidos.Application.UseCases
{
    public class ConsultarCreditosPorNfseUseCase
    {
        private readonly ICreditoRepository _repo;

        public ConsultarCreditosPorNfseUseCase(ICreditoRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result> ExecuteAsync(string numeroNfse, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(numeroNfse))
                return Result.BadRequest(new List<string> { "numeroNfse inválido." });

            var creditos = await _repo.GetByNumeroNfseAsync(numeroNfse, ct);
            var response = creditos.Select(c  => CreditoFactory.ToResponse(c)).ToList();

            return new Result(true, 200, response);
        }
    }
}
