
using MyRecipeBook.Domain.Services.ServiceBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MyRecipeBook.API.BackgroundServices
{
    public class DeleteUserServiceRabbitMQ : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName = "delete-user-queue";

        public DeleteUserServiceRabbitMQ(IServiceProvider services)
        {
            _services = services;

            var factory = new ConnectionFactory() { Uri = new Uri("amqp://guest:guest@localhost:5672/") };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var userIdentifier = Guid.Parse(message);

                using var scope = _services.CreateScope();
                var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();
                await deleteUserUseCase.Execute(userIdentifier);

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
