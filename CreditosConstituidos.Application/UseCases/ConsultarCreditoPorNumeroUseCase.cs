using CreditosConstituidos.Application.Common;
using CreditosConstituidos.Application.Factories;
using CreditosConstituidos.Application.Interfaces;

namespace CreditosConstituidos.Application.UseCases
{
    public class ConsultarCreditoPorNumeroUseCase
    {
        private readonly ICreditoRepository _repo;

        public ConsultarCreditoPorNumeroUseCase(ICreditoRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result> ExecuteAsync(string numeroCredito, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(numeroCredito))
                return Result.BadRequest(new List<string> { "numeroCredito inválido." });

            var credito = await _repo.GetByNumeroCreditoAsync(numeroCredito, ct);

            if (credito == null)
                return new Result(false, 404, new { message = "Crédito não encontrado." });

            var response = CreditoFactory.ToResponse(credito);

            return new Result(true, 200, response);
        }
    }
}

