namespace Mailtrap.Contacts.Requests;

/// <summary>
/// Generic request object for contact CRUD operations.
/// </summary>
public record ContactRequest : IValidatable
{
    /// <summary>
    /// Gets or sets contact email.
    /// </summary>
    ///
    /// <value>
    /// Contact email.
    /// </value>
    [JsonPropertyName("email")]
    [JsonRequired]
    public string Email { get; set; }

    /// <summary>
    /// Gets contact fields.
    /// </summary>
    ///
    /// <remarks>
    /// object-typed values may deserialize numbers as JsonElement
    /// </remarks>
    ///
    /// <value>
    /// Contact fields.
    /// </value>
    [JsonPropertyName("fields")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, object> Fields { get; } = new Dictionary<string, object>();

    /// <summary>
    /// Primary instance constructor.
    /// </summary>
    ///
    /// <param name="email">
    /// Email of the contact.
    /// </param>
    ///
    /// <remarks>
    /// Contact's <paramref name="email"/> must be min 2 characters and max 100 characters long.
    /// </remarks>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="email"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    public ContactRequest(string email)
    {
        Ensure.NotNullOrEmpty(email, nameof(email));

        Email = email;
    }

    /// <summary>
    /// Parameterless instance constructor for serializers.
    /// </summary>
    [JsonConstructor]
    public ContactRequest() { Email = string.Empty; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return ContactRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
