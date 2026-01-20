/*
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.API.BackgroundServices
{
    public class DeleteUserServiceAzure : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IServiceBusProcessor _processor;

        public DeleteUserServiceAzure(IServiceProvider services, IServiceBusProcessor processor)
        {
            _services = services;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += ProcessMessageAsync;

            _processor.ProcessErrorAsync += ExceptionReceivedHandler;

            await _processor.StartProcessingAsync(stoppingToken);
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
        {
            var message = eventArgs.Message.Body.ToString();

            var userIdentifier = Guid.Parse(message);

            var scope = _services.CreateScope();

            var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

            await deleteUserUseCase.Execute(userIdentifier);
        }

        private async Task ExceptionReceivedHandler(ProcessErrorEventArgs _) => Task.CompletedTask;

        ~DeleteUserServiceAzure() => Dispose();

        public override void Dispose()
        {
            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
*/