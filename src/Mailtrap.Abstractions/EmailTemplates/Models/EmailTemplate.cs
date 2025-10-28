namespace Mailtrap.EmailTemplates.Models;

/// <summary>
/// Represents Email Template details.
/// </summary>
public sealed record EmailTemplate
{
    /// <summary>
    /// Gets or sets email template identifier.
    /// </summary>
    ///
    /// <value>
    /// Email template identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets email template unique identifier.
    /// </summary>
    ///
    /// <value>
    /// Email template unique identifier.
    /// </value>
    [JsonPropertyName("uuid")]
    [JsonPropertyOrder(2)]
    public string Uuid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets email template name.
    /// </summary>
    /// <remarks>
    /// Email template name must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Email template name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(3)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email template category.
    /// </summary>
    /// <remarks>
    /// Email template category must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Email template category.
    /// </value>
    [JsonPropertyName("category")]
    [JsonPropertyOrder(4)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email template subject.
    /// </summary>
    /// <remarks>
    /// Email template subject must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Email template subject.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(5)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email template body text.
    /// </summary>
    /// <remarks>
    /// Email template body text must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Email template's body text.
    /// </value>
    [JsonPropertyName("body_text")]
    [JsonPropertyOrder(6)]
    public string BodyText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email template body HTML.
    /// </summary>
    /// <remarks>
    /// Email template body HTML must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Email template's body HTML.
    /// </value>
    [JsonPropertyName("body_html")]
    [JsonPropertyOrder(7)]
    public string BodyHtml { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email template creation date and time.
    /// </summary>
    /// <value>
    /// Email template creation date and time.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(8)]
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// Gets or sets the email template date and time of update.
    /// </summary>
    /// <value>
    /// Email template date and time of update.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(9)]
    public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.MinValue;
}
