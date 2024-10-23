using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace LocacaoDesafioBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cadastrar nova moto
        /// </summary>
        /// <param name="moto">Modelo da moto a ser cadastrada</param>
        /// <returns>Objeto da moto cadastrada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Create([FromBody] Moto moto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a placa já existe
            if (await _context.Motos.AnyAsync(m => m.Placa == moto.Placa))
            {
                return BadRequest(new { message = "A placa já está em uso." });
            }

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            // Publicar o evento de moto cadastrada
            //await _motoEventService.PublishMotoCadastradaEvent(moto);

            return CreatedAtAction(nameof(Details), new { id = moto.Id }, moto);
        }

        /// <summary>
        /// Obter todas as motos
        /// </summary>
        /// <returns>Lista de motos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Moto>), StatusCodes.Status200OK)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Index()
        {
            var motos = await _context.Motos.ToListAsync();
            return Ok(motos);
        }

        /// <summary>
        /// Obter detalhes de uma moto pelo ID
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Detalhes da moto</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Details(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
            {
                return NotFound();
            }
            return Ok(moto);
        }

        /// <summary>
        /// Atualizar a placa de uma moto existente
        /// </summary>
        /// <param name="id">ID da moto cuja placa será atualizada</param>
        /// <param name="placa">Nova placa para a moto</param>
        /// <returns>Resposta com a moto atualizada</returns>
        [HttpPut("{id}/placa")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> UpdatePlaca(int id, [FromBody] string placa)
        {
            if (string.IsNullOrEmpty(placa))
            {
                return BadRequest(new { message = "Placa não pode ser vazia." });
            }

            var existingMoto = await _context.Motos.FindAsync(id);
            if (existingMoto == null)
            {
                return NotFound(new { message = "Moto não encontrada." });
            }

            // Atualizar a placa da moto
            existingMoto.Placa = placa;

            await _context.SaveChangesAsync();

            return Ok(existingMoto); // Retorna a moto com a placa atualizada
        }

        /// <summary>
        /// Deletar uma moto pelo ID
        /// </summary>
        /// <param name="id">ID da moto a ser deletada</param>
        /// <returns>Resposta sem conteúdo</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Delete(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
            {
                return NotFound();
            }

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
