using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public sealed class RatingJsonConverter : JsonConverter<Rating>
{
    public override Rating Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        decimal rate = 0;
        int count = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString()!;
                reader.Read();
                
                switch (propertyName)
                {
                    case "Rate":
                        rate = reader.GetDecimal();
                        break;
                    case "Count":
                        count = reader.GetInt32();
                        break;
                }
            }
        }

        return new Rating(rate, count);
    }

    public override void Write(Utf8JsonWriter writer, Rating value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Rate", value.Rate);
        writer.WriteNumber("Count", value.Count);
        writer.WriteEndObject();
    }
}