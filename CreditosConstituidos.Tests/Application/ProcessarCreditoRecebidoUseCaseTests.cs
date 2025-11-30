using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Interfaces;
using CreditosConstituidos.Application.UseCases;
using CreditosConstituidos.Domain.Entities;

namespace CreditosConstituidos.Tests.Application
{
    [TestClass]
    public class ProcessarCreditoRecebidoUseCaseTests
    {
        private ProcessarCreditoRecebidoUseCase _useCase = null!;
        private FakeCreditoRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeCreditoRepository();
            _useCase = new ProcessarCreditoRecebidoUseCase(_repo);
        }

        private static CreditoRequestDto BuildDto(string numeroCredito = "123456")
            => new()
            {
                NumeroCredito = numeroCredito,
                NumeroNfse = "7891011",
                DataConstituicao = new DateTime(2024, 2, 25),
                ValorIssqn = 1500.75m,
                TipoCredito = "ISSQN",
                SimplesNacional = "Sim",
                Aliquota = 5.0m,
                ValorFaturado = 30000m,
                ValorDeducao = 5000m,
                BaseCalculo = 25000m
            };

        [TestMethod]
        public async Task ExecuteAsync_DeveInserirCredito_QuandoNaoExiste()
        {
            var dto = BuildDto("CRED-001");

            await _useCase.ExecuteAsync(dto, CancellationToken.None);

            Assert.AreEqual(1, _repo.Storage.Count, "Deveria ter inserido 1 crédito.");
            var saved = _repo.Storage.Single();
            Assert.AreEqual("CRED-001", saved.NumeroCredito);
            Assert.AreEqual("7891011", saved.NumeroNfse);
            Assert.IsTrue(saved.SimplesNacional);
        }

        [TestMethod]
        public async Task ExecuteAsync_NaoDeveInserirQuandoJaExiste()
        {
            var dto = BuildDto("CRED-002");

            await _useCase.ExecuteAsync(dto, CancellationToken.None);
            Assert.AreEqual(1, _repo.Storage.Count, "Pré-condição falhou: deveria ter 1 crédito já inserido.");

            await _useCase.ExecuteAsync(dto, CancellationToken.None);

            Assert.AreEqual(1, _repo.Storage.Count, "Não deveria inserir um crédito duplicado.");
        }

        private class FakeCreditoRepository : ICreditoRepository
        {
            public List<Credito> Storage { get; } = new();

            public Task AddAsync(Credito credito, CancellationToken ct = default)
            {
                Storage.Add(credito);
                return Task.CompletedTask;
            }

            public Task<bool> ExistsByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            {
                var exists = Storage.Any(c => c.NumeroCredito == numeroCredito);
                return Task.FromResult(exists);
            }

            public Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            {
                var entity = Storage.FirstOrDefault(c => c.NumeroCredito == numeroCredito);
                return Task.FromResult(entity);
            }

            public Task<List<Credito>> GetByNumeroNfseAsync(string numeroNfse, CancellationToken ct = default)
            {
                var list = Storage.Where(c => c.NumeroNfse == numeroNfse).ToList();
                return Task.FromResult(list);
            }
        }
    }
}
