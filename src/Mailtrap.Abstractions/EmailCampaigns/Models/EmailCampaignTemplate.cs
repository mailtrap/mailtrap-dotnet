namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents the template associated with an email campaign.
/// </summary>
public sealed record EmailCampaignTemplate
{
    /// <summary>
    /// Gets or sets the template identifier.
    /// </summary>
    ///
    /// <value>
    /// Template identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the email subject line.
    /// </summary>
    ///
    /// <value>
    /// Email subject line.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(2)]
    public string? Subject { get; set; }
}
