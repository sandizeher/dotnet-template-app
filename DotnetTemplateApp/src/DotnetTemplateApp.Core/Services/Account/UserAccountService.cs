using AutoMapper;
using DotnetTemplateApp.Core.Common.Dtos;
using DotnetTemplateApp.Core.Services.Account.Dtos.Response;
using DotnetTemplateApp.Core.Services.Account.Interfaces;
using DotnetTemplateApp.Domain.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace DotnetTemplateApp.Core.Services.Account
{
    public class UserAccountService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UserAccountService> logger) : IUserAccountService
    {

        public async Task<UserAccountResponseDto?> GetUserAccount(Guid userAccountId)
        {
            var user = await unitOfWork.UserAccountRepository.GetById(userAccountId);
            return mapper.Map<UserAccountResponseDto>(user);
        }

        public async Task<UserAccountResponseDto?> GetUserAccountByEmail(string email)
        {
            var user = await unitOfWork.UserAccountRepository.GetSingle(x => x.Email == email, asNoTracking: true);
            return mapper.Map<UserAccountResponseDto>(user);
        }

        public async Task<GenericResponseDto> DeleteUserAccount(Guid userId)
        {
            await unitOfWork.UserRepository.ExecuteDeleteAsync(x => x.Id == userId);
            // Add logic for deleting user account related data if needed
            logger.LogInformation("Deleted user account with ID: {UserId}", userId);
            return new() { IsSuccess = true };
        }
    }
}
