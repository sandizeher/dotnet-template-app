using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling;
using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Core.Common.Dtos
{
    [ExcludeFromCodeCoverage]
    public record GenericResponseDto : ErrorDetailsBase
    {
        public bool IsSuccess { get; init; }
    }
}
