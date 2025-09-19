using DotnetTemplateApp.Core.Services.Account.Dtos.Response;

namespace DotnetTemplateApp.Core.Services.Account.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UseRequestDto?>> GetUsers(Guid userId);
        Task<UseRequestDto?> GetUser(Guid userId);
        Task<UseRequestDto> GetUserByAccountId(Guid userAccountId);
    }
}
