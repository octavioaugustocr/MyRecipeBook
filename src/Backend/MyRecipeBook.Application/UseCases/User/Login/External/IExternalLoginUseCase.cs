namespace MyRecipeBook.Application.UseCases.User.Login.External
{
    public interface IExternalLoginUseCase
    {
        public Task<string> Execute(string name, string email);
    }
}
