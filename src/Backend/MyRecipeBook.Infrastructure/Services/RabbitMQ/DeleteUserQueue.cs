using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.ServiceBus;
using RabbitMQ.Client;
using System.Text;

namespace MyRecipeBook.Infrastructure.Services.RabbitMQ
{
    public class DeleteUserQueue : IDeleteUserQueue
    {
        private readonly IConnection _connection;
        private readonly string _queueName = "delete-user-queue";

        public DeleteUserQueue(IConnection connection)
        {
            _connection = connection;
        }

        public Task SendMessage(User user)
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(user.UserIdentifier.ToString());

            channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }
    }
}
