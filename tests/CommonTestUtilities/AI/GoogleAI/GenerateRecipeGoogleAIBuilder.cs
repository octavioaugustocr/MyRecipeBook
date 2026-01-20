using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Services.GoogleAI;

namespace CommonTestUtilities.AI.GoogleAI
{
    public class GenerateRecipeGoogleAIBuilder
    {
        public static IGenerateRecipeGoogleAI Build(GeneratedRecipeDto dto)
        {
            var mock = new Mock<IGenerateRecipeGoogleAI>();

            mock.Setup(service => service.Generate(It.IsAny<List<string>>())).ReturnsAsync(dto);

            return mock.Object;
        }
    }
}
