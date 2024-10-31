using System.Security.Cryptography;

namespace LocacaoDesafioBackEnd.Tests // Use o namespace apropriado
{
    public static class KeyGenerator
    {
        public static string GenerateRandomKey()
        {
            var key = new byte[32]; // 32 bytes para uma chave de 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key); // Retorna a chave em Base64
        }
    }
}