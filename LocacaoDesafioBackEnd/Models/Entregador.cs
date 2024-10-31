using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocacaoDesafioBackEnd.Models
{
    public class Entregador
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Makes it auto-generated
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; }

        [Required]
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [Required]
        [JsonPropertyName("cnpj")]
        [RegularExpression(@"\d{14}", ErrorMessage = "O CNPJ deve conter exatamente 14 dígitos.")]
        public string Cnpj { get; set; } // Aplicar a constraint de unicidade no banco de dados

        [Required]
        [JsonPropertyName("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [JsonPropertyName("numero_cnh")]
        public string NumeroCNH { get; set; } // Aplicar a constraint de unicidade no banco de dados

        [Required]
        [JsonPropertyName("tipo_cnh")]
        [RegularExpression("^(A|B|A\\+B|AB)$", ErrorMessage = "Tipo de CNH deve ser A, B, A+B ou AB.")]
        public string TipoCNH { get; set; }

        [Required]
        [JsonPropertyName("imagem_cnh")]
        public string ImagemCNH { get; set; } // Base64 string
    }
}
