namespace Mailtrap.ApiTokens.Responses;


/// <summary>
/// Response returned when an API token is reset. Includes the new full token value,
/// which is only returned once at reset time.
/// </summary>
public sealed record ApiTokenResetResponse
{
    /// <summary>
    /// Gets the API token identifier.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets the API token display name.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the last 4 characters of the new token.
    /// </summary>
    [JsonPropertyName("last_4_digits")]
    [JsonPropertyOrder(3)]
    public string Last4Digits { get; set; } = string.Empty;

    /// <summary>
    /// Gets the name of the user or token that created this API token.
    /// </summary>
    [JsonPropertyName("created_by")]
    [JsonPropertyOrder(4)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets the date and time when the API token expires, or <see langword="null"/> if it does not expire.
    /// </summary>
    [JsonPropertyName("expires_at")]
    [JsonPropertyOrder(5)]
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    /// Gets the resource accesses granted to this API token.
    /// </summary>
    [JsonPropertyName("resources")]
    [JsonPropertyOrder(6)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ApiTokenAccess> Resources { get; } = [];

    /// <summary>
    /// Gets the new full token value. Only returned at reset time — store it securely.
    /// </summary>
    [JsonPropertyName("token")]
    [JsonPropertyOrder(7)]
    public string Token { get; set; } = string.Empty;
}
