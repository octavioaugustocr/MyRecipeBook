using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Services.GoogleAI
{
    public interface IGenerateRecipeGoogleAI
    {
        Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
    }
}
