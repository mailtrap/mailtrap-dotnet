namespace Mailtrap.Webhooks.Responses;


/// <summary>
/// Response DTO that unwraps the created webhook (including <c>signing_secret</c>) from the <c>data</c> envelope.
/// </summary>
internal sealed record CreateWebhookResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public CreateWebhookResponse Webhook { get; } = new();
}
