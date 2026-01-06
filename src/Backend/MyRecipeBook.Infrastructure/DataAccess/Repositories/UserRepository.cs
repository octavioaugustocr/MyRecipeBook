using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public UserRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _dbContext
                .Users
                .AsNoTracking() // cannot be edited
                .FirstOrDefaultAsync(u => u.Active && u.Email.Equals(email) && u.Password.Equals(password));
        }
    }
}
