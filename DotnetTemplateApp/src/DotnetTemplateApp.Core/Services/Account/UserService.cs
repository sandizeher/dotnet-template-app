using AutoMapper;
using DotnetTemplateApp.Core.Services.Account.Dtos.Response;
using DotnetTemplateApp.Core.Services.Account.Interfaces;
using DotnetTemplateApp.Domain.Interfaces.Persistence;

namespace DotnetTemplateApp.Core.Services.Account
{
    public partial class UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IUserService
    {
        public async Task<IEnumerable<UserResponseDto?>> GetUsers(Guid listeningUserId)
        {
            var result = await unitOfWork.UserRepository.Get(asNoTracking: true,
                includeProperties: "UserAccount",
                orderBy: x => x.OrderBy(y => y.Firstname));

            return mapper.Map<IEnumerable<UserResponseDto>>(result);
        }

        public async Task<UserResponseDto?> GetUser(Guid userId)
        {
            var result = await unitOfWork.UserRepository.GetById(userId);
            return mapper.Map<UserResponseDto>(result);
        }

        public async Task<UserResponseDto> GetUserByAccountId(Guid userAccountId)
        {
            var result = await unitOfWork.UserRepository.GetUserByAccountId(userAccountId);
            return mapper.Map<UserResponseDto>(result);
        }
    }
}
