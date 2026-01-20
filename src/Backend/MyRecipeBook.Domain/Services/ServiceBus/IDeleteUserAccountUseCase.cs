namespace MyRecipeBook.Domain.Services.ServiceBus
{
    public interface IDeleteUserAccountUseCase
    {
        public Task Execute(Guid userIdentifier);
    }
}
