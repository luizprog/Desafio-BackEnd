// File: Services/RabbitMqHostedService.cs
using LocacaoDesafioBackEnd.Services;
using Microsoft.Extensions.Hosting;

public class RabbitMqHostedService : IHostedService
{
    private readonly RabbitMqService _rabbitMqService;

    public RabbitMqHostedService(RabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rabbitMqService.ConsumeMessages(); // Começa a consumir mensagens
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Aqui você pode adicionar lógica para parar o consumo, se necessário
        return Task.CompletedTask;
    }
}
