namespace Mailtrap.ApiTokens.Models;


/// <summary>
/// Represents an access entry granted to an API token for a specific resource.
/// </summary>
public sealed record ApiTokenAccess
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
    public ResourceType Type { get; set; } = ResourceType.Unknown;

    /// <summary>
    /// Gets the resource identifier.
    /// </summary>
    ///
    /// <value>
    /// Resource identifier.
    /// </value>
    [JsonPropertyName("resource_id")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets the resource access level.
    /// </summary>
    ///
    /// <value>
    /// Access level for resource.
    /// </value>
    [JsonPropertyName("access_level")]
    [JsonPropertyOrder(3)]
    public AccessLevel AccessLevel { get; set; } = AccessLevel.Indeterminate;
}
