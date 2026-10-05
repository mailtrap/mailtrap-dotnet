namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents a message inside a thread.<br/>
/// Only <see cref="VisibilityStatus"/> and <see cref="Direction"/> are guaranteed;
/// <see cref="ThreadMessageVisibilityStatus.Placeholder"/> entries omit the rest.
/// </summary>
public sealed record InboundThreadMessage
{
    /// <summary>
    /// Gets or sets the visibility status of the message.
    /// </summary>
    ///
    /// <value>
    /// Visibility status of the message.
    /// </value>
    [JsonPropertyName("visibility_status")]
    public ThreadMessageVisibilityStatus VisibilityStatus { get; set; } = ThreadMessageVisibilityStatus.Unknown;

    /// <summary>
    /// Gets or sets the direction of the message.
    /// </summary>
    ///
    /// <value>
    /// Direction of the message.
    /// </value>
    [JsonPropertyName("direction")]
    public ThreadMessageDirection Direction { get; set; } = ThreadMessageDirection.Unknown;

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
    /// Gets or sets the message group identifier (shared by a reply-all fan-out).
    /// </summary>
    ///
    /// <value>
    /// Message group identifier.
    /// </value>
    [JsonPropertyName("message_group_id")]
    public string? MessageGroupId { get; set; }

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
    /// Gets or sets the timestamp when the message was created.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message was created.
    /// </value>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the message size, in bytes.
    /// </summary>
    ///
    /// <value>
    /// Message size, in bytes.
    /// </value>
    [JsonPropertyName("email_size")]
    public int? EmailSize { get; set; }

    /// <summary>
    /// Gets or sets the plain-text body.
    /// </summary>
    ///
    /// <value>
    /// Plain-text body.
    /// </value>
    [JsonPropertyName("text_body")]
    public string? TextBody { get; set; }

    /// <summary>
    /// Gets or sets the HTML body.
    /// </summary>
    ///
    /// <value>
    /// HTML body.
    /// </value>
    [JsonPropertyName("html_body")]
    public string? HtmlBody { get; set; }

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
    /// Gets or sets the delivery outcome (outbound messages only).
    /// </summary>
    ///
    /// <value>
    /// Delivery outcome.
    /// </value>
    [JsonPropertyName("delivery")]
    public ThreadMessageDelivery? Delivery { get; set; }

    /// <summary>
    /// Gets the forward outcomes (inbound messages only).
    /// </summary>
    ///
    /// <value>
    /// Forward outcomes.
    /// </value>
    [JsonPropertyName("forwards")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ForwardOutcome> Forwards { get; } = [];
}
