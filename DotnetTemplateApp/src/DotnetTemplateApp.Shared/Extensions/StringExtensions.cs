namespace DotnetTemplateApp.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

        public static string ToTitleCase(this string value) => value.IsNullOrEmpty() ? value : string.Concat(value[0].ToString().ToUpperInvariant(), value[1..].ToLowerInvariant());

        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new System.Text.StringBuilder();
            sb.Append(char.ToLower(input[0]));

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    sb.Append('_');
                    sb.Append(char.ToLower(input[i]));
                }
                else
                {
                    sb.Append(input[i]);
                }
            }

            return sb.ToString();
        }

        public static bool IsValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}
