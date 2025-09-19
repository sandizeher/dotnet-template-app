using System.Diagnostics.CodeAnalysis;
using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling;

namespace DotnetTemplateApp.Api.Filters.Models
{
    [ExcludeFromCodeCoverage]
    public record ErrorResponse(ICollection<ErrorDetails> ErrorDetails);
}
