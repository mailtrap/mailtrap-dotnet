namespace Mailtrap.ContactImports.Models;

/// <summary>
/// Generic response object for contact import operations.
/// </summary>
public sealed record ContactImport
{
    /// <summary>
    /// Gets or sets created contact import identifier.
    /// </summary>
    ///
    /// <value>
    /// Contact import identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets contact import status.
    /// </summary>
    ///
    /// <value>
    /// Contact import status.
    /// </value>
    [JsonPropertyName("status")]
    [JsonPropertyOrder(2)]
    public ContactImportStatus Status { get; set; } = ContactImportStatus.Unknown;

    /// <summary>
    /// Gets or sets count of created contacts.
    /// </summary>
    ///
    /// <value>
    /// Count of created contacts.
    /// </value>
    [JsonPropertyName("created_contacts_count")]
    [JsonPropertyOrder(3)]
    public long? CreatedContactsCount { get; set; }

    /// <summary>
    /// Gets or sets count of updated contacts.
    /// </summary>
    ///
    /// <value>
    /// Count of updated contacts.
    /// </value>
    [JsonPropertyName("updated_contacts_count")]
    [JsonPropertyOrder(4)]
    public long? UpdatedContactsCount { get; set; }

    /// <summary>
    /// Gets or sets count of contacts over limit.
    /// </summary>
    ///
    /// <value>
    /// Count of contacts over limit.
    /// </value>
    [JsonPropertyName("contacts_over_limit_count")]
    [JsonPropertyOrder(5)]
    public long? ContactsOverLimitCount { get; set; }
}
