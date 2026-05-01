namespace Mailtrap.ApiTokens.Requests;


/// <summary>
/// Represents an access entry to grant to an API token for a specific resource.
/// </summary>
public sealed record ApiTokenAccessRequest : IValidatable
{
    /// <summary>
    /// Gets the resource type.
    /// </summary>
    ///
    /// <value>
    /// Resource type.
    /// </value>
    [JsonPropertyName("resource_type")]
    [JsonPropertyOrder(1)]
    public ResourceType ResourceType { get; }

    /// <summary>
    /// Gets the resource identifier.
    /// </summary>
    ///
    /// <value>
    /// Resource identifier.
    /// </value>
    [JsonPropertyName("resource_id")]
    [JsonPropertyOrder(2)]
    public long ResourceId { get; }

    /// <summary>
    /// Gets the resource access level.
    /// </summary>
    ///
    /// <value>
    /// Access level for resource. Allowed values: <see cref="AccessLevel.Viewer"/> or <see cref="AccessLevel.Admin"/>.
    /// </value>
    [JsonPropertyName("access_level")]
    [JsonPropertyOrder(3)]
    public AccessLevel AccessLevel { get; }


    /// <summary>
    /// Primary instance constructor.
    /// </summary>
    ///
    /// <param name="resourceType">
    /// Type of the resource to grant access to.
    /// </param>
    ///
    /// <param name="resourceId">
    /// ID of the resource to grant access to.
    /// </param>
    ///
    /// <param name="accessLevel">
    /// Access level for the resource. Allowed values: <see cref="AccessLevel.Viewer"/> or <see cref="AccessLevel.Admin"/>.
    /// </param>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="resourceType"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="resourceId"/> is less than or equal to zero,
    /// or <paramref name="accessLevel"/> is not <see cref="AccessLevel.Viewer"/> or <see cref="AccessLevel.Admin"/>.
    /// </exception>
    public ApiTokenAccessRequest(
        ResourceType resourceType,
        long resourceId,
        AccessLevel accessLevel)
    {
        Ensure.NotNull(resourceType, nameof(resourceType));
        Ensure.GreaterThanZero(resourceId, nameof(resourceId));

        if (accessLevel is not AccessLevel.Viewer and not AccessLevel.Admin)
        {
            throw new ArgumentOutOfRangeException(
                nameof(accessLevel),
                accessLevel,
                "Allowed values are Viewer or Admin");
        }

        ResourceType = resourceType;
        ResourceId = resourceId;
        AccessLevel = accessLevel;
    }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return ApiTokenAccessRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
