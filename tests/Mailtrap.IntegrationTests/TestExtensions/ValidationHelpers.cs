namespace Mailtrap.IntegrationTests.TestExtensions;

/// <summary>
/// Provides validation helper methods for integration tests.
/// </summary>
internal static class ValidationHelpers
{
    /// <summary>
    /// Deserializes the string content to the specified type using the provided JSON serializer options.
    /// </summary>
    /// <typeparam name="TValue">The type to deserialize to.</typeparam>
    /// <param name="responseContent">The string content to deserialize.</param>
    /// <param name="jsonSerializerOptions">The JSON serializer options.</param>
    /// <returns>The deserialized object of type <typeparamref name="TValue"/>.</returns>
    internal static async Task<TValue?> DeserializeStringContentAsync<TValue>(this StringContent responseContent, JsonSerializerOptions jsonSerializerOptions)
        where TValue : class
    {
        var responseStream = await responseContent.ReadAsStreamAsync();
        var expectedResponse = await JsonSerializer.DeserializeAsync<TValue>(responseStream, jsonSerializerOptions);
        if (responseStream.CanSeek)
        {
            responseStream.Position = 0; // Reset stream position
        }

        return expectedResponse;
    }

    /// <summary>
    /// Compares two objects of the same type, handling JsonElement properties correctly.
    /// </summary>
    /// <typeparam name="TValue">
    /// Supposed to be <see cref="ContactResponse"/> and derived classes.
    /// However, can be used for majority request/response classes which can contain <see cref="JsonElement"/> after serialization,
    /// e.g. contains properties of type <see cref="object"/> or any sort of collection with <see cref="object"/> inside.
    /// </typeparam>
    /// <param name="result">Object with actual result</param>
    /// <param name="expected">Object with expected results</param>
    internal static void ShouldBeEquivalentToResponse<TValue>(this TValue result, TValue expected)
        where TValue : class
    {
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expected, options => options
            // Convert JsonElement to string before comparison
            // this should allow to correctly compare Dictionary<string, object> like Contact.Fields
            .Using<JsonElement>(ctx =>
            {
                var expected = ctx.Expectation.ToString();
                var actual = ctx.Subject.ToString();
                actual.Should().Be(expected);
            }).WhenTypeIs<JsonElement>());
    }
}
