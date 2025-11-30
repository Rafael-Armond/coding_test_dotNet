using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditosConstituidos.Domain.Entities
{
    [Table("credito")]
    public class Credito
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("numero_credito")]
        public string NumeroCredito { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Column("numero_nfse")]
        public string NumeroNfse { get; set; } = null!;

        [Required]
        [Column("data_constituicao", TypeName = "date")]
        public DateTime DataConstituicao { get; set; }

        [Required]
        [Column("valor_issqn", TypeName = "decimal(15,2)")]
        public decimal ValorIssqn { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("tipo_credito")]
        public string TipoCredito { get; set; } = null!;

        [Required]
        [Column("simples_nacional")]
        public bool SimplesNacional { get; set; }

        [Required]
        [Column("aliquota", TypeName = "decimal(5,2)")]
        public decimal Aliquota { get; set; }

        [Required]
        [Column("valor_faturado", TypeName = "decimal(15,2)")]
        public decimal ValorFaturado { get; set; }

        [Required]
        [Column("valor_deducao", TypeName = "decimal(15,2)")]
        public decimal ValorDeducao { get; set; }

        [Required]
        [Column("base_calculo", TypeName = "decimal(15,2)")]
        public decimal BaseCalculo { get; set; }
    }
}
