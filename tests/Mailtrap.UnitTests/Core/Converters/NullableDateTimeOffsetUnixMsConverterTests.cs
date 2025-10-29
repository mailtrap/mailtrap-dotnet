using System.Globalization;

namespace Mailtrap.UnitTests.Core.Converters;

[TestFixture]
internal sealed class NullableDateTimeOffsetUnixMsConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new NullableDateTimeOffsetUnixMsConverter() }
    };

    [Test]
    public void Write_NullValue_SerializesAsNull()
    {
        // Arrange
        DateTimeOffset? value = null;

        // Act
        var json = JsonSerializer.Serialize(value, _options);

        // Assert
        json.Should().Be("null");
    }

    [Test]
    public void Write_ValidDate_SerializesToUnixMs()
    {
        // Arrange & Act
        var testDate = DateTimeOffset.Parse("2020-01-01T00:00:00Z", CultureInfo.InvariantCulture);
        var expectedMs = testDate.ToUnixTimeMilliseconds();
        var json = JsonSerializer.Serialize<DateTimeOffset?>(testDate, _options);
        // Assert
        json.Should().Be(expectedMs.ToString(CultureInfo.InvariantCulture));
    }

    [TestCase("\"\"")]
    [TestCase("\"  \"")]
    public void Read_ShouldReturnNull_WhenEmptyOrWhitespaceString(string json)
    {
        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Read_NullValue_DeserializesAsNull()
    {
        // Arrange
        var json = "null";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Read_ValidUnixMs_DeserializesToCorrectDate()
    {
        // Arrange
        var ms = DateTimeOffset.Parse("2020-01-01T00:00:00Z", CultureInfo.InvariantCulture).ToUnixTimeMilliseconds();
        var json = ms.ToString(CultureInfo.InvariantCulture);

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(ms));
    }

    [Test]
    public void Read_ShouldParse_IsoString()
    {
        // Arrange
        const string json = "\"2025-10-28T12:34:56Z\"";
        var expected = DateTimeOffset.Parse("2025-10-28T12:34:56Z", CultureInfo.InvariantCulture);

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Read_InvalidTokenType_ThrowsException()
    {
        // Arrange
        var json = "\"not a number\"";

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<DateTimeOffset?>(json, _options);
        act.Should().Throw<JsonException>().WithMessage("*Cannot convert value*");
    }
}
