namespace Mailtrap.Templates.Responses;


/// <summary>
/// Response DTO that unwraps a single template from the <c>data</c> envelope.
/// </summary>
internal sealed record TemplateResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public Template Template { get; } = new();
}
