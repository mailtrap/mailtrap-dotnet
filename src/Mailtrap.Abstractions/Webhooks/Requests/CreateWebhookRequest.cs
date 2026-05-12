namespace Mailtrap.Webhooks.Requests;


/// <summary>
/// Request object for creating a webhook.
/// </summary>
public sealed record CreateWebhookRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the URL that will receive webhook payloads.
    /// </summary>
    ///
    /// <value>
    /// Target URL.
    /// </value>
    [JsonPropertyName("url")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public Uri? Url { get; set; }

    /// <summary>
    /// Gets or sets webhook type.
    /// </summary>
    ///
    /// <value>
    /// Webhook type. Allowed values: "email_sending", "audit_log".
    /// </value>
    [JsonPropertyName("webhook_type")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public WebhookType WebhookType { get; set; } = WebhookType.Unknown;

    /// <summary>
    /// Gets or sets a value indicating whether the webhook is active. Defaults to <see langword="true"/>.
    /// </summary>
    ///
    /// <value>
    /// Active flag or <see langword="null"/> to use the API default.
    /// </value>
    [JsonPropertyName("active")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or sets the format of the webhook payload.
    /// </summary>
    ///
    /// <value>
    /// Webhook payload format or <see langword="null"/> to use the API default ("json").
    /// </value>
    [JsonPropertyName("payload_format")]
    [JsonPropertyOrder(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WebhookPayloadFormat? PayloadFormat { get; set; }

    /// <summary>
    /// Gets or sets sending stream the webhook subscribes to.<br/>
    /// Required for <c>email_sending</c> webhook type.
    /// </summary>
    ///
    /// <value>
    /// Sending stream or <see langword="null"/>.
    /// </value>
    [JsonPropertyName("sending_stream")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SendingStream? SendingStream { get; set; }

    /// <summary>
    /// Gets the list of event types to subscribe to.<br/>
    /// Required for <c>email_sending</c> webhook type.
    /// </summary>
    ///
    /// <value>
    /// List of event types.
    /// </value>
    [JsonPropertyName("event_types")]
    [JsonPropertyOrder(6)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<WebhookEventType> EventTypes { get; } = [];

    /// <summary>
    /// Gets or sets the domain ID to scope the webhook to.<br/>
    /// When omitted, the webhook applies to all domains.<br/>
    /// Applicable only for <c>email_sending</c> webhooks.
    /// </summary>
    ///
    /// <value>
    /// Domain identifier or <see langword="null"/>.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? DomainId { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateWebhookRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
