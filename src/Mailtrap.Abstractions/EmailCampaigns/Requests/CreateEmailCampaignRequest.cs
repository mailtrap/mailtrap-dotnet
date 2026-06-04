namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Request object for creating an email campaign.
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
    /// Gets or sets the identifier of the verified sending domain used for the campaign. Required.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier.
    /// </value>
    [JsonPropertyName("mailsend_domain_id")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public long MailsendDomainId { get; set; }

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
    /// Gets or sets the local part (before the @) of the From address.
    /// </summary>
    ///
    /// <value>
    /// From address local part, or <see langword="null"/> to use the API default.
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
    /// Reply-To address parts, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReplyTo? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the template attributes.<br/>
    /// Provide a <see cref="EmailCampaignTemplateAttributes.Subject"/> to set the campaign subject inline.
    /// </summary>
    ///
    /// <value>
    /// Template attributes, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("template_attributes")]
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EmailCampaignTemplateAttributes? TemplateAttributes { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateEmailCampaignRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
