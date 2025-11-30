using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CreditosConstituidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditosController : ControllerBase
    {
        private readonly IntegrarCreditosConstituidosUseCase _integrarCreditosConstituidosUseCase;
        private readonly ConsultarCreditosPorNfseUseCase _consultarPorNfseUseCase;
        private readonly ConsultarCreditoPorNumeroUseCase _consultarPorNumeroUseCase;

        public CreditosController(
            IntegrarCreditosConstituidosUseCase integrarCreditosConstituidosUseCase,
            ConsultarCreditosPorNfseUseCase consultarPorNfseUseCase,
            ConsultarCreditoPorNumeroUseCase consultarPorNumeroUseCase)
        {
            _integrarCreditosConstituidosUseCase = integrarCreditosConstituidosUseCase;
            _consultarPorNfseUseCase = consultarPorNfseUseCase;
            _consultarPorNumeroUseCase = consultarPorNumeroUseCase;
        }

        [HttpPost("integrar-credito-constituido")]
        public async Task<IActionResult> IntegrarCreditoConstituido(
            [FromBody] List<CreditoRequestDto> creditos,
            CancellationToken ct)
        {
            var result = await _integrarCreditosConstituidosUseCase.ExecuteAsync(creditos, ct);
            return StatusCode(result.StatusCode, result.Payload);
        }

        [HttpGet("{numeroNfse}")]
        public async Task<IActionResult> GetByNumeroNfse(
           string numeroNfse,
           CancellationToken ct)
        {
            var result = await _consultarPorNfseUseCase.ExecuteAsync(numeroNfse, ct);
            return StatusCode(result.StatusCode, result.Payload);
        }

        [HttpGet("credito/{numeroCredito}")]
        public async Task<IActionResult> GetByNumeroCredito(
            string numeroCredito,
            CancellationToken ct)
        {
            var result = await _consultarPorNumeroUseCase.ExecuteAsync(numeroCredito, ct);
            return StatusCode(result.StatusCode, result.Payload);
        }
    }
}
