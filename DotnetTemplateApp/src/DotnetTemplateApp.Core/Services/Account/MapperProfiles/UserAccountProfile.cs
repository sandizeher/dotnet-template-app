using AutoMapper;
using DotnetTemplateApp.Core.Services.Account.Dtos.Response;
using DotnetTemplateApp.Domain.Models.Account;

namespace DotnetTemplateApp.Core.Services.Account.MapperProfiles
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            CreateMap<UserAccountResponseDto, UserAccount>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.UserAccountId))
                .ReverseMap();
        }
    }
}
