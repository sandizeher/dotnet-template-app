using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    public class StringBoolConverter : JsonConverter<bool>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(bool);
        }

        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                return reader.GetBoolean();
            }

            bool.TryParse(reader.GetString(), out var value);
            return value;
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
