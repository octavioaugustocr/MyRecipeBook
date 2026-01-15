namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe?> GetById(Entities.User user, long id);
    void Update(Entities.Recipe recipe);
}