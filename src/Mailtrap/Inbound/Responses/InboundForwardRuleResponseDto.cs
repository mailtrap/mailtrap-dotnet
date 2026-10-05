namespace Mailtrap.Inbound.Responses;


/// <summary>
/// Response DTO that unwraps a single forward rule from the <c>data</c> envelope.
/// </summary>
internal sealed record InboundForwardRuleResponseDto
{
    [JsonPropertyName("data")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public InboundForwardRule ForwardRule { get; } = new();
}
