using CreditosConstituidos.Application.DTOs;
using CreditosConstituidos.Domain.Entities;

namespace CreditosConstituidos.Application.Factories
{
    public static class CreditoFactory
    {
        public static Credito FromDto(CreditoRequestDto dto) => new()
        {
            NumeroCredito = dto.NumeroCredito,
            NumeroNfse = dto.NumeroNfse,
            DataConstituicao = dto.DataConstituicao,
            ValorIssqn = dto.ValorIssqn,
            TipoCredito = dto.TipoCredito,
            SimplesNacional = dto.SimplesNacional.Equals("Sim", StringComparison.OrdinalIgnoreCase),
            Aliquota = dto.Aliquota,
            ValorFaturado = dto.ValorFaturado,
            ValorDeducao = dto.ValorDeducao,
            BaseCalculo = dto.BaseCalculo
        };

        public static CreditoResponseDto ToResponse(Credito entity) => new()
        {
            NumeroCredito = entity.NumeroCredito,
            NumeroNfse = entity.NumeroNfse,
            DataConstituicao = entity.DataConstituicao,
            ValorIssqn = entity.ValorIssqn,
            TipoCredito = entity.TipoCredito,
            SimplesNacional = entity.SimplesNacional ? "Sim" : "Não",
            Aliquota = entity.Aliquota,
            ValorFaturado = entity.ValorFaturado,
            ValorDeducao = entity.ValorDeducao,
            BaseCalculo = entity.BaseCalculo
        };
    }
}
