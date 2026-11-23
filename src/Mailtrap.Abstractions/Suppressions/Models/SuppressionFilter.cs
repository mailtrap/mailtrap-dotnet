namespace Mailtrap.Suppressions.Models;


/// <summary>
/// Represents a set of filtering parameters for the suppression fetching.
/// </summary>
public sealed record SuppressionFilter
{
    /// <summary>
    /// Gets or sets an email address of suppressions that will be returned by fetch.<br />
    /// If specified, only suppressions with particular email address are returned.
    /// </summary>
    ///
    /// <value>
    /// Email address of suppressions that will be returned by fetch.
    /// </value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the lower bound of the creation timestamp.
    /// </summary>
    ///
    /// <value>
    /// Only suppressions created at or after this moment are returned.
    /// </value>
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the upper bound of the creation timestamp.
    /// </summary>
    ///
    /// <value>
    /// Only suppressions created at or before this moment are returned.
    /// </value>
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the pagination cursor.
    /// </summary>
    ///
    /// <value>
    /// Identifier from the previous response, to fetch records after it.
    /// </value>
    public string? LastId { get; set; }
}
