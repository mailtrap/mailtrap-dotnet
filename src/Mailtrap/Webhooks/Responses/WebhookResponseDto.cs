namespace Mailtrap.Webhooks.Responses;


/// <summary>
/// Response DTO that unwraps a single webhook from the <c>data</c> envelope.
/// </summary>
internal sealed record WebhookResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public Webhook Webhook { get; } = new();
}
