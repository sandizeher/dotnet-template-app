using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Core.Services.Account.Dtos.Response
{
    [ExcludeFromCodeCoverage]
    public record UserResponseDto
    {
        public Guid UserId { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public string? Middlename { get; set; }
        public required string UserAlias { get; set; }
        public required string DateOfBirth { get; set; }
    }
}
