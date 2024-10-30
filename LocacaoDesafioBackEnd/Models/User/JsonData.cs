using System.Collections.Generic;

namespace LocacaoDesafioBackEnd.Models.User
{
    public class JsonData
    {
        public List<string> Roles { get; set; }
        public List<User> Users { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
