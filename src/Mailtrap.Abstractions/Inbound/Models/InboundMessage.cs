namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents a received inbound message.<br/>
/// Body and raw-message fields (<see cref="HtmlBody"/>, <see cref="TextBody"/>,
/// <see cref="RawMessageUrl"/>, ...) and attachment download URLs are populated
/// on get-by-id; list items carry only the summary fields.
/// </summary>
public sealed record InboundMessage
{
    /// <summary>
    /// Gets or sets the message identifier.
    /// </summary>
    ///
    /// <value>
    /// Message identifier.
    /// </value>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the inbox the message belongs to.
    /// </summary>
    ///
    /// <value>
    /// Inbox identifier.
    /// </value>
    [JsonPropertyName("inbox_id")]
    public long? InboxId { get; set; }

    /// <summary>
    /// Gets or sets the sender address.
    /// </summary>
    ///
    /// <value>
    /// Sender address.
    /// </value>
    [JsonPropertyName("from")]
    public string? From { get; set; }

    /// <summary>
    /// Gets the recipient addresses.
    /// </summary>
    ///
    /// <value>
    /// Recipient addresses.
    /// </value>
    [JsonPropertyName("to")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> To { get; } = [];

    /// <summary>
    /// Gets the carbon-copy addresses.
    /// </summary>
    ///
    /// <value>
    /// Carbon-copy addresses.
    /// </value>
    [JsonPropertyName("cc")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> Cc { get; } = [];

    /// <summary>
    /// Gets the blind carbon-copy addresses.
    /// </summary>
    ///
    /// <value>
    /// Blind carbon-copy addresses.
    /// </value>
    [JsonPropertyName("bcc")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> Bcc { get; } = [];

    /// <summary>
    /// Gets or sets the reply-to address.
    /// </summary>
    ///
    /// <value>
    /// Reply-to address.
    /// </value>
    [JsonPropertyName("reply_to")]
    public string? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the subject line.
    /// </summary>
    ///
    /// <value>
    /// Subject line.
    /// </value>
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the RFC <c>Message-ID</c> header value.
    /// </summary>
    ///
    /// <value>
    /// RFC <c>Message-ID</c> header value.
    /// </value>
    [JsonPropertyName("rfc_message_id")]
    public string? RfcMessageId { get; set; }

    /// <summary>
    /// Gets or sets the RFC <c>In-Reply-To</c> header value.
    /// </summary>
    ///
    /// <value>
    /// RFC <c>In-Reply-To</c> header value.
    /// </value>
    [JsonPropertyName("in_reply_to")]
    public string? InReplyTo { get; set; }

    /// <summary>
    /// Gets the RFC <c>References</c> header values.
    /// </summary>
    ///
    /// <value>
    /// RFC <c>References</c> header values.
    /// </value>
    [JsonPropertyName("references")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> References { get; } = [];

    /// <summary>
    /// Gets the message headers.
    /// </summary>
    ///
    /// <value>
    /// Message headers.
    /// </value>
    [JsonPropertyName("headers")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the total message size, in bytes.
    /// </summary>
    ///
    /// <value>
    /// Total message size, in bytes.
    /// </value>
    [JsonPropertyName("size")]
    public int? Size { get; set; }

    /// <summary>
    /// Gets or sets the HTML body size, in bytes.
    /// </summary>
    ///
    /// <value>
    /// HTML body size, in bytes.
    /// </value>
    [JsonPropertyName("html_size")]
    public int? HtmlSize { get; set; }

    /// <summary>
    /// Gets or sets the text body size, in bytes.
    /// </summary>
    ///
    /// <value>
    /// Text body size, in bytes.
    /// </value>
    [JsonPropertyName("text_size")]
    public int? TextSize { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the message was received.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message was received.
    /// </value>
    [JsonPropertyName("received_at")]
    public DateTimeOffset? ReceivedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the thread the message belongs to.
    /// </summary>
    ///
    /// <value>
    /// Thread identifier.
    /// </value>
    [JsonPropertyName("thread_id")]
    public string? ThreadId { get; set; }

    /// <summary>
    /// Gets the message attachments.
    /// </summary>
    ///
    /// <value>
    /// Message attachments.
    /// </value>
    [JsonPropertyName("attachments")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<InboundAttachment> Attachments { get; } = [];

    /// <summary>
    /// Gets the forward outcomes.
    /// </summary>
    ///
    /// <value>
    /// Forward outcomes.
    /// </value>
    [JsonPropertyName("forwards")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ForwardOutcome> Forwards { get; } = [];

    /// <summary>
    /// Gets or sets the signed URL to download the raw <c>.eml</c> message (get-by-id only).
    /// </summary>
    ///
    /// <value>
    /// Signed raw-message URL.
    /// </value>
    [JsonPropertyName("raw_message_url")]
    public string? RawMessageUrl { get; set; }

    /// <summary>
    /// Gets or sets the expiration timestamp of the signed raw-message URL.
    /// </summary>
    ///
    /// <value>
    /// Expiration timestamp of the signed raw-message URL.
    /// </value>
    [JsonPropertyName("raw_message_expires_at")]
    public DateTimeOffset? RawMessageExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the HTML body (get-by-id only).
    /// </summary>
    ///
    /// <value>
    /// HTML body.
    /// </value>
    [JsonPropertyName("html_body")]
    public string? HtmlBody { get; set; }

    /// <summary>
    /// Gets or sets the plain-text body (get-by-id only).
    /// </summary>
    ///
    /// <value>
    /// Plain-text body.
    /// </value>
    [JsonPropertyName("text_body")]
    public string? TextBody { get; set; }
}
