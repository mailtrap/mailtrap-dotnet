namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Request object for updating an email campaign.<br/>
/// All properties are optional - only properties set on the request are sent and changed.
/// </summary>
public sealed record UpdateEmailCampaignRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the campaign name.
    /// </summary>
    ///
    /// <value>
    /// Campaign name, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the verified sending domain used for the campaign.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("mailsend_domain_id")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? MailsendDomainId { get; set; }

    /// <summary>
    /// Gets or sets the display name shown in the From header.
    /// </summary>
    ///
    /// <value>
    /// From display name, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("from_display_name")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the local part (before the @) of the From address.
    /// </summary>
    ///
    /// <value>
    /// From address local part, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("from_local_part")]
    [JsonPropertyOrder(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromLocalPart { get; set; }

    /// <summary>
    /// Gets or sets how the campaign is delivered.
    /// </summary>
    ///
    /// <value>
    /// Delivery mode (<see cref="DeliveryMode.Immediate"/> or <see cref="DeliveryMode.Scheduled"/>),
    /// or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("delivery_mode")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeliveryMode? DeliveryMode { get; set; }

    /// <summary>
    /// Gets or sets the date and time to send the campaign.<br/>
    /// Required when <see cref="DeliveryMode"/> is <see cref="DeliveryMode.Scheduled"/>.
    /// </summary>
    ///
    /// <value>
    /// Scheduled send time, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("scheduled_for")]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? ScheduledFor { get; set; }

    /// <summary>
    /// Gets or sets the delivery throttling options.
    /// </summary>
    ///
    /// <value>
    /// Delivery throttling options, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("delivery_options")]
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmailCampaignDeliveryOptions? DeliveryOptions { get; set; }

    /// <summary>
    /// Gets or sets the Reply-To address parts.
    /// </summary>
    ///
    /// <value>
    /// Reply-To address parts, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(8)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReplyTo? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the template attributes.<br/>
    /// Provide an existing <see cref="EmailCampaignTemplateAttributes.Id"/> to update the template in place
    /// instead of creating a new one.
    /// </summary>
    ///
    /// <value>
    /// Template attributes, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("template_attributes")]
    [JsonPropertyOrder(9)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmailCampaignTemplateAttributes? TemplateAttributes { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return UpdateEmailCampaignRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
