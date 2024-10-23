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
        public DateTime DataLocacao { get; set; }

        [Required]
        public DateTime DataDevolucao { get; set; }

        public virtual Moto Moto { get; set; }
        public virtual Entregador Entregador { get; set; }
    }
}
