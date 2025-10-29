namespace Mailtrap.Contacts.Models;

/// <summary>
/// Represents Contact details.
/// </summary>
public sealed record Contact
{
    /// <summary>
    /// Gets or sets contact identifier.
    /// </summary>
    ///
    /// <value>
    /// Contact identifier.
    /// </value>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets contact email.
    /// </summary>
    ///
    /// <value>
    /// Contact email.
    /// </value>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets contact fields.
    /// </summary>
    ///
    /// <value>
    /// Contact fields.
    /// </value>
    [JsonPropertyName("fields")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, object> Fields { get; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets contact's list ids.
    /// </summary>
    ///
    /// <value>
    /// Contact's list ids.
    /// </value>
    [JsonPropertyName("list_ids")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<int> ListIds { get; } = [];

    /// <summary>
    /// Gets or sets status of the contact.
    /// </summary>
    ///
    /// <value>
    /// Contact status.
    /// </value>
    [JsonPropertyName("status")]
    public ContactStatus Status { get; set; } = ContactStatus.Unknown;

    /// <summary>
    /// Gets or sets contact creation date and time.
    /// </summary>
    ///
    /// <value>
    /// Contact creation date and time.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(NullableDateTimeOffsetUnixMsConverter))]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets contact's update date and time.
    /// </summary>
    ///
    /// <value>
    /// Contact's update date and time.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(NullableDateTimeOffsetUnixMsConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
}
