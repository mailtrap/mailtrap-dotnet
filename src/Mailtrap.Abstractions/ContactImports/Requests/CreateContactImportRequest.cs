namespace Mailtrap.ContactImports.Requests;

/// <summary>
/// Generic request object for creating a contact import.
/// </summary>
public sealed record CreateContactImportRequest : IValidatable
{
    /// <summary>
    /// Gets collection of <see cref="ContactImportRequest"/> for import.
    /// </summary>
    ///
    /// <value>
    /// Contact requests collection for import.
    /// </value>
    [JsonPropertyName("contacts")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ContactImportRequest> Contacts { get; } = [];

    /// <summary>
    /// Primary instance constructor.
    /// </summary>
    ///
    /// <param name="contacts">
    /// Collection of contact requests to import.
    /// </param>
    ///
    /// <remarks>
    /// Each request in the <paramref name="contacts"/> must include a valid email.
    /// Size and item-level constraints are validated by <see cref="Validate"/>.
    /// </remarks>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="contacts"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <para>
    /// Use <see cref="Validate"/> to ensure the count is within the allowed range
    /// (currently 1–50,000) and each contact satisfies per-item rules.
    /// </para>
    public CreateContactImportRequest(IEnumerable<ContactImportRequest> contacts)
    {
        Ensure.NotNullOrEmpty(contacts, nameof(contacts));

        // Defensive copy to prevent post-ctor mutation.
        Contacts = contacts.Clone();
    }

    /// <summary>
    /// Parameterless instance constructor for serializers.
    /// </summary>
    [JsonConstructor]
    public CreateContactImportRequest() { }

    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateContactImportRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
