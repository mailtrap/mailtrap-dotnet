namespace Mailtrap.Webhooks.Responses;


/// <summary>
/// Response DTO that unwraps the list of webhooks from the <c>data</c> envelope.
/// </summary>
internal sealed record WebhookListResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<Webhook> Webhooks { get; } = [];
}
