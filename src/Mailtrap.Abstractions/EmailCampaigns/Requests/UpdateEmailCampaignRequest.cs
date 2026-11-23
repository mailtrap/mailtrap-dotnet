namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Request object for updating an email campaign.<br/>
/// The request body is sent flat (no envelope). All properties are optional -
/// only properties set on the request are sent and changed.
/// Only <see cref="CampaignState.Draft"/> campaigns can be updated.
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
    /// Gets or sets the identifier of the verified sending domain used for the campaign,
    /// as returned by the Sending Domains endpoints.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? DomainId { get; set; }

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
    /// Gets or sets the Reply-To address parts.
    /// </summary>
    ///
    /// <value>
    /// Reply-To address parts, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReplyTo? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the template attributes - the campaign's subject and design.<br/>
    /// The campaign's template is always edited in place; only the provided sub-fields change.
    /// </summary>
    ///
    /// <value>
    /// Template attributes, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("template_attributes")]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmailCampaignTemplateAttributes? TemplateAttributes { get; set; }

    /// <summary>
    /// Gets or sets how the campaign is delivered.
    /// </summary>
    ///
    /// <value>
    /// Delivery mode (<see cref="DeliveryMode.Rapid"/> or <see cref="DeliveryMode.Gradual"/>),
    /// or <see langword="null"/> to leave unchanged.
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
    /// Delivery throttling options, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("delivery_options")]
    [JsonPropertyOrder(8)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmailCampaignDeliveryOptions? DeliveryOptions { get; set; }

    /// <summary>
    /// Gets or sets the identifiers of the contact lists to send to.<br/>
    /// Treated as the full set of included lists - lists not listed are removed.
    /// Combine with <see cref="ContactSegmentIds"/> to target both.
    /// </summary>
    ///
    /// <value>
    /// Contact list identifiers, or <see langword="null"/> to leave unchanged.
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
    /// Contact segment identifiers, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("contact_segment_ids")]
    [JsonPropertyOrder(10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<long>? ContactSegmentIds { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return UpdateEmailCampaignRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
