using CreditosConstituidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditosConstituidos.Infrastructure
{
    public class CreditoDbContext : DbContext
    {
        public CreditoDbContext(DbContextOptions<CreditoDbContext> options) : base(options)
        { }

        public DbSet<Credito> Creditos => Set<Credito>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
