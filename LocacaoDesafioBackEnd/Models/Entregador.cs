using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LocacaoDesafioBackEnd.Models
{
    public class Entregador
    {
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; }

        [Required]
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [Required]
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [Required]
        [JsonPropertyName("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [JsonPropertyName("numero_cnh")]
        public required string NumeroCNH { get; set; }

        [Required]
        [JsonPropertyName("tipo_cnh")]
        public required string TipoCNH { get; set; } 

        [Required]
        [JsonPropertyName("imagem_cnh")]
        public required string ImagemCNH { get; set; } // Base64 string
    }
}
