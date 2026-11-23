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
    /// Gets or sets the identifier of the sending domain used for the campaign,
    /// as returned by the Sending Domains endpoints.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(2)]
    public long DomainId { get; set; }

    /// <summary>
    /// Gets or sets the name of the sending domain used for the campaign.
    /// </summary>
    ///
    /// <value>
    /// Sending domain name.
    /// </value>
    [JsonPropertyName("domain_name")]
    [JsonPropertyOrder(3)]
    public string? DomainName { get; set; }

    /// <summary>
    /// Gets or sets the campaign name.
    /// </summary>
    ///
    /// <value>
    /// Campaign name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(4)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the local part (before the @) of the From address.
    /// </summary>
    ///
    /// <value>
    /// From address local part.
    /// </value>
    [JsonPropertyName("from_local_part")]
    [JsonPropertyOrder(5)]
    public string? FromLocalPart { get; set; }

    /// <summary>
    /// Gets or sets the display name shown in the From header.
    /// </summary>
    ///
    /// <value>
    /// From display name.
    /// </value>
    [JsonPropertyName("from_display_name")]
    [JsonPropertyOrder(6)]
    public string? FromDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the Reply-To address parts.
    /// </summary>
    ///
    /// <value>
    /// Reply-To address parts, or <see langword="null"/> when not set.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(7)]
    public ReplyTo? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the current state of the campaign in its lifecycle.
    /// </summary>
    ///
    /// <value>
    /// Current campaign state. Allowed values: "draft", "scheduled", "started", "queued", "paused",
    /// "terminating", "under_review", "finished", "failed", "failed_immediately".
    /// </value>
    [JsonPropertyName("current_state")]
    [JsonPropertyOrder(8)]
    public CampaignState CurrentState { get; set; } = CampaignState.Unknown;

    /// <summary>
    /// Gets or sets metadata about the most recent state transition.
    /// </summary>
    ///
    /// <value>
    /// State transition metadata, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("current_state_metadata")]
    [JsonPropertyOrder(9)]
    public EmailCampaignStateMetadata? CurrentStateMetadata { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign was created.
    /// </summary>
    ///
    /// <value>
    /// Creation timestamp.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(10)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign was last updated.
    /// </summary>
    ///
    /// <value>
    /// Last update timestamp.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(11)]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time the campaign was last started.
    /// </summary>
    ///
    /// <value>
    /// Timestamp the campaign was last started, or <see langword="null"/> when it has never been started.
    /// </value>
    [JsonPropertyName("last_started_at")]
    [JsonPropertyOrder(12)]
    public DateTimeOffset? LastStartedAt { get; set; }

    /// <summary>
    /// Gets or sets the date the campaign was last started.
    /// </summary>
    ///
    /// <value>
    /// Date the campaign was last started, present only when the campaign has been started.
    /// </value>
    [JsonPropertyName("last_started_at_date")]
    [JsonPropertyOrder(13)]
    public string? LastStartedAtDate { get; set; }

    /// <summary>
    /// Gets or sets the total number of recipients targeted by the campaign.
    /// </summary>
    ///
    /// <value>
    /// Total number of recipients, or <see langword="null"/> until the audience is resolved.
    /// </value>
    [JsonPropertyName("recipient_total_count")]
    [JsonPropertyOrder(14)]
    public int? RecipientTotalCount { get; set; }

    /// <summary>
    /// Gets the identifiers of the contact lists included in the campaign's audience.
    /// </summary>
    ///
    /// <value>
    /// Contact list identifiers.
    /// </value>
    [JsonPropertyName("contact_list_ids")]
    [JsonPropertyOrder(15)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<long> ContactListIds { get; } = [];

    /// <summary>
    /// Gets the identifiers of the contact segments included in the campaign's audience.
    /// </summary>
    ///
    /// <value>
    /// Contact segment identifiers.
    /// </value>
    [JsonPropertyName("contact_segment_ids")]
    [JsonPropertyOrder(16)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<long> ContactSegmentIds { get; } = [];

    /// <summary>
    /// Gets or sets how the campaign is delivered.
    /// </summary>
    ///
    /// <value>
    /// Delivery mode. Allowed values: "rapid", "gradual".
    /// </value>
    [JsonPropertyName("delivery_mode")]
    [JsonPropertyOrder(17)]
    public DeliveryMode DeliveryMode { get; set; } = DeliveryMode.Unknown;

    /// <summary>
    /// Gets or sets the delivery throttling options.
    /// </summary>
    ///
    /// <value>
    /// Delivery throttling options, or <see langword="null"/> when not set.
    /// </value>
    [JsonPropertyName("delivery_options")]
    [JsonPropertyOrder(18)]
    public EmailCampaignDeliveryOptions? DeliveryOptions { get; set; }

    /// <summary>
    /// Gets or sets the template associated with the campaign.
    /// </summary>
    ///
    /// <value>
    /// Campaign template, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("template")]
    [JsonPropertyOrder(19)]
    public EmailCampaignTemplate? Template { get; set; }
}
