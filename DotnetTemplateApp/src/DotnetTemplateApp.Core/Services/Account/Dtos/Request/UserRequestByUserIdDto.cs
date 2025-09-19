using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Core.Services.Account.Dtos.Request
{
    [ExcludeFromCodeCoverage]
    public record UserRequestByUserIdDto
    {
        public Guid UserId { get; set; }
    }
}
