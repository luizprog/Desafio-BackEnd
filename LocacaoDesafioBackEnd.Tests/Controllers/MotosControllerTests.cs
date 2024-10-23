using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using LocacaoDesafioBackEnd.Controllers;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LocacaoDesafioBackEnd.Tests.Controllers
{
    public class MotosControllerTests
    {
        private MotosController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new MotosController(_context);
        }

        [Fact]
        public async Task Index_ReturnsOkResult_WithListOfMotos()
        {
            // Arrange
            var moto = new Moto
            {
                Id = 1,
                Modelo = "Modelo A",
                Placa = "ABC1234"
            };
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var okResult = result as ActionResult<IEnumerable<Moto>>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(1, okResult.Value.Count());
        }

        [Fact]
        public async Task Create_ValidMoto_ReturnsCreatedAtAction()
        {
            // Arrange
            var moto = new Moto
            {
                Id = 2,
                Modelo = "Modelo B",
                Placa = "XYZ5678"
            };

            // Act
            var result = await _controller.Create(moto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(moto, createdResult.Value);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsOkResult()
        {
            // Arrange
            var moto = new Moto
            {
                Id = 3,
                Modelo = "Modelo C",
                Placa = "DEF2345"
            };
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Details(moto.Id);

            // Assert
            var okResult = result as ActionResult<Moto>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(moto, okResult.Value);
        }

        [Fact]
        public async Task UpdatePlaca_ValidId_ReturnsOkResult_WithUpdatedMoto()
        {
            // Arrange
            var moto = new Moto
            {
                Id = 4,
                Modelo = "Modelo D",
                Placa = "GHI9876"
            };
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            var novaPlaca = "JKL1234";

            // Act
            var result = await _controller.UpdatePlaca(moto.Id, novaPlaca);

            // Assert
            var okResult = result as ActionResult<Moto>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(novaPlaca, okResult.Value.Placa);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            // Arrange
            var moto = new Moto
            {
                Id = 5,
                Modelo = "Modelo E",
                Placa = "MNO4567"
            };
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(moto.Id);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
