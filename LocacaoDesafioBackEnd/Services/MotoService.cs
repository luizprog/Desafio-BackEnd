using LocacaoDesafioBackEnd.Events;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Services;
using System.Threading.Tasks;

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
            await Task.CompletedTask;
        });
    }
}
