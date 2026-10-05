namespace Mailtrap.Templates.Models;

/// <summary>
/// Represents Template details.
/// </summary>
public sealed record Template
{
    /// <summary>
    /// Gets or sets template identifier.
    /// </summary>
    ///
    /// <value>
    /// Template identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets template unique identifier.
    /// </summary>
    ///
    /// <value>
    /// Template unique identifier.
    /// </value>
    [JsonPropertyName("uuid")]
    [JsonPropertyOrder(2)]
    public string Uuid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets template name.
    /// </summary>
    /// <remarks>
    /// Template name must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(3)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template category.
    /// </summary>
    /// <remarks>
    /// Template category must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template category.
    /// </value>
    [JsonPropertyName("category")]
    [JsonPropertyOrder(4)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template subject.
    /// </summary>
    /// <remarks>
    /// Template subject must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template subject.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(5)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template body text.
    /// </summary>
    /// <remarks>
    /// Template body text must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Template's body text.
    /// </value>
    [JsonPropertyName("body_text")]
    [JsonPropertyOrder(6)]
    public string BodyText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template body HTML.
    /// </summary>
    /// <remarks>
    /// Template body HTML must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Template's body HTML.
    /// </value>
    [JsonPropertyName("body_html")]
    [JsonPropertyOrder(7)]
    public string BodyHtml { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template creation date and time.
    /// </summary>
    /// <value>
    /// Template creation date and time.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(8)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the template date and time of update.
    /// </summary>
    /// <value>
    /// Template date and time of update.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(9)]
    public DateTimeOffset? UpdatedAt { get; set; }
}
