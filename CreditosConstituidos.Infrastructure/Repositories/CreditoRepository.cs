using CreditosConstituidos.Domain.Entities;
using CreditosConstituidos.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreditosConstituidos.Infrastructure.Repositories
{
    public class CreditoRepository : ICreditoRepository
    {
        private readonly CreditoDbContext _context;
        public CreditoRepository(CreditoDbContext context) => _context = context;

        public async Task AddAsync(Credito credito, CancellationToken ct = default)
        {
            await _context.Creditos.AddAsync(credito, ct);
            await _context.SaveChangesAsync(ct); 
        }

        public Task<bool> ExistsByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            => _context.Creditos.AnyAsync(c => c.NumeroCredito == numeroCredito, ct);

        public Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            => _context.Creditos.AsNoTracking()
                .FirstOrDefaultAsync(c => c.NumeroCredito == numeroCredito, ct);

        public Task<List<Credito>> GetByNumeroNfseAsync(string numeroNfse, CancellationToken ct = default)
            => _context.Creditos.AsNoTracking()
                .Where(c => c.NumeroNfse == numeroNfse)
                .ToListAsync(ct);
    }
}
