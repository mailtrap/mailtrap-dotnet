namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents the Reply-To address parts of an email campaign.
/// </summary>
public sealed record ReplyTo
{
    /// <summary>
    /// Gets or sets the display name shown in the Reply-To header.
    /// </summary>
    ///
    /// <value>
    /// Reply-To display name.
    /// </value>
    [JsonPropertyName("display_name")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the local part (before the @) of the Reply-To address.
    /// </summary>
    ///
    /// <value>
    /// Reply-To local part.
    /// </value>
    [JsonPropertyName("local_part")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LocalPart { get; set; }

    /// <summary>
    /// Gets or sets the domain part of the Reply-To address.
    /// </summary>
    ///
    /// <value>
    /// Reply-To domain.
    /// </value>
    [JsonPropertyName("domain")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Domain { get; set; }
}
