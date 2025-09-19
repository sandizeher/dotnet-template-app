using DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account;
using DotnetTemplateApp.Domain.Models.Account;
using DotnetTemplateApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DotnetTemplateApp.Persistence.Repositories.Account
{
    public class UserAccountRepository(ApplicationDbContext context) : GenericRepository<UserAccount>(context), IUserAccountRepository
    {
        public Task<bool> EmailExistsAsync(string email)
        {
            return _dbSet
                .AsNoTracking()
                .AnyAsync(x => x.Email == email.ToLowerInvariant().Trim());
        }

        public Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return _dbSet
                .AsNoTracking()
                .AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public Task<bool> UsernameExistsAsync(string username)
        {
            return _dbSet
                .AsNoTracking()
                .AnyAsync(x => x.Username == username);
        }
    }
}
