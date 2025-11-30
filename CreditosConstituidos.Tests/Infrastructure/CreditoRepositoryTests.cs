using CreditosConstituidos.Domain.Entities;
using CreditosConstituidos.Infrastructure;
using CreditosConstituidos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditosConstituidos.Tests.Infrastructure
{
    [TestClass]
    public class CreditoRepositoryTests
    {
        private CreditoDbContext _context = null!;
        private CreditoRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CreditoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CreditoDbContext(options);
            _repository = new CreditoRepository(_context);
        }

        private static Credito BuildCredito(
            string numeroCredito = "123456",
            string numeroNfse = "7891011")
        {
            return new Credito
            {
                NumeroCredito = numeroCredito,
                NumeroNfse = numeroNfse,
                DataConstituicao = new DateTime(2024, 02, 25),
                ValorIssqn = 1500.75m,
                TipoCredito = "ISSQN",
                SimplesNacional = true,
                Aliquota = 5.0m,
                ValorFaturado = 30000.00m,
                ValorDeducao = 5000.00m,
                BaseCalculo = 25000.00m
            };
        }

        [TestMethod]
        public async Task AddAsync_ShouldPersistCredito()
        {
            var credito = BuildCredito();

            await _repository.AddAsync(credito);

            var saved = await _context.Creditos.FirstOrDefaultAsync(c => c.NumeroCredito == "123456");
            Assert.IsNotNull(saved);
            Assert.AreEqual("123456", saved!.NumeroCredito);
        }

        [TestMethod]
        public async Task ExistsByNumeroCreditoAsync_ShouldReturnTrue_WhenCreditoExists()
        {
            var credito = BuildCredito("111", "NF1");
            await _repository.AddAsync(credito);

            var exists = await _repository.ExistsByNumeroCreditoAsync("111");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task ExistsByNumeroCreditoAsync_ShouldReturnFalse_WhenCreditoDoesNotExist()
        {
            var exists = await _repository.ExistsByNumeroCreditoAsync("999");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task GetByNumeroCreditoAsync_ShouldReturnCredito_WhenFound()
        {
            var credito = BuildCredito("222", "NF2");
            await _repository.AddAsync(credito);

            var result = await _repository.GetByNumeroCreditoAsync("222");

            Assert.IsNotNull(result);
            Assert.AreEqual("NF2", result!.NumeroNfse);
        }

        [TestMethod]
        public async Task GetByNumeroCreditoAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _repository.GetByNumeroCreditoAsync("404");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByNumeroNfseAsync_ShouldReturnList_WhenFound()
        {
            await _repository.AddAsync(BuildCredito("1", "NFSE-X"));
            await _repository.AddAsync(BuildCredito("2", "NFSE-X"));
            await _repository.AddAsync(BuildCredito("3", "NFSE-Y"));

            var result = await _repository.GetByNumeroNfseAsync("NFSE-X");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(c => c.NumeroNfse == "NFSE-X"));
        }

        [TestMethod]
        public async Task GetByNumeroNfseAsync_ShouldReturnEmptyList_WhenNotFound()
        {
            await _repository.AddAsync(BuildCredito("1", "NFSE-A"));

            var result = await _repository.GetByNumeroNfseAsync("NFSE-NOT-EXISTS");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
