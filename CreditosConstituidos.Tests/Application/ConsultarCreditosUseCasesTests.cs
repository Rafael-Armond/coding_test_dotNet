using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Interfaces;
using CreditosConstituidos.Application.UseCases;
using CreditosConstituidos.Domain.Entities;

namespace CreditosConstituidos.Tests.Application;

[TestClass]
public class ConsultarCreditosUseCasesTests
{
    private FakeCreditoRepository _repo = null!;
    private ConsultarCreditosPorNfseUseCase _porNfse = null!;
    private ConsultarCreditoPorNumeroUseCase _porNumero = null!;

    [TestInitialize]
    public void Setup()
    {
        _repo = new FakeCreditoRepository();
        _porNfse = new ConsultarCreditosPorNfseUseCase(_repo);
        _porNumero = new ConsultarCreditoPorNumeroUseCase(_repo);
    }

    private static Credito BuildCredito(string numCredito, string numNfse)
        => new()
        {
            NumeroCredito = numCredito,
            NumeroNfse = numNfse,
            DataConstituicao = new DateTime(2024, 02, 25),
            ValorIssqn = 1500.75m,
            TipoCredito = "ISSQN",
            SimplesNacional = true,
            Aliquota = 5.0m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

    [TestMethod]
    public async Task PorNfse_DeveRetornarLista()
    {
        _repo.Storage.Add(BuildCredito("1", "NF1"));
        _repo.Storage.Add(BuildCredito("2", "NF1"));
        _repo.Storage.Add(BuildCredito("3", "NF2"));

        var result = await _porNfse.ExecuteAsync("NF1", CancellationToken.None);

        Assert.IsTrue(result.Success);
        var list = (List<CreditoResponseDto>)result.Payload!;
        Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public async Task PorNfse_InputInvalido_DeveRetornar400()
    {
        var result = await _porNfse.ExecuteAsync("", CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.AreEqual(400, result.StatusCode);
    }

    [TestMethod]
    public async Task PorNumero_DeveRetornarCredito()
    {
        _repo.Storage.Add(BuildCredito("CRED123", "NF1"));

        var result = await _porNumero.ExecuteAsync("CRED123", CancellationToken.None);

        Assert.IsTrue(result.Success);
        var dto = (CreditoResponseDto)result.Payload!;
        Assert.AreEqual("CRED123", dto.NumeroCredito);
    }

    [TestMethod]
    public async Task PorNumero_NaoEncontrado_DeveRetornar404()
    {
        var result = await _porNumero.ExecuteAsync("DONT_EXIST", CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.AreEqual(404, result.StatusCode);
    }

    [TestMethod]
    public async Task PorNumero_InputInvalido_DeveRetornar400()
    {
        var result = await _porNumero.ExecuteAsync("", CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.AreEqual(400, result.StatusCode);
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
            => Task.FromResult(Storage.Any(c => c.NumeroCredito == numeroCredito));

        public Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            => Task.FromResult(Storage.FirstOrDefault(c => c.NumeroCredito == numeroCredito));

        public Task<List<Credito>> GetByNumeroNfseAsync(string numeroNfse, CancellationToken ct = default)
            => Task.FromResult(Storage.Where(c => c.NumeroNfse == numeroNfse).ToList());
    }
}
