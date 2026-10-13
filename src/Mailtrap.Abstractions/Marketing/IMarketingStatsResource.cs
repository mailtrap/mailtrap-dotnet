namespace Mailtrap.Marketing;


/// <summary>
/// Represents Email Marketing statistics resource.
/// </summary>
public interface IMarketingStatsResource : IRestResource
{
    /// <summary>
    /// Gets aggregated Email Marketing statistics (campaigns and automations together) for the period
    /// and filters specified by <paramref name="filter"/>.
    /// </summary>
    ///
    /// <param name="filter">
    /// Filter parameters for the stats period. <see cref="MarketingStatsFilter.StartDate"/>
    /// and <see cref="MarketingStatsFilter.EndDate"/> are required.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Aggregated Email Marketing statistics. All counts and rates are <c>0</c> when nothing matched the filters.
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="filter"/> is <see langword="null"/>,
    /// or its <see cref="MarketingStatsFilter.StartDate"/> or <see cref="MarketingStatsFilter.EndDate"/> is empty.
    /// </exception>
    public Task<MarketingStats> GetStats(MarketingStatsFilter filter, CancellationToken cancellationToken = default);
}
