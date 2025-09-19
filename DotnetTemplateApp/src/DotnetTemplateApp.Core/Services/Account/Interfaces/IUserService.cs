using DotnetTemplateApp.Core.Services.Account.Dtos.Response;

namespace DotnetTemplateApp.Core.Services.Account.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto?>> GetUsers(Guid userId);
        Task<UserResponseDto?> GetUser(Guid userId);
        Task<UserResponseDto> GetUserByAccountId(Guid userAccountId);
    }
}
