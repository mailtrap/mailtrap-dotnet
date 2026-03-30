namespace Mailtrap.TestingMessages.Models;


/// <summary>
/// Represents details of a message in inbox.
/// </summary>
public sealed record TestingMessage
{
    /// <summary>
    /// Gets message identifier.
    /// </summary>
    ///
    /// <value>
    /// Message identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets message inbox identifier.
    /// </summary>
    ///
    /// <value>
    /// Message inbox identifier.
    /// </value>
    [JsonPropertyName("inbox_id")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public long InboxId { get; set; }

    /// <summary>
    /// Gets message subject.
    /// </summary>
    ///
    /// <value>
    /// Message subject.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(3)]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets email address of 'From' field of the message.
    /// </summary>
    ///
    /// <value>
    /// Email address of 'From' field of the message.
    /// </value>
    [JsonPropertyName("from_email")]
    [JsonPropertyOrder(4)]
    public string? FromEmail { get; set; }

    /// <summary>
    /// Gets display name of 'From' field of the message.
    /// </summary>
    ///
    /// <value>
    /// Display name of 'From' field of the message.
    /// </value>
    [JsonPropertyName("from_name")]
    [JsonPropertyOrder(5)]
    public string? FromName { get; set; }

    /// <summary>
    /// Gets email address of 'To' field of the message.
    /// </summary>
    ///
    /// <value>
    /// Email address of 'To' field of the message.
    /// </value>
    [JsonPropertyName("to_email")]
    [JsonPropertyOrder(6)]
    public string? ToEmail { get; set; }

    /// <summary>
    /// Gets display name of 'To' field of the message.
    /// </summary>
    ///
    /// <value>
    /// Display name of 'To' field of the message.
    /// </value>
    [JsonPropertyName("to_name")]
    [JsonPropertyOrder(7)]
    public string? ToName { get; set; }

    /// <summary>
    /// Gets size of the message.
    /// </summary>
    ///
    /// <value>
    /// Size of the message.
    /// </value>
    [JsonPropertyName("email_size")]
    [JsonPropertyOrder(8)]
    public long? EmailSize { get; set; }

    /// <summary>
    /// Gets flag indicating if the message was read.
    /// </summary>
    ///
    /// <value>
    /// <see langword="true"/> if the message was read.<br />
    /// <see langword="false"/> otherwise.
    /// </value>
    [JsonPropertyName("is_read")]
    [JsonPropertyOrder(9)]
    public bool? IsRead { get; set; }

    /// <summary>
    /// Gets timestamp when the message was created.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message was created.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(10)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets timestamp when the message was updated.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message was updated.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(11)]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets timestamp when the message was sent.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message was sent.
    /// </value>
    [JsonPropertyName("sent_at")]
    [JsonPropertyOrder(12)]
    public DateTimeOffset? SentAt { get; set; }

    /// <summary>
    /// Gets HTML body size of the message.
    /// </summary>
    ///
    /// <value>
    /// HTML body size of the message.
    /// </value>
    [JsonPropertyName("html_body_size")]
    [JsonPropertyOrder(13)]
    public long? HtmlBodySize { get; set; }

    /// <summary>
    /// Gets plain text body size of the message.
    /// </summary>
    ///
    /// <value>
    /// Plain text body size of the message.
    /// </value>
    [JsonPropertyName("text_body_size")]
    [JsonPropertyOrder(14)]
    public long? TextBodySize { get; set; }

    /// <summary>
    /// Gets human-friendly size of the message.
    /// </summary>
    ///
    /// <value>
    /// Human-friendly size of the message.<br />
    /// E.g. '345 Bytes'.
    /// </value>
    [JsonPropertyName("human_size")]
    [JsonPropertyOrder(15)]
    public string? HumanSize { get; set; }

    /// <summary>
    /// Gets path to the HTML representation of the message.
    /// </summary>
    ///
    /// <value>
    /// Path to the HTML representation of the message.
    /// </value>
    [JsonPropertyName("html_path")]
    [JsonPropertyOrder(16)]
    public string? HtmlPath { get; set; }

    /// <summary>
    /// Gets path to the plain text representation of the message.
    /// </summary>
    ///
    /// <value>
    /// Path to the plain text representation of the message.
    /// </value>
    [JsonPropertyName("txt_path")]
    [JsonPropertyOrder(17)]
    public string? TextPath { get; set; }

    /// <summary>
    /// Gets path to the raw representation of the message.
    /// </summary>
    ///
    /// <value>
    /// Path to the raw representation of the message.
    /// </value>
    [JsonPropertyName("raw_path")]
    [JsonPropertyOrder(18)]
    public string? RawPath { get; set; }

    /// <summary>
    /// Gets download path for the message.
    /// </summary>
    ///
    /// <value>
    /// Download path for the message.
    /// </value>
    [JsonPropertyName("download_path")]
    [JsonPropertyOrder(19)]
    public string? DownloadPath { get; set; }

    /// <summary>
    /// Gets path to the message HTML source.
    /// </summary>
    ///
    /// <value>
    /// Path to the message HTML source.
    /// </value>
    [JsonPropertyName("html_source_path")]
    [JsonPropertyOrder(20)]
    public string? HtmlSourcePath { get; set; }

    /// <summary>
    /// Gets blacklist report for the message.
    /// </summary>
    ///
    /// <value>
    /// Blacklist report for the message.
    /// </value>
    [JsonPropertyName("blacklists_report_info")]
    [JsonPropertyOrder(21)]
    [JsonConverter(typeof(BlacklistReportJsonConverter))]
    public BlacklistReport BlacklistsReportInfo { get; set; } = new(); // Can't use JSON Populate here, since it uses custom converter

    /// <summary>
    /// Gets SMTP information for the message.
    /// </summary>
    ///
    /// <value>
    /// SMTP information for the message.
    /// </value>
    [JsonPropertyName("smtp_information")]
    [JsonPropertyOrder(22)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public SmtpInformation SmtpInformation { get; } = new();
}
