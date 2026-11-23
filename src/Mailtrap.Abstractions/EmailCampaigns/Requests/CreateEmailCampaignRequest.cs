namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Request object for creating an email campaign.<br/>
/// The request body is sent flat (no envelope). The campaign is always created in the
/// <see cref="CampaignState.Draft"/> state - scheduling and starting are separate actions.
/// </summary>
public sealed record CreateEmailCampaignRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the campaign name. Required.
    /// </summary>
    ///
    /// <value>
    /// Campaign name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the verified sending domain used for the campaign,
    /// as returned by the Sending Domains endpoints. Required.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public long? DomainId { get; set; }

    /// <summary>
    /// Gets or sets the display name shown in the From header.
    /// </summary>
    ///
    /// <value>
    /// From display name, or <see langword="null"/> to use the API default.
    /// </value>
    [JsonPropertyName("from_display_name")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the local part (before the @) of the From address. Required.
    /// </summary>
    ///
    /// <value>
    /// From address local part.
    /// </value>
    [JsonPropertyName("from_local_part")]
    [JsonPropertyOrder(4)]
    [JsonRequired]
    public string? FromLocalPart { get; set; }

    /// <summary>
    /// Gets or sets the Reply-To address parts.
    /// </summary>
    ///
    /// <value>
    /// Reply-To address parts, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReplyTo? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the template attributes - the campaign's subject and design.
    /// Required, with a non-empty <see cref="EmailCampaignTemplateAttributes.Subject"/>.
    /// </summary>
    ///
    /// <value>
    /// Template attributes.
    /// </value>
    [JsonPropertyName("template_attributes")]
    [JsonPropertyOrder(6)]
    [JsonRequired]
    public EmailCampaignTemplateAttributes? TemplateAttributes { get; set; }

    /// <summary>
    /// Gets or sets how the campaign is delivered.
    /// </summary>
    ///
    /// <value>
    /// Delivery mode (<see cref="DeliveryMode.Rapid"/> or <see cref="DeliveryMode.Gradual"/>),
    /// or <see langword="null"/> to use the API default.
    /// </value>
    [JsonPropertyName("delivery_mode")]
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeliveryMode? DeliveryMode { get; set; }

    /// <summary>
    /// Gets or sets the delivery throttling options.
    /// Applies when <see cref="DeliveryMode"/> is <see cref="DeliveryMode.Gradual"/>.
    /// </summary>
    ///
    /// <value>
    /// Delivery throttling options, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("delivery_options")]
    [JsonPropertyOrder(8)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmailCampaignDeliveryOptions? DeliveryOptions { get; set; }

    /// <summary>
    /// Gets or sets the identifiers of the contact lists to send to.<br/>
    /// Treated as the full set of included lists.
    /// Combine with <see cref="ContactSegmentIds"/> to target both.
    /// </summary>
    ///
    /// <value>
    /// Contact list identifiers, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("contact_list_ids")]
    [JsonPropertyOrder(9)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<long>? ContactListIds { get; set; }

    /// <summary>
    /// Gets or sets the identifiers of the contact segments to send to.<br/>
    /// Treated as the full set of included segments.
    /// </summary>
    ///
    /// <value>
    /// Contact segment identifiers, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("contact_segment_ids")]
    [JsonPropertyOrder(10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<long>? ContactSegmentIds { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateEmailCampaignRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
