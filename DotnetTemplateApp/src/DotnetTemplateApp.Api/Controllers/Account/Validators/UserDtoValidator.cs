using DotnetTemplateApp.Core.Services.Account.Dtos.Response;
using DotnetTemplateApp.Shared.Extensions;
using FluentValidation;

namespace DotnetTemplateApp.Api.Controllers.Account.Validators
{
    public class UserDtoValidator : AbstractValidator<UseRequestDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Firstname).NotEmpty();
            RuleFor(x => x.Lastname).NotEmpty();
            RuleFor(x => x.DateOfBirth!).Must(StringExtensions.IsValidDate).When(x => !x.DateOfBirth.IsNullOrEmpty());
        }
    }
}
