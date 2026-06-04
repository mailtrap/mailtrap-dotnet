namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents aggregated performance metrics for an email campaign.<br/>
/// All counts and rates are <c>0</c> when the campaign has not been started.<br/>
/// The same shape is returned both inline on an <see cref="EmailCampaign"/>
/// (its <see cref="EmailCampaign.Stats"/> member) and as the standalone statistics response.
/// </summary>
public sealed record EmailCampaignStats
{
    /// <summary>
    /// Gets or sets the number of delivered messages.
    /// </summary>
    ///
    /// <value>
    /// Number of delivered messages.
    /// </value>
    [JsonPropertyName("delivery_count")]
    [JsonPropertyOrder(1)]
    public int DeliveryCount { get; set; }

    /// <summary>
    /// Gets or sets the number of opened messages.
    /// </summary>
    ///
    /// <value>
    /// Number of opened messages.
    /// </value>
    [JsonPropertyName("open_count")]
    [JsonPropertyOrder(2)]
    public int OpenCount { get; set; }

    /// <summary>
    /// Gets or sets the number of clicked messages.
    /// </summary>
    ///
    /// <value>
    /// Number of clicked messages.
    /// </value>
    [JsonPropertyName("click_count")]
    [JsonPropertyOrder(3)]
    public int ClickCount { get; set; }

    /// <summary>
    /// Gets or sets the number of bounced messages.
    /// </summary>
    ///
    /// <value>
    /// Number of bounced messages.
    /// </value>
    [JsonPropertyName("bounce_count")]
    [JsonPropertyOrder(4)]
    public int BounceCount { get; set; }

    /// <summary>
    /// Gets or sets the number of unsubscriptions.
    /// </summary>
    ///
    /// <value>
    /// Number of unsubscriptions.
    /// </value>
    [JsonPropertyName("unsubscription_count")]
    [JsonPropertyOrder(5)]
    public int UnsubscriptionCount { get; set; }

    /// <summary>
    /// Gets or sets the number of sent messages.
    /// </summary>
    ///
    /// <value>
    /// Number of sent messages.
    /// </value>
    [JsonPropertyName("sent_count")]
    [JsonPropertyOrder(6)]
    public int SentCount { get; set; }

    /// <summary>
    /// Gets or sets the number of spam complaints.
    /// </summary>
    ///
    /// <value>
    /// Number of spam complaints.
    /// </value>
    [JsonPropertyName("spam_count")]
    [JsonPropertyOrder(7)]
    public int SpamCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of messages.
    /// </summary>
    ///
    /// <value>
    /// Total number of messages.
    /// </value>
    [JsonPropertyName("message_count")]
    [JsonPropertyOrder(8)]
    public int MessageCount { get; set; }

    /// <summary>
    /// Gets or sets the number of rejected messages.
    /// </summary>
    ///
    /// <value>
    /// Number of rejected messages.
    /// </value>
    [JsonPropertyName("reject_count")]
    [JsonPropertyOrder(9)]
    public int RejectCount { get; set; }

    /// <summary>
    /// Gets or sets the share of sent messages that were delivered (0–1).
    /// </summary>
    ///
    /// <value>
    /// Delivery rate.
    /// </value>
    [JsonPropertyName("delivery_rate")]
    [JsonPropertyOrder(10)]
    public float DeliveryRate { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages that were opened (0–1).
    /// </summary>
    ///
    /// <value>
    /// Open rate.
    /// </value>
    [JsonPropertyName("open_rate")]
    [JsonPropertyOrder(11)]
    public float OpenRate { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages that were clicked (0–1).
    /// </summary>
    ///
    /// <value>
    /// Click rate.
    /// </value>
    [JsonPropertyName("click_rate")]
    [JsonPropertyOrder(12)]
    public float ClickRate { get; set; }

    /// <summary>
    /// Gets or sets the share of sent messages that bounced (0–1).
    /// </summary>
    ///
    /// <value>
    /// Bounce rate.
    /// </value>
    [JsonPropertyName("bounce_rate")]
    [JsonPropertyOrder(13)]
    public float BounceRate { get; set; }

    /// <summary>
    /// Gets or sets the share of sent messages marked as spam (0–1).
    /// </summary>
    ///
    /// <value>
    /// Spam rate.
    /// </value>
    [JsonPropertyName("spam_rate")]
    [JsonPropertyOrder(14)]
    public float SpamRate { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages that unsubscribed (0–1).
    /// </summary>
    ///
    /// <value>
    /// Unsubscription rate.
    /// </value>
    [JsonPropertyName("unsubscription_rate")]
    [JsonPropertyOrder(15)]
    public float UnsubscriptionRate { get; set; }
}
