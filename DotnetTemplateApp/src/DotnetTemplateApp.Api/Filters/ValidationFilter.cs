using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DotnetTemplateApp.Api.Filters.Models;
using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling;

namespace DotnetTemplateApp.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class ValidationFilter(ILogger<ValidationFilter> logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(x => x.ErrorMessage).ToArray());

                var errorDetails = new List<ErrorDetails>();
                foreach (var error in errorsInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        errorDetails.Add(new ErrorDetails(ErrorType.ValidationError, FormValidationErrorTitle(error.Key), subError));
                    }
                }

                var errorResponse = new ErrorResponse(errorDetails);
                context.Result = new BadRequestObjectResult(errorResponse);

                var requestText = string.Empty;
                try
                {
                    var body = context.HttpContext.Request.Body;
                    body.Seek(0, SeekOrigin.Begin);
                    using var reader = new StreamReader(body);
                    requestText = await reader.ReadToEndAsync();
                    body.Seek(0, SeekOrigin.Begin);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while reading response body");
                }

                logger.LogError("An error occurred on model validation. Error response: {@ErrorResponse}, RequestData: {@RequestData}", errorResponse, requestText);

                return;
            }

            await next();

        }

        private static string FormValidationErrorTitle(string title) => $"Validation failed for field '{title}'";
    }
}
