using DotnetTemplateApp.Api.Security.Jwt.Dtos;
using DotnetTemplateApp.Api.Security.Jwt.Interfaces;
using DotnetTemplateApp.Domain.Models.Security;
using DotnetTemplateApp.Shared.ConfigurationSettings.Common;
using DotnetTemplateApp.Shared.ConfigurationSettings.Security;
using DotnetTemplateApp.Shared.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotnetTemplateApp.Api.Security.Jwt
{
    public class JwtUtils(IOptions<JwtOptions> jwtOptions, ILogger<JwtUtils> logger) : IJwtUtils
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.GetSettings();

        public string GenerateJwtToken(Guid userAccountId, Guid userId)
        {
            var claims = new List<Claim>
            {
                new("userAccountId", $"{userAccountId}"),
                new("userId", $"{userId}")
            };

            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey ?? string.Empty);
            var symmetricKey = new SymmetricSecurityKey(key)
            {
                KeyId = _jwtOptions.Kid
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenTTLMinutes),
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public async Task<JwtTokenClaims?> ValidateJwtToken(string? token)
        {
            if (token == null) return null;

            try
            {
                var tokenHandler = new JsonWebTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey ?? string.Empty);
                var symmetricKey = new SymmetricSecurityKey(key)
                {
                    KeyId = _jwtOptions.Kid
                };

                var validationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = symmetricKey,
                    ClockSkew = TimeSpan.FromMinutes(1)
                });

                if (!validationResult.IsValid)
                {
                    throw validationResult.Exception ?? new SecurityTokenException("Unhandled JWT validation error");
                }

                var jwtToken = (JsonWebToken)validationResult.SecurityToken;

                return ExtractClaims(jwtToken.Claims);
            }
            catch (SecurityTokenExpiredException)
            {
                throw;
            }
            catch (SecurityTokenInvalidLifetimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogWarning("JwtToken validation failed. Error message: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public Token GenerateRefreshToken(Guid userAccountId)
        {
            var refreshToken = new Token
            {
                UserAccountId = userAccountId,
                RefreshToken = CreateToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }

        private JwtTokenClaims ExtractClaims(IEnumerable<Claim> claims)
        {
            var claimUserAccountId = claims.FirstOrDefault(x => x.Type == "userAccountId")?.Value;
            var claimUserId = claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            var userAccountId = claimUserAccountId.TryParseNullable<Guid>();
            var userId = claimUserId.TryParseNullable<Guid>();
            if (userId == Guid.Empty || userAccountId == Guid.Empty)
            {
                logger.LogWarning("Failed to parse userAccountId {UserAccountId} or userId {UserId} from claims", claimUserAccountId, claimUserId);
            }

            return new JwtTokenClaims { UserAccountId = userAccountId, UserId = userId };
        }

        #region Helpers
        private static string CreateToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return token;
        }

        #endregion
    }
}
