namespace Mailtrap.ContactEvents.Requests;

/// <summary>
/// Request object for creating a contact event.
/// </summary>
public sealed record CreateContactEventRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the contact event name.
    /// </summary>
    ///
    /// <value>
    /// Contact event name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonRequired]
    public string Name { get; set; }

    /// <summary>
    /// Gets contact event params.
    /// </summary>
    ///
    /// <value>
    /// Contact event params.
    /// </value>
    [JsonPropertyName("params")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, object?> Params { get; } = new Dictionary<string, object?>();

    /// <summary>
    /// Primary instance constructor.
    /// </summary>
    ///
    /// <param name="name">
    /// <inheritdoc cref="CreateContactEventRequest" path="/param[@name=name]"/>.
    /// </param>
    ///
    /// <param name="params">
    /// <inheritdoc cref="CreateContactEventRequest" path="/param[@name=params]"/>.
    /// </param>
    ///
    /// <remarks>
    /// <paramref name="name"/> must be 1-255 characters long.
    /// </remarks>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="name"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    public CreateContactEventRequest(string name, IDictionary<string, object?>? @params = null)
    {
        Ensure.NotNullOrEmpty(name, nameof(name));

        Name = name;

        if (@params is not null)
        {
            // Defensive copy to prevent post-ctor mutation.
            Params = @params.Clone();
        }
    }

    /// <summary>
    /// Parameterless constructor for serializers.
    /// </summary>
    [JsonConstructor]
    public CreateContactEventRequest()
    {
        Name = string.Empty;
    }

    /// <inheritdoc />
    public ValidationResult Validate()
    {
        return CreateContactEventRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
