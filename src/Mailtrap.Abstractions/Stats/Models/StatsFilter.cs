namespace Mailtrap.Stats.Models;


/// <summary>
/// Represents a set of filtering parameters for the stats fetching.
/// </summary>
public sealed record StatsFilter
{
    /// <summary>
    /// Gets or sets the start date for the stats period.
    /// </summary>
    public string StartDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the end date for the stats period.
    /// </summary>
    public string EndDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets sending domain IDs to filter by.
    /// </summary>
    public IList<long> SendingDomainIds { get; } = [];

    /// <summary>
    /// Gets sending streams to filter by.
    /// </summary>
    public IList<string> SendingStreams { get; } = [];

    /// <summary>
    /// Gets categories to filter by.
    /// </summary>
    public IList<string> Categories { get; } = [];

    /// <summary>
    /// Gets email service providers to filter by.
    /// </summary>
    public IList<string> EmailServiceProviders { get; } = [];
}
