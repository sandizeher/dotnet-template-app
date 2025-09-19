using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Api.Security.Jwt.Dtos
{
    [ExcludeFromCodeCoverage]
    public record JwtTokenClaims
    {
        public Guid? UserAccountId { get; set; }
        public Guid? UserId { get; set; }
    }
}
