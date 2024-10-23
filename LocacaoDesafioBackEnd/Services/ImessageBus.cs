using LocacaoDesafioBackEnd.Services; // Para IMessageBus
using LocacaoDesafioBackEnd.Data; // Para ApplicationDbContext



namespace LocacaoDesafioBackEnd.Services
{
    public interface IMessageBus
    {
        void Publish<T>(T message) where T : class;
        void Subscribe<T>(Action<T> onMessage) where T : class;

        Task PublishAsync<T>(T message) where T : class;
        Task SubscribeAsync<T>(Func<T, Task> onMessage) where T : class;
    }
}
