namespace Mailtrap.Inbound.Responses;


/// <summary>
/// Response DTO that unwraps the list of forward rules from the <c>data</c> envelope.
/// </summary>
internal sealed record InboundForwardRuleListResponseDto
{
    [JsonPropertyName("data")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<InboundForwardRule> ForwardRules { get; } = [];
}
