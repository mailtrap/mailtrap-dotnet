using System.Globalization;

namespace Mailtrap.UnitTests.Core.Converters;


[TestFixture]
internal sealed class NullableDateTimeOffsetConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new NullableDateTimeOffsetConverter() }
    };


    [Test]
    [TestCase("\"\"", null)]
    [TestCase("\"   \"", null)]
    public void Read_ShouldReturnNull_WhenEmptyOrWhitespaceString(string json, DateTimeOffset? expected)
    {
        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Read_ShouldParseValidDateString_WithSurroundingWhitespace()
    {
        // Arrange
        var expected = DateTimeOffset.Parse("2025-10-28T12:34:56Z", CultureInfo.InvariantCulture);
        var json = "\" 2025-10-28T12:34:56Z \"";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Read_ShouldParseUnixMilliseconds_WhenNumericToken()
    {
        // Arrange
        var unixMs = 1698499200000L; // corresponds to 2023-10-28T00:00:00Z
        var expected = DateTimeOffset.FromUnixTimeMilliseconds(unixMs);
        var json = unixMs.ToString(CultureInfo.InvariantCulture);

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Read_ShouldParseUnixMilliseconds_WhenStringToken()
    {
        // Arrange
        var json = "\"1698499200000\""; // corresponds to 2023-10-28T00:00:00Z
        var expected = DateTimeOffset.FromUnixTimeMilliseconds(1698499200000);

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Read_ShouldThrowJsonException_ForInvalidString()
    {
        // Arrange
        var json = "\"not-a-date\"";

        // Act
        var act = () => JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        act.Should().Throw<JsonException>()
            .WithMessage("*Cannot convert value*DateTimeOffset*");
    }

    [Test]
    public void Read_ShouldThrowJsonException_ForInvalidNumber()
    {
        // Arrange
        var json = "123.123";

        // Act
        var act = () => JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        act.Should().Throw<JsonException>()
            .WithMessage("*Invalid numeric value for Unix milliseconds epoch.*");
    }

    [Test]
    public void Read_ShouldReturnNull_ForJsonNull()
    {
        // Arrange
        const string json = "null";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Read_ShouldReturnNull_ForJsonNullElement()
    {
        // Arrange
        const string json = "null";
        var test = JsonSerializer.Serialize<object>(json, _options);

        // Act
        var act = () => JsonSerializer.Deserialize<DateTimeOffset?>(test, _options);

        // Assert
        act.Should().Throw<JsonException>()
            .WithMessage("*Cannot convert value*to DateTimeOffset*");
    }

    [Test]
    public void Write_ShouldWriteIsoString_WhenConfiguredAsIso()
    {
        // Arrange
        var value = DateTimeOffset.Parse("2025-10-28T12:34:56Z", CultureInfo.InvariantCulture);

        // Act
        var json = JsonSerializer.Serialize<DateTimeOffset?>(value, _options);

        // Assert
        json.Should().Be("\"2025-10-28T12:34:56+00:00\"");
    }

    [Test]
    public void Write_ShouldSerializeNullValue()
    {
        // Arrange
        DateTimeOffset? value = null;

        // Act
        var json = JsonSerializer.Serialize(value, _options);

        // Assert
        json.Should().Be("null");
    }
}
