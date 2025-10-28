namespace Mailtrap.Contacts.Converters;

/// <summary>
/// Converts DateTimeOffset to Unix time milliseconds for JSON serialization.
/// Handles empty strings as null values.
/// </summary>
internal sealed class DateTimeToUnixMsNullableJsonConverter : JsonConverter<DateTimeOffset?>
{
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            if (long.TryParse(stringValue, out var msFromString))
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(msFromString);
            }

            throw new JsonException($"Expected number for Unix time milliseconds but got string.");
        }

        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException($"Expected number for Unix time milliseconds but got {reader.TokenType}.");
        }

        var ms = reader.GetInt64();
        return DateTimeOffset.FromUnixTimeMilliseconds(ms);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNumberValue(value.Value.ToUnixTimeMilliseconds());
    }
}
