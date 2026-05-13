namespace Mailtrap.Organizations.Models;


/// <summary>
/// Represents a sub account within an organization.
/// </summary>
public sealed record SubAccount
{
    /// <summary>
    /// Gets the sub account identifier.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets the sub account name.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;
}
