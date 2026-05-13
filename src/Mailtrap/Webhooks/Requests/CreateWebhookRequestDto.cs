namespace Mailtrap.Webhooks.Requests;


/// <summary>
/// Request DTO that wraps <see cref="CreateWebhookRequest"/> in the <c>webhook</c> envelope expected by the API.
/// </summary>
internal sealed record CreateWebhookRequestDto : IValidatable
{
    [JsonPropertyName("webhook")]
    [JsonPropertyOrder(1)]
    public CreateWebhookRequest Webhook { get; }


    public CreateWebhookRequestDto(CreateWebhookRequest webhook)
    {
        Ensure.NotNull(webhook, nameof(webhook));

        Webhook = webhook;
    }


    public ValidationResult Validate() => Webhook.Validate();
}
