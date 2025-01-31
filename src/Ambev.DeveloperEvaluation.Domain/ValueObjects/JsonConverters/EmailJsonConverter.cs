using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public class EmailJsonConverter : JsonConverter<Email>
{
    public override Email? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value == null ? null : (Email)value;
    }

    public override void Write(Utf8JsonWriter writer, Email? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}