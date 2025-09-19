using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Api.Filters.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorModel
    {
        public string? FieldName { get; set; }
        public string? Message { get; set; }
    }
}
