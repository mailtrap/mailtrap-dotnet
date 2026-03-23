namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Represents an email log message (list item or single message with events).
/// </summary>
public sealed record EmailLogMessage
{
    /// <summary>
    /// Message identifier (UUID).
    /// </summary>
    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }

    /// <summary>
    /// Message status.
    /// </summary>
    [JsonPropertyName("status")]
    public EmailLogStatus Status { get; set; } = EmailLogStatus.Unknown;

    /// <summary>
    /// Subject line.
    /// </summary>
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    /// <summary>
    /// Sender address.
    /// </summary>
    [JsonPropertyName("from")]
    public string? From { get; set; }

    /// <summary>
    /// Recipient address.
    /// </summary>
    [JsonPropertyName("to")]
    public string? To { get; set; }

    /// <summary>
    /// When the message was sent.
    /// </summary>
    [JsonPropertyName("sent_at")]
    public DateTimeOffset? SentAt { get; set; }

    /// <summary>
    /// Client IP address.
    /// </summary>
    [JsonPropertyName("client_ip")]
    public string? ClientIp { get; set; }

    /// <summary>
    /// Message category.
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>
    /// Custom variables. May be null when the API omits or returns null.
    /// </summary>
    [JsonPropertyName("custom_variables")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, object?>? CustomVariables { get; set; }

    /// <summary>
    /// Sending stream.
    /// </summary>
    [JsonPropertyName("sending_stream")]
    public SendingStream SendingStream { get; set; } = SendingStream.Unknown;

    /// <summary>
    /// Sending domain ID.
    /// </summary>
    [JsonPropertyName("sending_domain_id")]
    public long? SendingDomainId { get; set; }

    /// <summary>
    /// Template ID.
    /// </summary>
    [JsonPropertyName("template_id")]
    public long? TemplateId { get; set; }

    /// <summary>
    /// Template variables. May be null when the API omits or returns null.
    /// </summary>
    [JsonPropertyName("template_variables")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, object?>? TemplateVariables { get; set; }

    /// <summary>
    /// Number of opens.
    /// </summary>
    [JsonPropertyName("opens_count")]
    public int OpensCount { get; set; }

    /// <summary>
    /// Number of clicks.
    /// </summary>
    [JsonPropertyName("clicks_count")]
    public int ClicksCount { get; set; }

    /// <summary>
    /// Signed URL to download raw .eml message (single-message response only).
    /// </summary>
    [JsonPropertyName("raw_message_url")]
    public string? RawMessageUrl { get; set; }

    /// <summary>
    /// Events for this message (single-message response only). May be null when the API omits or returns null.
    /// </summary>
    [JsonPropertyName("events")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<EmailLogMessageEvent>? Events { get; set; }
}
