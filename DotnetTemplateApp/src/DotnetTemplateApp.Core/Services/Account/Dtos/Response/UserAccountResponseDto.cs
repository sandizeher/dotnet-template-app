using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling;
using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Core.Services.Account.Dtos.Response
{
    [ExcludeFromCodeCoverage]
    public record UserAccountResponseDto : ErrorDetailsBase
    {
        public Guid UserAccountId { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberPrefix { get; set; }
    }
}
