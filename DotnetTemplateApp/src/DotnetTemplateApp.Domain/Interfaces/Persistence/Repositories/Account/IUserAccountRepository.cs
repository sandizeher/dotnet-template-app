using DotnetTemplateApp.Domain.Models.Account;

namespace DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account
{
    public interface IUserAccountRepository : IRepository<UserAccount>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        Task<bool> UsernameExistsAsync(string username);
    }
}
