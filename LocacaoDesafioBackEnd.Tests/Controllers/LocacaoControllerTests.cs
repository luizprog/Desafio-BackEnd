using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using LocacaoDesafioBackEnd;
using Microsoft.EntityFrameworkCore;
using LocacaoDesafioBackEnd.Data;
using LocacaoDesafioBackEnd.Models;
using Newtonsoft.Json;
using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace LocacaoDesafioBackEnd.Tests.Controllers;
public class LocacaoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public LocacaoControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // Configurar banco de dados em memória para os testes
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("LocacaoTestDb")
            .Options;
        var context = new ApplicationDbContext(options);
        return context;
    }
    [Fact]
    public async Task GetLocacoes_Returns_OkResponse()
    {
        // Arrange
        var context = GetDbContext();
        context.Locacoes.Add(new Locacao { Id = 1, MotoId = 1, EntregadorId = 1, DataLocacao = DateTime.UtcNow });
        context.SaveChanges();

        // Act
        var response = await _client.GetAsync("/locacoes");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Id");
    }
    [Fact]
    public async Task PostLocacao_Creates_NewLocacao()
    {
        // Arrange
        var novoLocacao = new
        {
            MotoId = 1,
            EntregadorId = 1,
            DataLocacao = DateTime.UtcNow,
            Valor = 100,
            ValorDiaria = 20
        };
        var content = new StringContent(JsonConvert.SerializeObject(novoLocacao), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/locacoes?duracaoDias=5", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var contentResponse = await response.Content.ReadAsStringAsync();
        contentResponse.Should().Contain("Id");
    }

    [Fact]
    public async Task PutLocacao_Updates_ExistingLocacao()
    {
        // Arrange
        var context = GetDbContext();
        var locacaoExistente = new Locacao
        {
            Id = 1,
            MotoId = 1,
            EntregadorId = 1,
            DataLocacao = DateTime.UtcNow,
            Valor = 100,
            ValorDiaria = 20
        };
        context.Locacoes.Add(locacaoExistente);
        context.SaveChanges();

        var locacaoAtualizada = new Locacao
        {
            Id = 1,
            MotoId = 2,  // Novo moto ID
            EntregadorId = 1,
            DataLocacao = DateTime.UtcNow,
            Valor = 150,
            ValorDiaria = 30
        };
        var content = new StringContent(JsonConvert.SerializeObject(locacaoAtualizada), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/locacoes/{locacaoExistente.Id}", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // Verificar se a locação foi atualizada no banco
        var updatedLocacao = context.Locacoes.Find(locacaoExistente.Id);
        updatedLocacao.Should().NotBeNull();
        updatedLocacao.Valor.Should().Be(150);
    }

    [Fact]
    public async Task DeleteLocacao_Removes_Locacao()
    {
        // Arrange
        var context = GetDbContext();
        var locacaoExistente = new Locacao
        {
            Id = 1,
            MotoId = 1,
            EntregadorId = 1,
            DataLocacao = DateTime.UtcNow,
            Valor = 100,
            ValorDiaria = 20
        };
        context.Locacoes.Add(locacaoExistente);
        context.SaveChanges();

        // Act
        var response = await _client.DeleteAsync($"/locacoes/{locacaoExistente.Id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // Verificar se a locação foi removida do banco
        var deletedLocacao = context.Locacoes.Find(locacaoExistente.Id);
        deletedLocacao.Should().BeNull();
    }

    [Fact]
    public async Task ConsultarValorTotal_Returns_ValorTotalEstimado()
    {
        // Arrange
        var context = GetDbContext();
        var locacaoExistente = new Locacao
        {
            Id = 1,
            MotoId = 1,
            EntregadorId = 1,
            DataLocacao = DateTime.UtcNow,
            ValorDiaria = 50,
            ValorTotal = 200
        };
        context.Locacoes.Add(locacaoExistente);
        context.SaveChanges();

        // Act
        var response = await _client.GetAsync($"/locacoes/{locacaoExistente.Id}/calcular?dataDevolucao={DateTime.UtcNow.AddDays(3)}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("ValorTotalEstimado");
    }


}