namespace Mailtrap.ContactLists.Models;


/// <summary>
/// Represents a set of filtering parameters for the contact list fetching.
/// </summary>
public sealed record ContactListListFilter
{
    /// <summary>
    /// Gets or sets a name to filter contact lists by.<br />
    /// If specified, only contact lists whose name starts with the provided value
    /// (case-insensitive prefix match) are returned.
    /// </summary>
    ///
    /// <value>
    /// Name prefix used to filter contact lists returned by fetch.
    /// </value>
    public string? Search { get; set; }
}
