namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents an email campaign.
/// </summary>
public sealed record EmailCampaign
{
    /// <summary>
    /// Gets or sets the email campaign identifier.
    /// </summary>
    ///
    /// <value>
    /// Email campaign identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the resource type discriminator.
    /// </summary>
    ///
    /// <value>
    /// Resource type discriminator (e.g. <c>EmailCampaign</c>).
    /// </value>
    [JsonPropertyName("type")]
    [JsonPropertyOrder(2)]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the sending domain used for the campaign.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier.
    /// </value>
    [JsonPropertyName("mailsend_domain_id")]
    [JsonPropertyOrder(3)]
    public long MailsendDomainId { get; set; }

    /// <summary>
    /// Gets or sets the name of the sending domain used for the campaign.
    /// </summary>
    ///
    /// <value>
    /// Sending domain name.
    /// </value>
    [JsonPropertyName("mailsend_domain_name")]
    [JsonPropertyOrder(4)]
    public string? MailsendDomainName { get; set; }

    /// <summary>
    /// Gets or sets the campaign name.
    /// </summary>
    ///
    /// <value>
    /// Campaign name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(5)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the local part (before the @) of the From address.
    /// </summary>
    ///
    /// <value>
    /// From address local part.
    /// </value>
    [JsonPropertyName("from_local_part")]
    [JsonPropertyOrder(6)]
    public string? FromLocalPart { get; set; }

    /// <summary>
    /// Gets or sets the display name shown in the From header.
    /// </summary>
    ///
    /// <value>
    /// From display name.
    /// </value>
    [JsonPropertyName("from_display_name")]
    [JsonPropertyOrder(7)]
    public string? FromDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the Reply-To address parts.
    /// </summary>
    ///
    /// <value>
    /// Reply-To address parts, or <see langword="null"/> when not set.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(8)]
    public ReplyTo? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the current state of the campaign in its lifecycle.
    /// </summary>
    ///
    /// <value>
    /// Current campaign state. Allowed values: "draft", "scheduled", "sending", "sent", "terminated".
    /// </value>
    [JsonPropertyName("current_state")]
    [JsonPropertyOrder(9)]
    public CampaignState CurrentState { get; set; } = CampaignState.Unknown;

    /// <summary>
    /// Gets or sets metadata about the most recent state transition.
    /// </summary>
    ///
    /// <value>
    /// State transition metadata, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("current_state_metadata")]
    [JsonPropertyOrder(10)]
    public EmailCampaignStateMetadata? CurrentStateMetadata { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign was created.
    /// </summary>
    ///
    /// <value>
    /// Creation timestamp.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(11)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign was last updated.
    /// </summary>
    ///
    /// <value>
    /// Last update timestamp.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(12)]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign was last started.
    /// </summary>
    ///
    /// <value>
    /// Timestamp the campaign was last started, or <see langword="null"/> when it has never been started.
    /// </value>
    [JsonPropertyName("last_started_at")]
    [JsonPropertyOrder(13)]
    public DateTimeOffset? LastStartedAt { get; set; }

    /// <summary>
    /// Gets or sets the date the campaign was last started.
    /// </summary>
    ///
    /// <value>
    /// Date the campaign was last started, present only when the campaign has been started.
    /// </value>
    [JsonPropertyName("last_started_at_date")]
    [JsonPropertyOrder(14)]
    public string? LastStartedAtDate { get; set; }

    /// <summary>
    /// Gets or sets the total number of recipients.
    /// </summary>
    ///
    /// <value>
    /// Total number of recipients.
    /// </value>
    [JsonPropertyName("recipient_total_count")]
    [JsonPropertyOrder(15)]
    public int RecipientTotalCount { get; set; }

    /// <summary>
    /// Gets or sets how the campaign is delivered.
    /// </summary>
    ///
    /// <value>
    /// Delivery mode. Allowed values: "immediate", "scheduled".
    /// </value>
    [JsonPropertyName("delivery_mode")]
    [JsonPropertyOrder(16)]
    public DeliveryMode DeliveryMode { get; set; } = DeliveryMode.Unknown;

    /// <summary>
    /// Gets or sets the delivery throttling options.
    /// </summary>
    ///
    /// <value>
    /// Delivery throttling options, or <see langword="null"/> when not set.
    /// </value>
    [JsonPropertyName("delivery_options")]
    [JsonPropertyOrder(17)]
    public EmailCampaignDeliveryOptions? DeliveryOptions { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign is scheduled to be sent.
    /// </summary>
    ///
    /// <value>
    /// Scheduled send time, or <see langword="null"/> when not scheduled.
    /// </value>
    [JsonPropertyName("scheduled_for")]
    [JsonPropertyOrder(18)]
    public DateTimeOffset? ScheduledFor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the campaign has recipients defined.<br/>
    /// Omitted from list items.
    /// </summary>
    ///
    /// <value>
    /// <see langword="true"/> when recipients are defined; <see langword="null"/> when omitted (e.g. in list items).
    /// </value>
    [JsonPropertyName("audience_defined")]
    [JsonPropertyOrder(19)]
    public bool? AudienceDefined { get; set; }

    /// <summary>
    /// Gets or sets the aggregated statistics for the campaign.<br/>
    /// Present only when the campaign carries stats.
    /// </summary>
    ///
    /// <value>
    /// Aggregated campaign statistics, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("stats")]
    [JsonPropertyOrder(20)]
    public EmailCampaignStats? Stats { get; set; }

    /// <summary>
    /// Gets or sets the template associated with the campaign.
    /// </summary>
    ///
    /// <value>
    /// Campaign template, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("template")]
    [JsonPropertyOrder(21)]
    public EmailCampaignTemplate? Template { get; set; }
}
