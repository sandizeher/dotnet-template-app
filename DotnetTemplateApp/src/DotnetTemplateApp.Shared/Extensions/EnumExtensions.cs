namespace DotnetTemplateApp.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T>? GetFlags<T>(this Enum? input) where T : Enum
        {
            if (input == null) return null;
            return Enum.GetValues(input.GetType()).Cast<T>().Where(x => input.HasFlag(x));
        }
    }
}
