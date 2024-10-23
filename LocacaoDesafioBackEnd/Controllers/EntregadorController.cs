using Microsoft.AspNetCore.Mvc;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace LocacaoDesafioBackEnd.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class EntregadorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EntregadorController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Cadastrar novo entregador
        /// </summary>
        /// <param name="entregador">Dados do novo entregador</param>
        /// <returns>O entregador cadastrado</returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CadastrarEntregador([FromBody] Entregador entregador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Entregadores.AddAsync(entregador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CadastrarEntregador), new { id = entregador.Id }, entregador);
        }

        /// <summary>
        /// Enviar foto da CNH do entregador
        /// </summary>
        /// <param name="idEntregador">ID do entregador</param>
        /// <param name="cnhRequest">Arquivo da foto da CNH</param>
        /// <returns>Status do envio</returns>
        [HttpPost("{idEntregador}/cnh")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> EnviarCnh(int idEntregador, [FromBody] CnhRequest cnhRequest)
        {
            if (string.IsNullOrEmpty(cnhRequest.ImagemCnh))
            {
                return BadRequest("Imagem da CNH é obrigatória");
            }

            // Verifica se a imagem é do tipo correto
            if (!cnhRequest.ImagemCnh.EndsWith(".png") && !cnhRequest.ImagemCnh.EndsWith(".bmp"))
            {
                return BadRequest("A imagem deve ser nos formatos PNG ou BMP.");
            }

            byte[] imagemBytes = Convert.FromBase64String(cnhRequest.ImagemCnh);

            // Caminho para salvar a imagem na raiz do projeto
            var fileName = $"cnh_{idEntregador}_{Guid.NewGuid()}.png"; // Nome único para a imagem
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName); // Salva na raiz do projeto

            // Salvar a imagem no disco
            await System.IO.File.WriteAllBytesAsync(filePath, imagemBytes);

            // Atualiza o registro do entregador, se necessário
            var entregador = await _context.Entregadores.FindAsync(idEntregador);
            if (entregador == null)
            {
                return NotFound("Entregador não encontrado");
            }

            _context.Entregadores.Update(entregador);
            await _context.SaveChangesAsync();

            return Ok("CNH enviada com sucesso");
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entregador>>> GetEntregadores()
        {
            var entregadores = await _context.Entregadores.ToListAsync();
            return Ok(entregadores);
        }

        public class CnhRequest
        {
            public string ImagemCnh { get; set; }
        }
    }
}
