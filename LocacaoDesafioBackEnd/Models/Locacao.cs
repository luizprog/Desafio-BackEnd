using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDesafioBackEnd.Models
{
    [Table("locacoes")]
    public class Locacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Moto")]
        public int MotoId { get; set; }

        [ForeignKey("Entregador")]
        public int EntregadorId { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime DataLocacao { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? DataDevolucao { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime DataPrevisaoTermino { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDiaria { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        public virtual Moto Moto { get; set; }
        public virtual Entregador Entregador { get; set; }
    }
}
