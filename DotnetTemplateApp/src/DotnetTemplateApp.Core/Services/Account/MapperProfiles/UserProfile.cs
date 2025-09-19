using AutoMapper;
using DotnetTemplateApp.Core.Services.Account.Dtos.Response;
using DotnetTemplateApp.Domain.Models.Account;
using DotnetTemplateApp.Shared.ConfigurationSettings.Common;

namespace DotnetTemplateApp.Core.Services.Account.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponseDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.UserId))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(x => ConvertHelpers.TryParseDateTimeAsUtcNullable(x.DateOfBirth) ?? DateTime.MinValue))
                .ReverseMap();
        }
    }
}
