using CreditosConstituidos.Domain.Entities;

namespace CreditosConstituidos.Application.Interfaces
{
    public interface ICreditoRepository
    {
        Task AddAsync(Credito credito, CancellationToken ct = default);
        Task<bool> ExistsByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default);
        Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default);
        Task<List<Credito>> GetByNumeroNfseAsync(string numeroNfse, CancellationToken ct = default);
    }
}
