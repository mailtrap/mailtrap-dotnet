namespace Mailtrap.Organizations.Models;


/// <summary>
/// Attributes used to create a sub account.
/// </summary>
public sealed record SubAccountAttributes
{
    /// <summary>
    /// Gets or sets the sub account name.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    public string Name { get; set; } = string.Empty;
}
