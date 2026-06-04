namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// How an email campaign is delivered.
/// </summary>
public sealed record DeliveryMode : StringEnum<DeliveryMode>
{
    /// <summary>
    /// Gets the value representing "immediate" delivery mode.
    /// </summary>
    ///
    /// <value>
    /// Represents "immediate" delivery mode - the campaign is sent right away.
    /// </value>
    public static readonly DeliveryMode Immediate = Define("immediate");

    /// <summary>
    /// Gets the value representing "scheduled" delivery mode.
    /// </summary>
    ///
    /// <value>
    /// Represents "scheduled" delivery mode - the campaign is sent at a scheduled time.
    /// </value>
    public static readonly DeliveryMode Scheduled = Define("scheduled");
}
