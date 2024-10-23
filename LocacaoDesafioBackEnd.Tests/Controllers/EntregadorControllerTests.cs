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
    public class EntregadorControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly EntregadorController _controller;

        public EntregadorControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new EntregadorController(_mockContext.Object);
        }

        [Fact]
        public async Task CadastrarEntregador_ReturnsCreatedAtAction_WithEntregador()
        {
            // Arrange
            var entregador = new Entregador { Id = 1, Nome = "João", TipoCnh = 2, DataNascimento = DateTime.Now.AddYears(-25) };

            _mockContext.Setup(c => c.Entregadores.AddAsync(entregador, default)).ReturnsAsync((Entregador entregador, CancellationToken ct) => entregador);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.CadastrarEntregador(entregador);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Entregador>(createdResult.Value);
            Assert.Equal(entregador.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetEntregadores_ReturnsOkResult_WithListOfEntregadores()
        {
            // Arrange
            var entregadores = new List<Entregador>
            {
                new Entregador { Id = 1, Nome = "João", TipoCnh = 2, DataNascimento = DateTime.Now.AddYears(-25) },
                new Entregador { Id = 2, Nome = "Maria", TipoCnh = 1, DataNascimento = DateTime.Now.AddYears(-30) }
            };

            _mockContext.Setup(c => c.Entregadores.ToListAsync()).ReturnsAsync(entregadores);

            // Act
            var result = await _controller.GetEntregadores();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Entregador>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // Adicione outros testes conforme necessário
    }
}
