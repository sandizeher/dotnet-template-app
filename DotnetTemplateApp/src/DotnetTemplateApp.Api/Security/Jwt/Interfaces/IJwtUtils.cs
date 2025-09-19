using DotnetTemplateApp.Api.Security.Jwt.Dtos;
using DotnetTemplateApp.Domain.Models.Security;

namespace DotnetTemplateApp.Api.Security.Jwt.Interfaces
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(Guid userAccountId, Guid userId);
        public Task<JwtTokenClaims?> ValidateJwtToken(string? token);
        public Token GenerateRefreshToken(Guid userAccountId);
    }
}
