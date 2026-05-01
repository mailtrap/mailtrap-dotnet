namespace Mailtrap.ApiTokens.Models;


/// <summary>
/// Represents API token details.
/// </summary>
public sealed record ApiToken
{
    /// <summary>
    /// Gets the API token identifier.
    /// </summary>
    ///
    /// <value>
    /// API token identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets the API token display name.
    /// </summary>
    ///
    /// <value>
    /// API token display name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the last 4 characters of the token.
    /// </summary>
    ///
    /// <value>
    /// Last 4 characters of the token. The full token value is only returned on create or reset.
    /// </value>
    [JsonPropertyName("last_4_digits")]
    [JsonPropertyOrder(3)]
    public string Last4Digits { get; set; } = string.Empty;

    /// <summary>
    /// Gets the name of the user or token that created this API token.
    /// </summary>
    ///
    /// <value>
    /// Creator name or <see langword="null"/> if not available.
    /// </value>
    [JsonPropertyName("created_by")]
    [JsonPropertyOrder(4)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets the date and time when the API token expires.
    /// </summary>
    ///
    /// <value>
    /// Expiration date and time, or <see langword="null"/> if the token does not expire.
    /// </value>
    [JsonPropertyName("expires_at")]
    [JsonPropertyOrder(5)]
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    /// Gets the resource accesses granted to this API token.
    /// </summary>
    ///
    /// <value>
    /// Collection of resource accesses.
    /// </value>
    [JsonPropertyName("resources")]
    [JsonPropertyOrder(6)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ApiTokenAccess> Resources { get; } = [];
}
