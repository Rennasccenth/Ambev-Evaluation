using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public class PasswordJsonConverter : JsonConverter<Password>
{
    public override Password? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value == null ? null : (Password)value;
    }

    public override void Write(Utf8JsonWriter writer, Password? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}