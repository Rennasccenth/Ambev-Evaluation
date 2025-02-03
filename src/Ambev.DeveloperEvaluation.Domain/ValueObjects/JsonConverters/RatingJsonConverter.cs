using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public class RatingJsonConverter : JsonConverter<Rating>
{
    public override Rating Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        decimal rate = 0;
        int count = 0;

        Dictionary<string, string> propertyNameComparer;
        if (options.PropertyNamingPolicy is not null && options.PropertyNamingPolicy != JsonNamingPolicy.CamelCase)
        {
            propertyNameComparer = new Dictionary<string, string>
            {
                { "rate", "Rate" },
                { "count", "Count" }
            };
        }
        else
        {
            propertyNameComparer = new Dictionary<string, string>
            {
                { "Rate", "Rate" },
                { "Count", "Count" }
            };
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName) continue;
            var propertyName = reader.GetString()!;
            reader.Read();
                
            if (propertyNameComparer.TryGetValue(propertyName, out var standardizedProperty))
            {
                switch (standardizedProperty)
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
        
        string rateProperty = options.PropertyNamingPolicy?.ConvertName("Rate") ?? "Rate";
        string countProperty = options.PropertyNamingPolicy?.ConvertName("Count") ?? "Count";
        
        writer.WriteNumber(rateProperty, value.Rate);
        writer.WriteNumber(countProperty, value.Count);
        writer.WriteEndObject();
    }
}