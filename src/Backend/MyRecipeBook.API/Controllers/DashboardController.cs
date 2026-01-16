using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Atributes;
using MyRecipeBook.Application.UseCases.Dashboard;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers
{
    [AuthenticatedUser]
    public class DashboardController : MyRecipeBookBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromServices] IGetDashboardUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Recipes.Any())
                return Ok(response.Recipes);

            return NoContent();
        }
    }
}
