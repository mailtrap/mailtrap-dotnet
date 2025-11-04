using System.Globalization;

namespace Mailtrap.Core.Converters;

/// <summary>
/// JSON converter for nullable <see cref="DateTimeOffset"/>.
/// Supports reading both ISO 8601 string format and Unix time in milliseconds.
/// Writes only ISO 8601 string format. To write Unix time in milliseconds please use <see cref="NullableDateTimeOffsetUnixMsConverter"/>
/// Empty or whitespace strings are treated as <see langword="null"/>.
/// </summary>
public sealed class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    /// <inheritdoc />
    /// <exception cref="JsonException">
    /// Thrown when the JSON value cannot be parsed as a valid <see cref="DateTimeOffset"/>.
    /// </exception>
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => ParseString(reader.GetString()),
            JsonTokenType.Number => ParseNumber(ref reader),
            _ => throw new JsonException($"Unexpected JSON token {reader.TokenType} for {nameof(DateTimeOffset)}.")
        };
    }

    /// <summary>
    /// Parses a string value into a <see cref="DateTimeOffset"/> if possible.
    /// </summary>
    /// <param name="value">The input string value read from the JSON token.</param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> parsed from the input value, or <see langword="null"/> if the string is <see langword="null"/>,
    /// empty, or consists only of whitespace.
    /// </returns>
    /// <exception cref="JsonException">
    /// Thrown when the string cannot be parsed as a valid ISO 8601 date/time or Unix milliseconds timestamp.
    /// </exception>
    private static DateTimeOffset? ParseString(string? value)
    {
        var s = value?.Trim();
        if (string.IsNullOrEmpty(s))
        {
            return null;
        }

        if (DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }

        return long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unixMs)
            ? (DateTimeOffset?)DateTimeOffset.FromUnixTimeMilliseconds(unixMs)
            : throw new JsonException($"Cannot convert value '{s}' to {nameof(DateTimeOffset)}.");
    }

    /// <summary>
    /// Parses a numeric JSON token as a Unix timestamp in milliseconds.
    /// </summary>
    /// <param name="reader">
    /// The <see cref="Utf8JsonReader"/> positioned at a numeric JSON token.
    /// </param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the Unix time in milliseconds
    /// specified by the numeric value.
    /// </returns>
    /// <exception cref="JsonException">
    /// Thrown if the numeric token cannot be converted to a valid 64-bit integer.
    /// </exception>
    private static DateTimeOffset ParseNumber(ref Utf8JsonReader reader)
    {
        return reader.TryGetInt64(out var unixMs)
            ? DateTimeOffset.FromUnixTimeMilliseconds(unixMs)
            : throw new JsonException("Invalid numeric value for Unix milliseconds epoch.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        Ensure.NotNull(writer, nameof(writer));

        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
