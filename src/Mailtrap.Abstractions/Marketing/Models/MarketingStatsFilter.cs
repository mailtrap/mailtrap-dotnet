namespace Mailtrap.Marketing.Models;


/// <summary>
/// Represents a set of filtering parameters for the Email Marketing stats fetching.
/// </summary>
public sealed record MarketingStatsFilter
{
    /// <summary>
    /// Gets or sets the start date for the stats period (inclusive), in <c>YYYY-MM-DD</c> format.
    /// </summary>
    ///
    /// <remarks>
    /// Required. Serialized to the <c>start_date</c> query parameter.
    /// </remarks>
    public string StartDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the end date for the stats period (inclusive), in <c>YYYY-MM-DD</c> format.
    /// </summary>
    ///
    /// <remarks>
    /// Required. Serialized to the <c>end_date</c> query parameter.
    /// </remarks>
    public string EndDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets sending domain IDs to filter by.
    /// </summary>
    public IList<long> SendingDomainIds { get; } = [];

    /// <summary>
    /// Gets Email Marketing sources to filter by: <c>campaigns</c> and/or <c>automations</c>.
    /// </summary>
    ///
    /// <remarks>
    /// A single value narrows the stats to that source.
    /// None or both mean all Email Marketing traffic.
    /// </remarks>
    public IList<string> Sources { get; } = [];

    /// <summary>
    /// Gets email service providers to filter by.
    /// </summary>
    public IList<string> EmailServiceProviders { get; } = [];
}
