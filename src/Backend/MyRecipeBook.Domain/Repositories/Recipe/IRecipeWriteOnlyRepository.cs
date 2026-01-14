namespace MyRecipeBook.Domain.Repositories.Recipe
{
    public interface IRecipeWriteOnlyRepository
    {
        public Task Add(Domain.Entities.Recipe recipe);
        public Task Delete(long recipeId);
    }
}
