namespace Mailtrap.ContactExports.Requests;

/// <summary>
/// Request object for creating a contact export.
/// </summary>
public sealed record CreateContactExportRequest : IValidatable
{
    /// <summary>
    /// Gets collection of <see cref="ContactExportFilterBase"/> for export.
    /// </summary>
    ///
    /// <value>
    /// Contact export filters collection.
    /// </value>
    [JsonPropertyName("filters")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ContactExportFilterBase> Filters { get; } = [];

    /// <summary>
    /// Parameterless instance constructor for serializers.
    /// </summary>
    [JsonConstructor]
    public CreateContactExportRequest() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateContactExportRequest"/> class with provided filters.
    /// </summary>
    /// <param name="filters">
    /// Collection of contact export filters.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="filters"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <remarks>
    /// Use <see cref="Validate"/> to ensure the filters count is within the allowed range
    /// and each filter satisfies per-item rules.
    /// </remarks>
    public CreateContactExportRequest(IEnumerable<ContactExportFilterBase> filters)
    {
        Ensure.NotNullOrEmpty(filters, nameof(filters));

        // Defensive copy to prevent post-ctor mutation.
        Filters = filters.Clone();
    }

    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateContactExportRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
