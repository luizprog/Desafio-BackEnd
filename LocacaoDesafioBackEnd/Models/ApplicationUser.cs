using Microsoft.AspNetCore.Identity;

namespace LocacaoDesafioBackEnd.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsAdmin { get; set; }
    }
}
