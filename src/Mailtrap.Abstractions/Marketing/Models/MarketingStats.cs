namespace Mailtrap.Marketing.Models;


/// <summary>
/// Represents aggregated Email Marketing metrics (campaigns and automations) for the requested period and filters.<br/>
/// All counts and rates are <c>0</c> when nothing matched the filters.
/// </summary>
public sealed record MarketingStats
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
    /// Gets or sets the share of sent messages that were delivered (0–1).
    /// </summary>
    ///
    /// <value>
    /// Delivery rate.
    /// </value>
    [JsonPropertyName("delivery_rate")]
    [JsonPropertyOrder(2)]
    public double DeliveryRate { get; set; }

    /// <summary>
    /// Gets or sets the number of bounced messages.
    /// </summary>
    ///
    /// <value>
    /// Number of bounced messages.
    /// </value>
    [JsonPropertyName("bounce_count")]
    [JsonPropertyOrder(3)]
    public int BounceCount { get; set; }

    /// <summary>
    /// Gets or sets the share of sent messages that bounced (0–1).
    /// </summary>
    ///
    /// <value>
    /// Bounce rate.
    /// </value>
    [JsonPropertyName("bounce_rate")]
    [JsonPropertyOrder(4)]
    public double BounceRate { get; set; }

    /// <summary>
    /// Gets or sets the number of opened messages.
    /// </summary>
    ///
    /// <value>
    /// Number of opened messages.
    /// </value>
    [JsonPropertyName("open_count")]
    [JsonPropertyOrder(5)]
    public int OpenCount { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages that were opened (0–1).
    /// </summary>
    ///
    /// <value>
    /// Open rate.
    /// </value>
    [JsonPropertyName("open_rate")]
    [JsonPropertyOrder(6)]
    public double OpenRate { get; set; }

    /// <summary>
    /// Gets or sets the number of clicked messages.
    /// </summary>
    ///
    /// <value>
    /// Number of clicked messages.
    /// </value>
    [JsonPropertyName("click_count")]
    [JsonPropertyOrder(7)]
    public int ClickCount { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages that were clicked (0–1).
    /// </summary>
    ///
    /// <value>
    /// Click rate.
    /// </value>
    [JsonPropertyName("click_rate")]
    [JsonPropertyOrder(8)]
    public double ClickRate { get; set; }

    /// <summary>
    /// Gets or sets the number of spam complaints.
    /// </summary>
    ///
    /// <value>
    /// Number of spam complaints.
    /// </value>
    [JsonPropertyName("spam_count")]
    [JsonPropertyOrder(9)]
    public int SpamCount { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages marked as spam (0–1).
    /// </summary>
    ///
    /// <value>
    /// Spam rate.
    /// </value>
    [JsonPropertyName("spam_rate")]
    [JsonPropertyOrder(10)]
    public double SpamRate { get; set; }

    /// <summary>
    /// Gets or sets the number of unsubscriptions.
    /// </summary>
    ///
    /// <value>
    /// Number of unsubscriptions.
    /// </value>
    [JsonPropertyName("unsubscription_count")]
    [JsonPropertyOrder(11)]
    public int UnsubscriptionCount { get; set; }

    /// <summary>
    /// Gets or sets the share of delivered messages that unsubscribed (0–1).
    /// </summary>
    ///
    /// <value>
    /// Unsubscription rate.
    /// </value>
    [JsonPropertyName("unsubscription_rate")]
    [JsonPropertyOrder(12)]
    public double UnsubscriptionRate { get; set; }

    /// <summary>
    /// Gets or sets the number of sent messages (delivered plus bounced).
    /// </summary>
    ///
    /// <value>
    /// Number of sent messages.
    /// </value>
    [JsonPropertyName("sent_count")]
    [JsonPropertyOrder(13)]
    public int SentCount { get; set; }
}
