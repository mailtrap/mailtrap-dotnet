namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Represents the template attributes of an email campaign create/update request.
/// </summary>
public sealed record EmailCampaignTemplateAttributes
{
    /// <summary>
    /// Gets or sets the identifier of an existing template to update in place
    /// instead of creating a new one.<br/>
    /// Applicable only when updating a campaign.
    /// </summary>
    ///
    /// <value>
    /// Existing template identifier, or <see langword="null"/> to create a new template.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or sets the email subject line.
    /// </summary>
    ///
    /// <value>
    /// Email subject line, or <see langword="null"/> to omit.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subject { get; set; }
}
