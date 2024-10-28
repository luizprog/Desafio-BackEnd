using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using LocacaoDesafioBackEnd.Events;
using LocacaoDesafioBackEnd.Services;
using Microsoft.AspNetCore.Authorization;

namespace LocacaoDesafioBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MotoService _motoService;

        public MotosController(ApplicationDbContext context, MotoService motoService)
        {
            _context = context;
            _motoService = motoService; // Inicializa o MotoService
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

            await _motoService.PublishMotoCadastradaEvent(moto);

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
        /// Buscar moto por placa (Acesso apenas para Admin)
        /// </summary>
        /// <param name="placa">Placa da moto a ser buscada</param>
        /// <returns>Detalhes da moto</returns>
        [HttpGet("placa/{placa}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
            if (moto == null)
            {
                return NotFound(new { message = "Moto não encontrada." });
            }
            return Ok(moto);
        }

        /// <summary>
        /// Atualizar a placa de uma moto existente (Acesso apenas para Admin)
        /// </summary>
        /// <param name="id">ID da moto cuja placa será atualizada</param>
        /// <param name="placa">Nova placa para a moto</param>
        /// <returns>Resposta com a moto atualizada</returns>
        [HttpPut("{id}/placa")]
        [Authorize(Roles = "Admin")]
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

    }
}
