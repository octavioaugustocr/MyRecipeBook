using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Token
{
    public interface IUseRefreshTokenUseCase
    {
        Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
    }
}
