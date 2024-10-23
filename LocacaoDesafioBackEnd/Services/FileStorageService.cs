// File: Services/FileStorageService.cs
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace LocacaoDesafioBackEnd.Services
{
    public class FileStorageService
    {
        private readonly string _uploadPath;

        public FileStorageService(IConfiguration configuration)
        {
            _uploadPath = configuration["FileStorage:UploadPath"];
            Directory.CreateDirectory(_uploadPath); // Cria o diretório se não existir
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(_uploadPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath; // Retorna o caminho do arquivo salvo
        }
    }
}
