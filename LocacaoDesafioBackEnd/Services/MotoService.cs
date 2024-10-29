using LocacaoDesafioBackEnd.Data; // Adicione o namespace para o ApplicationDbContext
using LocacaoDesafioBackEnd.Events;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Models.Notifications;
using LocacaoDesafioBackEnd.Services;

public class MotoService
{
    private readonly IMessageBus _messageBus;
    private readonly ApplicationDbContext _dbContext; // Use ApplicationDbContext

    public MotoService(IMessageBus messageBus, ApplicationDbContext dbContext) // Injetando o ApplicationDbContext
    {
        _messageBus = messageBus;
        _dbContext = dbContext; // Inicializando o ApplicationDbContext
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
            await StoreNotification(message); // Armazena a notificação
        }
        await Task.CompletedTask; // Simulando um processamento assíncrono
    }

    private void NotifyFor2024(MotoCadastradaEvent message)
    {
        // Lógica de notificação quando o ano for 2024
        Console.WriteLine($"Notificação: A moto cadastrada com ID {message.Id} é de 2024!");
    }

    private async Task StoreNotification(MotoCadastradaEvent message)
    {
        var notificacao = new Notificacao
        {
            Tipo = "Moto", // Definindo o tipo de notificação
            Mensagem = $"A moto cadastrada com ID {message.Id} é de 2024!",
            DataCriacao = DateTime.UtcNow
        };

        await _dbContext.Notificacoes.AddAsync(notificacao);
        await _dbContext.SaveChangesAsync(); // Salva a notificação

        var motoNotificacao = new MotoNotificacao
        {
            NotificacaoId = notificacao.Id,
            MotoId = message.Id
        };

        await _dbContext.MotoNotificacoes.AddAsync(motoNotificacao);
        await _dbContext.SaveChangesAsync(); // Salva a associação com a moto
    }

    public async Task PublishMotoExcluidaEvent(Moto moto)
    {

        await Task.Run(() =>
        {
            Console.WriteLine($"Moto excluída: {moto.Id}");
        });
    }

}
