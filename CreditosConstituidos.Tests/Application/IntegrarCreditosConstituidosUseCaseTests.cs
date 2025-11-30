using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.Interfaces;
using CreditosConstituidos.Application.UseCases;
using CreditosConstituidos.Domain.Entities;

namespace CreditosConstituidos.Tests;

[TestClass]
public class IntegrarCreditosConstituidosUseCaseTests
{
    private IntegrarCreditosConstituidosUseCase _useCase = null!;
    private FakeCreditoRepository _repo = null!;
    private FakeMessageBusProducer _producer = null!;

    [TestInitialize]
    public void Setup()
    {
        _repo = new FakeCreditoRepository();
        _producer = new FakeMessageBusProducer();
        _useCase = new IntegrarCreditosConstituidosUseCase(_repo, _producer);
    }

    private static CreditoRequestDto BuildDto(string numeroCredito = "123")
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
    public async Task ExecuteAsync_ShouldReturn202_AndPublishMessages_WhenValid()
    {
        var list = new List<CreditoRequestDto>
        {
            BuildDto("1"),
            BuildDto("2"),
            BuildDto("3")
        };

        var result = await _useCase.ExecuteAsync(list, CancellationToken.None);

        Assert.IsTrue(result.Success);
        Assert.AreEqual(202, result.StatusCode);
        Assert.AreEqual(3, _producer.Published.Count);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldReturn400_WhenListIsEmpty()
    {
        var list = new List<CreditoRequestDto>();

        var result = await _useCase.ExecuteAsync(list, CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.AreEqual(400, result.StatusCode);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldReturn400_WhenDtoInvalid()
    {
        var list = new List<CreditoRequestDto>
        {
            new CreditoRequestDto() 
        };

        var result = await _useCase.ExecuteAsync(list, CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.AreEqual(400, result.StatusCode);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldReturn409_WhenCreditoAlreadyExists()
    {
        _repo.ExistingCreditos.Add("999");

        var list = new List<CreditoRequestDto>
        {
            BuildDto("999")
        };

        var result = await _useCase.ExecuteAsync(list, CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.AreEqual(409, result.StatusCode);
        Assert.AreEqual(0, _producer.Published.Count);
    }

    private class FakeCreditoRepository : ICreditoRepository
    {
        public HashSet<string> ExistingCreditos { get; } = new();

        public Task AddAsync(Credito credito, CancellationToken ct = default)
            => Task.CompletedTask;

        public Task<bool> ExistsByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            => Task.FromResult(ExistingCreditos.Contains(numeroCredito));

        public Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken ct = default)
            => Task.FromResult<Credito?>(null);

        public Task<List<CreditosConstituidos.Domain.Entities.Credito>> GetByNumeroNfseAsync(string numeroNfse, CancellationToken ct = default)
            => Task.FromResult(new List<CreditosConstituidos.Domain.Entities.Credito>());
    }

    private class FakeMessageBusProducer : IMessageBusProducer
    {
        public List<(string topic, string message)> Published { get; } = new();

        public Task PublishAsync(string topic, string message, CancellationToken ct = default)
        {
            Published.Add((topic, message));
            return Task.CompletedTask;
        }
    }
}
