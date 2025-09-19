using DotnetTemplateApp.Core.Common.Dtos;
using DotnetTemplateApp.Core.Services.Account.Dtos.Response;

namespace DotnetTemplateApp.Core.Services.Account.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccountResponseDto?> GetUserAccount(Guid userAccountId);
        Task<UserAccountResponseDto?> GetUserAccountByEmail(string email);
        Task<GenericResponseDto> DeleteUserAccount(Guid userId);
    }
}
