namespace Mailtrap.ContactLists.Requests;

/// <summary>
/// Generic request object for contact list CRUD operations.
/// </summary>
public sealed record ContactListRequest : IValidatable
{
    /// <summary>
    /// Gets or sets contact list name.
    /// </summary>
    ///
    /// <value>
    /// Contact list name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Primary instance constructor.
    /// </summary>
    ///
    /// <param name="name">
    /// Name of the contact list.
    /// </param>
    ///
    /// <remarks>
    /// The <paramref name="name"/> must be between 1 and 255 characters.
    /// </remarks>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="name"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    [JsonConstructor]
    public ContactListRequest(string name)
    {
        Ensure.NotNullOrEmpty(name, nameof(name));

        Name = name;
    }

    /// <summary>
    /// Parameterless instance constructor for serializers.
    /// </summary>
    public ContactListRequest() { }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return ContactListRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
