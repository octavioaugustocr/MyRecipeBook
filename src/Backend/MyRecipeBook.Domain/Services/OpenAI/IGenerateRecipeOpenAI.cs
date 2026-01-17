using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Services.OpenAI
{
    public interface IGenerateRecipeOpenAI
    {
        Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
    }
}
