namespace Mailtrap.Marketing.Responses;


/// <summary>
/// Response DTO that unwraps Email Marketing statistics from the <c>data</c> envelope.
/// </summary>
internal sealed record MarketingStatsResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public MarketingStats Stats { get; } = new();
}
