using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDesafioBackEnd.Models
{
    [Table("motos")]
    public class Moto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        [Required]
        [StringLength(50)]
        public string Marca { get; set; }

        [Required]
        public bool Disponivel { get; set; }

        [Required]
        [StringLength(20)] // Ajuste o tamanho conforme necessário
        public string Placa { get; set; } // Adicione este campo

        [Required]
        public int Ano { get; set; }

        [Required]
        [StringLength(100)]
        public string Cor { get; set; }
    }
}
