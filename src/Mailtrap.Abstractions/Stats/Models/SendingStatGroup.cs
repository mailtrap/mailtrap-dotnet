namespace Mailtrap.Stats.Models;


/// <summary>
/// Represents a group of sending statistics with a name/value identifier.
/// </summary>
public sealed record SendingStatGroup
{
    /// <summary>
    /// Gets the name of the grouping key (e.g., "sending_domain_id", "category", "email_service_provider", "date").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the value of the grouping key.
    /// </summary>
    public object Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets the sending statistics for this group.
    /// </summary>
    public SendingStats Stats { get; set; } = new();
}
