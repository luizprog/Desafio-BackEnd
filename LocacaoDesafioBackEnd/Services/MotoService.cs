using LocacaoDesafioBackEnd.Events;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Services;

public class MotoService
{
    private readonly IMessageBus _messageBus;

    public MotoService(IMessageBus messageBus)
    {
        _messageBus = messageBus;
        SubscribeToMotoCadastradaEvent();
    }

    public async Task PublishMotoCadastradaEvent(Moto moto)
    {
        var motoCadastradaEvent = new MotoCadastradaEvent
        {
            Id = moto.Id,
            Modelo = moto.Modelo,
            Marca = moto.Marca,
            Placa = moto.Placa,
            Ano = moto.Ano,
            Cor = moto.Cor
        };
        await _messageBus.PublishAsync(motoCadastradaEvent);
    }

    public void SubscribeToMotoCadastradaEvent()
    {
        _messageBus.SubscribeAsync<MotoCadastradaEvent>(async message =>
        {
            await HandleMotoCadastradaEvent(message);
        });
    }

    private async Task HandleMotoCadastradaEvent(MotoCadastradaEvent message)
    {
        if (message.Ano == 2024)
        {
            NotifyFor2024(message);
        }
        await Task.CompletedTask; // Simulando um processamento assíncrono
    }

    private void NotifyFor2024(MotoCadastradaEvent message)
    {
        // Lógica de notificação quando o ano for 2024
        Console.WriteLine($"Notificação: A moto cadastrada com ID {message.Id} é de 2024!");
    }
}
