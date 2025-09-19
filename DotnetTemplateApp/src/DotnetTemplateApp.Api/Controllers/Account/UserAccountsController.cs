using DotnetTemplateApp.Api.RateLimiting;
using DotnetTemplateApp.Api.Security.Attributes;
using DotnetTemplateApp.Core.Common.Dtos;
using DotnetTemplateApp.Core.Services.Account.Dtos.Response;
using DotnetTemplateApp.Core.Services.Account.Interfaces;
using DotnetTemplateApp.Shared.ConfigurationSettings.Common;
using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Api.Controllers.Account
{
    [ApiController]
    [Authorize]
    [EnableRateLimiting(DefaultRateLimiterPolicy.PolicyName)]
    [Produces("application/json")]
    [Route("[controller]")]
    [ExcludeFromCodeCoverage]
    public class UserAccountsController(IUserAccountService userAccountService) : ControllerBase
    {
        [HttpGet("current")]
        [ProducesResponseType(typeof(UserAccountResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserAccount()
        {
            var userAccountId = ConvertHelpers.TryParseNullable<Guid>(HttpContext.Items["UserAccountId"]?.ToString());
            if (userAccountId == Guid.Empty)
            {
                return Unauthorized(GenericErrorResponse.UnauthorizedAccess.Message);
            }

            var result = await userAccountService.GetUserAccount(userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("{userAccountId}")]
        [ProducesResponseType(typeof(UserAccountResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByAccountId([FromRoute] Guid userAccountId)
        {
            var result = await userAccountService.GetUserAccount(userAccountId);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPatch("delete")]
        [EnableRateLimiting(SecurityRateLimiterPolicy.PolicyName)]
        [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserAccount()
        {
            var userId = ConvertHelpers.TryParseNullable<Guid>(HttpContext.Items["UserId"]?.ToString());
            if (userId == Guid.Empty)
            {
                return Unauthorized(GenericErrorResponse.UnauthorizedAccess.Message);
            }

            var result = await userAccountService.DeleteUserAccount(userId);
            return Ok(result);
        }
    }
}
