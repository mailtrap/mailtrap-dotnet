namespace Mailtrap.Core.Converters;

/// <summary>
/// JSON converter for nullable <see cref="DateTimeOffset"/>.
/// Supports Unix time in milliseconds.
/// Empty or whitespace strings are treated as <see langword="null"/>.
/// </summary>
internal sealed class NullableDateTimeOffsetUnixMsConverter : JsonConverter<DateTimeOffset?>
{
    private readonly NullableDateTimeOffsetConverter _nullableDateTimeOffsetConverter = new();

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => _nullableDateTimeOffsetConverter.Read(ref reader, typeToConvert, options);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        Ensure.NotNull(writer, nameof(writer));

        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value.ToUnixTimeMilliseconds());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
