using Microsoft.AspNetCore.Mvc;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LocacaoDesafioBackEnd.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class EntregadorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public EntregadorController(ApplicationDbContext context, UserService userService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Cadastrar novo entregador
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CadastrarEntregador([FromBody] Entregador entregador)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Entregadores.AnyAsync(e => e.Cnpj == entregador.Cnpj))
                return BadRequest(new { message = "CNPJ já cadastrado." });

            if (await _context.Entregadores.AnyAsync(e => e.NumeroCNH == entregador.NumeroCNH))
                return BadRequest(new { message = "Número de CNH já cadastrado." });

            // Adiciona o entregador ao banco de dados
            await _context.Entregadores.AddAsync(entregador);
            await _context.SaveChangesAsync();

            // Cria o usuário associado ao entregador
            var userName = entregador.Identificador.ToLower();
            var userEmail = $"{userName}@locacao.desafio";

            // Corrigido para não atribuir a uma variável
            await _userService.CreateUserAsync(userName, userEmail, isAdmin: false);

            // if (!userCreationResult.Succeeded)
            // {
            //     // Se a criação do usuário falhar, exclua o entregador para manter a consistência
            //     _context.Entregadores.Remove(entregador);
            //     await _context.SaveChangesAsync();

            //     return BadRequest(new { message = "Falha ao criar usuário para o entregador.", errors = userCreationResult.Errors });
            // }

            return CreatedAtAction(nameof(CadastrarEntregador), new { id = entregador.Id }, entregador);
        }


        /// <summary>
        /// Enviar foto da CNH do entregador
        /// </summary>
        [HttpPost("{idEntregador}/cnh")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EnviarCnh(int idEntregador, [FromForm] EnviarCnhRequest request)
        {
            if (request.ImagemCnh == null)
                return BadRequest("Imagem da CNH é obrigatória");

            var entregador = await _context.Entregadores.FindAsync(idEntregador);
            if (entregador == null)
                return NotFound("Entregador não encontrado");

            // Verifica se o usuário é o próprio entregador ou possui a role Admin
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && entregador.Identificador != userId)
                return Forbid("Você não tem permissão para atualizar a CNH de outro entregador.");

            // Verifica se o arquivo é PNG ou BMP
            var extension = Path.GetExtension(request.ImagemCnh.FileName).ToLower();
            if (extension != ".png" && extension != ".bmp")
                return BadRequest("A imagem deve ser nos formatos PNG ou BMP.");

            // Gera o caminho do arquivo
            var fileName = $"cnh_{idEntregador}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(AppContext.BaseDirectory, "Storage", fileName);

            // Cria o diretório, se não existir
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Salva o arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImagemCnh.CopyToAsync(stream);
            }

            // Atualiza o campo de imagem no entregador (opcional)
            entregador.ImagemCNH = filePath;
            _context.Entregadores.Update(entregador);
            await _context.SaveChangesAsync();

            return Ok("CNH enviada com sucesso");
        }

        /// <summary>
        /// Buscar todos entregadores
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entregador>>> GetEntregadores()
        {
            var entregadores = await _context.Entregadores.ToListAsync();
            return Ok(entregadores);
        }
    }
}
