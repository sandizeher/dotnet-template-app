using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    /// <summary>
    /// Convert a long value to a string and vice versa.
    /// </summary>
    public class JsonLongStringConverter : JsonConverter<string>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(string);
        }

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long longValue))
                {
                    return longValue.ToString();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    return reader.GetString() ?? string.Empty;
                }
                else
                {
                    throw new JsonException($"Unexpected token type {reader.TokenType} when parsing a string.");
                }
            }
            catch (Exception ex)
            {
                throw new JsonException($"Error converting value to string.", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
