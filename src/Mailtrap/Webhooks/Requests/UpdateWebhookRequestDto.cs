namespace Mailtrap.Webhooks.Requests;


/// <summary>
/// Request DTO that wraps <see cref="UpdateWebhookRequest"/> in the <c>webhook</c> envelope expected by the API.
/// </summary>
internal sealed record UpdateWebhookRequestDto : IValidatable
{
    [JsonPropertyName("webhook")]
    [JsonPropertyOrder(1)]
    public UpdateWebhookRequest Webhook { get; }


    public UpdateWebhookRequestDto(UpdateWebhookRequest webhook)
    {
        Ensure.NotNull(webhook, nameof(webhook));

        Webhook = webhook;
    }


    public ValidationResult Validate() => Webhook.Validate();
}
