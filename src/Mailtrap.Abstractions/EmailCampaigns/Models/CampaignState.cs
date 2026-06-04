namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Current state of an email campaign in its lifecycle.
/// </summary>
public sealed record CampaignState : StringEnum<CampaignState>
{
    /// <summary>
    /// Gets the value representing the "draft" campaign state.
    /// </summary>
    ///
    /// <value>
    /// Represents the "draft" campaign state.
    /// </value>
    public static readonly CampaignState Draft = Define("draft");

    /// <summary>
    /// Gets the value representing the "scheduled" campaign state.
    /// </summary>
    ///
    /// <value>
    /// Represents the "scheduled" campaign state.
    /// </value>
    public static readonly CampaignState Scheduled = Define("scheduled");

    /// <summary>
    /// Gets the value representing the "sending" campaign state.
    /// </summary>
    ///
    /// <value>
    /// Represents the "sending" campaign state.
    /// </value>
    public static readonly CampaignState Sending = Define("sending");

    /// <summary>
    /// Gets the value representing the "sent" campaign state.
    /// </summary>
    ///
    /// <value>
    /// Represents the "sent" campaign state.
    /// </value>
    public static readonly CampaignState Sent = Define("sent");

    /// <summary>
    /// Gets the value representing the "terminated" campaign state.
    /// </summary>
    ///
    /// <value>
    /// Represents the "terminated" campaign state.
    /// </value>
    public static readonly CampaignState Terminated = Define("terminated");
}
