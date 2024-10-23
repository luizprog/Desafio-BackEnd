using Microsoft.AspNetCore.Mvc;
using LocacaoDesafioBackEnd.Models;
using System.Collections.Generic;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using LocacaoDesafioBackEnd.Services;

namespace LocacaoDesafioBackEnd.Controllers
{
    [ApiController]
    [Route("locacoes")]
    public class LocacaoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RabbitMqService _rabbitMqService;

        public LocacaoController(ApplicationDbContext context, RabbitMqService rabbitMqService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _rabbitMqService = rabbitMqService ?? throw new ArgumentNullException(nameof(rabbitMqService));

        }

        /// <summary>
        /// Listar todas as locações
        /// </summary>
        /// <returns>Uma lista de locações cadastradas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            var locacoes = await _context.Locacoes.ToListAsync();
            return Ok(locacoes);
        }

        /// <summary>
        /// Cadastrar nova locação
        /// </summary>
        /// <param name="locacao">Dados da nova locação</param>
        /// <returns>A locação cadastrada</returns>
        [HttpPost]
        public async Task<IActionResult> PostLocacao([FromBody] Locacao locacao)
        {
            // Acesse as IDs da moto e do entregador, que são do tipo inteiro
            var motoExistente = await _context.Motos.FindAsync(locacao.MotoId);
            var entregadorExistente = await _context.Entregadores.FindAsync(locacao.EntregadorId);

            // Verifique se o modelo está válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifique se a moto e o entregador existem
            if (motoExistente == null || entregadorExistente == null)
            {
                return NotFound("Moto ou entregador não encontrado.");
            }

            // Crie a nova locação referenciando as entidades existentes
            var novaLocacao = new Locacao
            {
                MotoId = locacao.MotoId,
                EntregadorId = locacao.EntregadorId,
                DataLocacao = locacao.DataLocacao,
                DataDevolucao = locacao.DataDevolucao,
                Moto = motoExistente, // Opcional, se você precisar da moto completa
                Entregador = entregadorExistente // Opcional, se você precisar do entregador completo
            };

            _context.Locacoes.Add(novaLocacao);
            await _context.SaveChangesAsync();

            // Retorne a resposta adequada após o cadastro
            return CreatedAtAction(nameof(GetLocacoes), new { id = novaLocacao.Id }, novaLocacao);
        }


        /// <summary>
        /// Atualizar locação existente
        /// </summary>
        /// <param name="id">ID da locação</param>
        /// <param name="locacao">Dados atualizados</param>
        /// <returns>A locação atualizada</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocacao(int id, [FromBody] Locacao locacao)
        {
            if (id != locacao.Id)
            {
                return BadRequest("ID da locação não corresponde.");
            }

            var locacaoExistente = await _context.Locacoes
                .Include(l => l.Moto)
                .Include(l => l.Entregador)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (locacaoExistente == null)
            {
                return NotFound("Locação não encontrada.");
            }

            // Verifique se a moto e o entregador existem
            var motoExistente = await _context.Motos.FindAsync(locacao.MotoId);
            var entregadorExistente = await _context.Entregadores.FindAsync(locacao.EntregadorId);

            if (motoExistente == null || entregadorExistente == null)
            {
                return NotFound("Moto ou entregador não encontrado.");
            }

            // Atualize os dados da locação
            locacaoExistente.DataDevolucao = locacao.DataDevolucao;
            locacaoExistente.DataLocacao = locacao.DataLocacao;
            locacaoExistente.Entregador = entregadorExistente;
            locacaoExistente.EntregadorId = locacao.EntregadorId; // Mantém a referência ao entregador existente
            locacaoExistente.Moto = motoExistente;
            locacaoExistente.MotoId = locacao.MotoId; // Mantém a referência à moto existente

            // Não atribuímos a nova instância de Moto ou Entregador aqui, apenas suas IDs

            try
            {
                _context.Entry(locacaoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro ao atualizar a locação.");
            }

            return NoContent();
        }


        /// <summary>
        /// Cancelar uma locação
        /// </summary>
        /// <param name="id">ID da locação a ser cancelada</param>
        /// <returns>Status de cancelamento</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
            {
                return NotFound("Locação não encontrada.");
            }

            _context.Locacoes.Remove(locacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
