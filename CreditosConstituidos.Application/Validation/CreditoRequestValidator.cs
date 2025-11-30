using CreditosConstituidos.Application.DTOs;

namespace CreditosConstituidos.Application.Validation
{
    public static class CreditoRequestValidator
    {
        public static List<string> Validate(List<CreditoRequestDto> creditos)
        {
            var errors = new List<string>();

            if (creditos == null || creditos.Count == 0)
            {
                errors.Add("A lista de créditos está vazia.");
                return errors;
            }

            for (int i = 0; i < creditos.Count; i++)
            {
                var c = creditos[i];

                if (string.IsNullOrWhiteSpace(c.NumeroCredito))
                    errors.Add($"Item {i}: numeroCredito é obrigatório.");

                if (string.IsNullOrWhiteSpace(c.NumeroNfse))
                    errors.Add($"Item {i}: numeroNfse é obrigatório.");

                if (c.DataConstituicao == default)
                    errors.Add($"Item {i}: dataConstituicao é obrigatória.");

                if (c.ValorIssqn <= 0)
                    errors.Add($"Item {i}: valorIssqn deve ser maior que zero.");

                if (string.IsNullOrWhiteSpace(c.TipoCredito))
                    errors.Add($"Item {i}: tipoCredito é obrigatório.");

                if (c.SimplesNacional != "Sim" && c.SimplesNacional != "Não")
                    errors.Add($"Item {i}: simplesNacional deve ser 'Sim' ou 'Não'.");

                if (c.Aliquota < 0)
                    errors.Add($"Item {i}: aliquota inválida.");

                if (c.ValorFaturado < 0)
                    errors.Add($"Item {i}: valorFaturado inválido.");

                if (c.ValorDeducao < 0)
                    errors.Add($"Item {i}: valorDeducao inválido.");

                if (c.BaseCalculo < 0)
                    errors.Add($"Item {i}: baseCalculo inválido.");
            }

            return errors;
        }
    }
}
