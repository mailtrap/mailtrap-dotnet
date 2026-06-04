namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents metadata about the most recent state transition of an email campaign.
/// </summary>
public sealed record EmailCampaignStateMetadata
{
    /// <summary>
    /// Gets or sets the reason for the most recent state transition.
    /// </summary>
    ///
    /// <value>
    /// State transition reason.
    /// </value>
    [JsonPropertyName("reason")]
    [JsonPropertyOrder(1)]
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the error associated with the most recent state transition.
    /// </summary>
    ///
    /// <value>
    /// State transition error.
    /// </value>
    [JsonPropertyName("error")]
    [JsonPropertyOrder(2)]
    public string? Error { get; set; }

    /// <summary>
    /// Gets the collection of errors associated with the most recent state transition.
    /// </summary>
    ///
    /// <value>
    /// Collection of state transition errors.
    /// </value>
    [JsonPropertyName("errors")]
    [JsonPropertyOrder(3)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> Errors { get; } = [];

    /// <summary>
    /// Gets or sets the time the campaign is scheduled at.
    /// </summary>
    ///
    /// <value>
    /// Scheduled time, or <see langword="null"/> when not scheduled.
    /// </value>
    [JsonPropertyName("scheduled_at")]
    [JsonPropertyOrder(4)]
    public DateTimeOffset? ScheduledAt { get; set; }
}
