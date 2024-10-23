using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Messaging;
using Microsoft.Extensions.Configuration;

namespace LocacaoDesafioBackEnd.Services
{
    public class RabbitMqService : IMessageBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            var hostname = configuration["RabbitMQ:HostName"]; // Lê o hostname do arquivo de configuração
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "vehicle-registered",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        // Método Publish genérico com restrições (verificar restrições na interface IMessageBus)
        public void Publish<T>(T message) where T : class
        {
            var messageString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish(exchange: "",
                                 routingKey: "vehicle-registered",
                                 basicProperties: null,
                                 body: body);
        }

        // Método PublishAsync para envio de mensagens assíncrono
        public async Task PublishAsync<T>(T message) where T : class
        {
            await Task.Run(() =>
            {
                Publish(message);
            });
        }

        // Método Subscribe genérico com restrições
        public void Subscribe<T>(Action<T> callback) where T : class
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var deserializedMessage = JsonSerializer.Deserialize<T>(message);
                if (deserializedMessage != null)
                {
                    callback(deserializedMessage);
                }
            };

            _channel.BasicConsume(queue: "vehicle-registered", autoAck: true, consumer: consumer);
        }

        // Método SubscribeAsync para consumir mensagens assíncronas
        public async Task SubscribeAsync<T>(Func<T, Task> callback) where T : class
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var deserializedMessage = JsonSerializer.Deserialize<T>(message);
                if (deserializedMessage != null)
                {
                    await callback(deserializedMessage);
                }
            };

            _channel.BasicConsume(queue: "vehicle-registered", autoAck: true, consumer: consumer);
        }

        public void ConsumeMessages()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // Processar a mensagem aqui (ex: atualizar o banco de dados, notificar usuários, etc.)
            };
            _channel.BasicConsume(queue: "vehicle-registered",
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
