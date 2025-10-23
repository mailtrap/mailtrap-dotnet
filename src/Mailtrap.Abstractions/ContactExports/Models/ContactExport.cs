namespace Mailtrap.ContactExports.Models;

/// <summary>
/// Generic response object for contact export operations.
/// </summary>
public sealed record ContactExport
{
    /// <summary>
    /// Gets or sets created contact export identifier.
    /// </summary>
    ///
    /// <value>
    /// Contact export identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets contact export status.
    /// </summary>
    ///
    /// <value>
    /// Contact export status.
    /// </value>
    [JsonPropertyName("status")]
    [JsonPropertyOrder(2)]
    public ContactExportStatus Status { get; set; } = ContactExportStatus.Unknown;


    /// <summary>
    /// Gets or sets contact export creation date and time.
    /// </summary>
    ///
    /// <value>
    /// Contact export creation date and time.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(3)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets contact's export update date and time.
    /// </summary>
    ///
    /// <value>
    /// Contact's export update date and time.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(4)]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the URL of the exported contacts file.
    /// </summary>
    ///
    /// <value>
    /// The URL of the exported contacts file or <c>null</c> if not available.
    /// </value>
    /// <remarks>
    /// Only available when the export is finished.
    /// </remarks>
    [JsonPropertyName("url")]
    [JsonPropertyOrder(5)]
    public Uri? Url { get; set; }
}
