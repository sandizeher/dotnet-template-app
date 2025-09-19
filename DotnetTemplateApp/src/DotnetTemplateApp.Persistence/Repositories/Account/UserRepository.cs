using DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account;
using DotnetTemplateApp.Domain.Models.Account;
using DotnetTemplateApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DotnetTemplateApp.Persistence.Repositories.Account
{
    public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        public Task<User?> GetUserByAccountId(Guid userAccountId)
        {
            return _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserAccountId == userAccountId);
        }

        public Task<Guid> GetUserIdByAccountId(Guid userAccountId)
        {
            return _dbSet
                .AsNoTracking()
                .Where(x => x.UserAccountId == userAccountId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public Task<User?> GetUserDetails(Guid userId)
        {
            return _dbSet
                .Include(u => u.UserAccount)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

    }
}
