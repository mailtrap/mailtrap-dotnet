namespace Mailtrap.Suppressions.Models;

/// <summary>
/// Represents suppression details.
/// </summary>
public sealed record Suppression
{
    /// <summary>
    /// Gets or sets suppression identifier.
    /// </summary>
    ///
    /// <value>
    /// Suppression identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets suppression type.
    /// </summary>
    ///
    /// <value>
    /// Suppression type. Allowed values: "hard bounce", "spam complaint", "unsubscription", "manual import".
    /// </value>
    [JsonPropertyName("type")]
    [JsonPropertyOrder(2)]
    public SuppressionType Type { get; set; } = SuppressionType.Unknown;

    /// <summary>
    /// Gets or sets suppression creation date and time.
    /// </summary>
    ///
    /// <value>
    /// Suppression creation date and time.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(3)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets suppressed email address.
    /// </summary>
    ///
    /// <value>
    /// Suppressed email address.
    /// </value>
    [JsonPropertyName("email")]
    [JsonPropertyOrder(4)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets sending stream.
    /// </summary>
    ///
    /// <value>
    /// Sending stream. Allowed values: "any", "transactional", "bulk".
    /// </value>
    [JsonPropertyName("sending_stream")]
    [JsonPropertyOrder(5)]
    public SuppressionSendingStream SendingStream { get; set; } = SuppressionSendingStream.Unknown;

    /// <summary>
    /// Gets or sets domain name associated with the suppression.
    /// </summary>
    ///
    /// <value>
    /// Domain name or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("domain_name")]
    [JsonPropertyOrder(6)]
    public string? DomainName { get; set; }

    /// <summary>
    /// Gets or sets message bounce category.
    /// </summary>
    ///
    /// <value>
    /// Message bounce category or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_bounce_category")]
    [JsonPropertyOrder(7)]
    public string? MessageBounceCategory { get; set; }

    /// <summary>
    /// Gets or sets message category.
    /// </summary>
    ///
    /// <value>
    /// Message category or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_category")]
    [JsonPropertyOrder(8)]
    public string? MessageCategory { get; set; }

    /// <summary>
    /// Gets or sets message client IP.
    /// </summary>
    ///
    /// <value>
    /// Message client IP address or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_client_ip")]
    [JsonPropertyOrder(9)]
    public string? MessageClientIp { get; set; }

    /// <summary>
    /// Gets or sets message creation date and time.
    /// </summary>
    ///
    /// <value>
    /// Message creation date and time, or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_created_at")]
    [JsonPropertyOrder(10)]
    public DateTimeOffset? MessageCreatedAt { get; set; }

    /// <summary>
    /// Gets or sets ESP response.
    /// </summary>
    ///
    /// <value>
    /// ESP response or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_esp_response")]
    [JsonPropertyOrder(11)]
    public string? MessageEspResponse { get; set; }

    /// <summary>
    /// Gets or sets ESP server type.
    /// </summary>
    ///
    /// <value>
    /// ESP server type or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_esp_server_type")]
    [JsonPropertyOrder(12)]
    public string? MessageEspServerType { get; set; }

    /// <summary>
    /// Gets or sets outgoing IP used for the message.
    /// </summary>
    ///
    /// <value>
    /// Outgoing IP address or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_outgoing_ip")]
    [JsonPropertyOrder(13)]
    public string? MessageOutgoingIp { get; set; }

    /// <summary>
    /// Gets or sets recipient MX server name.
    /// </summary>
    ///
    /// <value>
    /// Recipient MX name or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_recipient_mx_name")]
    [JsonPropertyOrder(14)]
    public string? MessageRecipientMxName { get; set; }

    /// <summary>
    /// Gets or sets sender email address.
    /// </summary>
    ///
    /// <value>
    /// Sender email address or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_sender_email")]
    [JsonPropertyOrder(15)]
    public string? MessageSenderEmail { get; set; }

    /// <summary>
    /// Gets or sets message subject.
    /// </summary>
    ///
    /// <value>
    /// Message subject or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("message_subject")]
    [JsonPropertyOrder(16)]
    public string? MessageSubject { get; set; }
}
