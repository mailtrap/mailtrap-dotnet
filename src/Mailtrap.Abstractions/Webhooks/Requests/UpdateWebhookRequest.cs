namespace Mailtrap.Webhooks.Requests;


/// <summary>
/// Request object for updating a webhook. Only properties set on the request are sent.
/// </summary>
public sealed record UpdateWebhookRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the URL that will receive webhook payloads.
    /// </summary>
    ///
    /// <value>
    /// Target URL or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("url")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Uri? Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the webhook is active.
    /// </summary>
    ///
    /// <value>
    /// Active flag or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("active")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or sets the format of the webhook payload.
    /// </summary>
    ///
    /// <value>
    /// Webhook payload format or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("payload_format")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WebhookPayloadFormat? PayloadFormat { get; set; }

    /// <summary>
    /// Gets or sets the list of event types the webhook subscribes to.<br/>
    /// Applicable only for <c>email_sending</c> webhooks.<br/>
    /// Leave <see langword="null"/> to keep the existing event types unchanged.
    /// </summary>
    ///
    /// <value>
    /// List of event types or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("event_types")]
    [JsonPropertyOrder(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<WebhookEventType>? EventTypes { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return UpdateWebhookRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
