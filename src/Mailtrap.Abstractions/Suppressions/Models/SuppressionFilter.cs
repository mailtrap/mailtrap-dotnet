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
}
