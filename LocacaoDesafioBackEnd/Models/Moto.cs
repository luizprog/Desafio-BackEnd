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

        [Required] // Obrigatório
        [StringLength(100)]
        public string Modelo { get; set; }

        [StringLength(50)] // Não obrigatório
        public string Marca { get; set; }

        public bool Disponivel { get; set; } // Não obrigatório

        [Required] // Obrigatório
        [StringLength(20)] // Ajuste o tamanho conforme necessário
        public string Placa { get; set; } // Adicione este campo

        [Required] // Obrigatório
        public int Ano { get; set; }

        [StringLength(100)] // Não obrigatório
        public string Cor { get; set; }
    }
}
