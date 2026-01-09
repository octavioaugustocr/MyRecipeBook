using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestFilterRecipeJsonBuilder
    {
        public static RequestFilterRecipeJson Build()
        {
            return new Faker<RequestFilterRecipeJson>()
                .RuleFor(filter => filter.CookingTimes, (f) => f.Make(1, () => f.PickRandom<CookingTime>()))
                .RuleFor(filter => filter.Difficulties, (f) => f.Make(1, () => f.PickRandom<Difficulty>()))
                .RuleFor(filter => filter.DishTypes, (f) => f.Make(1, () => f.PickRandom<DishType>()))
                .RuleFor(filter => filter.RecipeTitle_Ingredient, (f) => f.Lorem.Word());
        }
    }
}
