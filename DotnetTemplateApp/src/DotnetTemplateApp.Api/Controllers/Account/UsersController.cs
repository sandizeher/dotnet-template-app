using DotnetTemplateApp.Api.RateLimiting;
using DotnetTemplateApp.Api.Security.Attributes;
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
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            var userId = ConvertHelpers.TryParseNullable<Guid>(HttpContext.Items["UserId"]?.ToString());
            if (userId == Guid.Empty)
            {
                return Unauthorized(GenericErrorResponse.UnauthorizedAccess.Message);
            }

            var result = await userService.GetUsers(userId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            var result = await userService.GetUser(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
