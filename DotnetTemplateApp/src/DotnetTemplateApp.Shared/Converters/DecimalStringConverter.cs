using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    public class DecimalStringConverter : JsonConverter<decimal?>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(decimal?);
        }

        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }

            decimal.TryParse(reader.GetString(), CultureInfo.InvariantCulture, out var value);
            return value;
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
        }
    }
}
