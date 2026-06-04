namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents delivery throttling options of an email campaign.
/// </summary>
public sealed record EmailCampaignDeliveryOptions
{
    /// <summary>
    /// Gets or sets the maximum number of emails to send per hour.
    /// </summary>
    ///
    /// <value>
    /// Maximum number of emails sent per hour, or <see langword="null"/> when not throttled.
    /// </value>
    [JsonPropertyName("emails_per_hour")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? EmailsPerHour { get; set; }
}
