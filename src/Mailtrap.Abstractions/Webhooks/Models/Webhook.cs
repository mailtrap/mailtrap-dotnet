namespace Mailtrap.Webhooks.Models;


/// <summary>
/// Represents webhook details.
/// </summary>
public record Webhook
{
    /// <summary>
    /// Gets or sets webhook identifier.
    /// </summary>
    ///
    /// <value>
    /// Webhook identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the URL that will receive webhook payloads.
    /// </summary>
    ///
    /// <value>
    /// The URL that will receive webhook payloads.
    /// </value>
    [JsonPropertyName("url")]
    [JsonPropertyOrder(2)]
    public Uri? Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the webhook is active.
    /// </summary>
    ///
    /// <value>
    /// <see langword="true"/> when the webhook is active; <see langword="false"/> otherwise.
    /// </value>
    [JsonPropertyName("active")]
    [JsonPropertyOrder(3)]
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets webhook type.
    /// </summary>
    ///
    /// <value>
    /// Webhook type. Allowed values: "email_sending", "audit_log".
    /// </value>
    [JsonPropertyName("webhook_type")]
    [JsonPropertyOrder(4)]
    public WebhookType WebhookType { get; set; } = WebhookType.Unknown;

    /// <summary>
    /// Gets or sets the format of the webhook payload.
    /// </summary>
    ///
    /// <value>
    /// Webhook payload format. Allowed values: "json", "jsonlines".
    /// </value>
    [JsonPropertyName("payload_format")]
    [JsonPropertyOrder(5)]
    public WebhookPayloadFormat PayloadFormat { get; set; } = WebhookPayloadFormat.Unknown;

    /// <summary>
    /// Gets or sets sending stream the webhook is subscribed to.<br/>
    /// Applicable only for <c>email_sending</c> webhooks.
    /// </summary>
    ///
    /// <value>
    /// Sending stream or <see langword="null"/> when not applicable.
    /// </value>
    [JsonPropertyName("sending_stream")]
    [JsonPropertyOrder(6)]
    public WebhookSendingStream? SendingStream { get; set; }

    /// <summary>
    /// Gets or sets the domain ID the webhook is scoped to.<br/>
    /// Applicable only for <c>email_sending</c> webhooks; <see langword="null"/> means all domains.
    /// </summary>
    ///
    /// <value>
    /// Domain identifier or <see langword="null"/> when scoped to all domains.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(7)]
    public long? DomainId { get; set; }

    /// <summary>
    /// Gets or sets the list of event types the webhook is subscribed to.<br/>
    /// Applicable only for <c>email_sending</c> webhooks.
    /// </summary>
    ///
    /// <value>
    /// List of event types.
    /// </value>
    [JsonPropertyName("event_types")]
    [JsonPropertyOrder(8)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<WebhookEventType> EventTypes { get; } = [];
}
