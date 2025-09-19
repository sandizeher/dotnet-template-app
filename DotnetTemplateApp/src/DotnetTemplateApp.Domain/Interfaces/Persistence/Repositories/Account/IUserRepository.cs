using DotnetTemplateApp.Domain.Models.Account;

namespace DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByAccountId(Guid userAccountId);
        Task<User?> GetUserDetails(Guid userId);
    }
}
