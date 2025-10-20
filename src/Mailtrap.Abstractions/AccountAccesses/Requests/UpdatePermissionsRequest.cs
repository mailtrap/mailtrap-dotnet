namespace Mailtrap.AccountAccesses.Requests;


/// <summary>
/// Request object for updating account access permissions.
/// </summary>
public sealed record UpdatePermissionsRequest : IValidatable
{
    /// <summary>
    /// Gets a collection of resources to update access permissions.
    /// </summary>
    ///
    /// <value>
    /// A list of resources to update access permissions.
    /// </value>
    [JsonPropertyName("permissions")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<UpdatePermissionsRequestItem> Permissions { get; } = [];


    /// <summary>
    /// Instance constructor.
    /// </summary>
    ///
    /// <param name="permissions">
    /// Collection of resources with permissions to update.
    /// </param>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="permissions"/> is <see langword="null"/>.
    /// </exception>
    public UpdatePermissionsRequest(params UpdatePermissionsRequestItem[] permissions)
    {
        Ensure.NotNull(permissions, nameof(permissions));

        Permissions = permissions.ToList(); // Defensive copy to prevent post-ctor mutation.
    }

    /// <summary>
    /// Instance constructor.
    /// </summary>
    ///
    /// <param name="permissions">
    /// Collection of resources with permissions to update.
    /// </param>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="permissions"/> is <see langword="null"/>.
    /// </exception>
    public UpdatePermissionsRequest(IEnumerable<UpdatePermissionsRequestItem> permissions)
    {
        Ensure.NotNull(permissions, nameof(permissions));

        Permissions = permissions.Clone();
    }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return UpdatePermissionsRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
