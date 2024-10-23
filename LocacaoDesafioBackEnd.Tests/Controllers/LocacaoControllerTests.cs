using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocacaoDesafioBackEnd.Controllers;
using LocacaoDesafioBackEnd.Data;
using LocacaoDesafioBackEnd.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LocacaoDesafioBackEnd.Tests.Controllers
{
    public class LocacaoControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly LocacaoController _controller;

        public LocacaoControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new LocacaoController(_mockContext.Object);
        }

        [Fact]
        public async Task GetLocacoes_ReturnsOkResult_WithListOfLocacoes()
        {
            // Arrange
            var locacoes = new List<Locacao>
            {
                new Locacao { Id = 1, EntregadorId = 1, MotoId = 1, DataLocacao = DateTime.Now, DataDevolucao = DateTime.Now.AddDays(1) },
                new Locacao { Id = 2, EntregadorId = 2, MotoId = 2, DataLocacao = DateTime.Now, DataDevolucao = DateTime.Now.AddDays(2) }
            };

            _mockContext.Setup(c => c.Locacoes.ToListAsync()).ReturnsAsync(locacoes);

            // Act
            var result = await _controller.GetLocacoes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Locacao>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task PostLocacao_ReturnsCreatedAtAction_WithLocacao()
        {
            // Arrange
            var locacao = new Locacao { Id = 3, EntregadorId = 1, MotoId = 1, DataLocacao = DateTime.Now, DataDevolucao = DateTime.Now.AddDays(1) };

            _mockContext.Setup(c => c.Locacoes.AddAsync(locacao, default)).ReturnsAsync((Locacao locacao, CancellationToken ct) => locacao);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PostLocacao(locacao);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Locacao>(createdResult.Value);
            Assert.Equal(locacao.Id, returnValue.Id);
        }

        // Adicione outros testes conforme necessário
    }
}
