namespace Mailtrap.Stats;


/// <summary>
/// Represents sending statistics resource.
/// </summary>
public interface IStatsResource : IRestResource
{
    /// <summary>
    /// Gets aggregated sending statistics.
    /// </summary>
    ///
    /// <param name="filter">
    /// Filter parameters for the stats period.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Aggregated sending statistics.
    /// </returns>
    public Task<SendingStats> GetStats(StatsFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets sending statistics grouped by sending domains.
    /// </summary>
    ///
    /// <param name="filter">
    /// Filter parameters for the stats period.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of sending statistics grouped by sending domain.
    /// </returns>
    public Task<IList<SendingStatGroup>> ByDomain(StatsFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets sending statistics grouped by categories.
    /// </summary>
    ///
    /// <param name="filter">
    /// Filter parameters for the stats period.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of sending statistics grouped by category.
    /// </returns>
    public Task<IList<SendingStatGroup>> ByCategory(StatsFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets sending statistics grouped by email service providers.
    /// </summary>
    ///
    /// <param name="filter">
    /// Filter parameters for the stats period.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of sending statistics grouped by email service provider.
    /// </returns>
    public Task<IList<SendingStatGroup>> ByEmailServiceProvider(StatsFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets sending statistics grouped by date.
    /// </summary>
    ///
    /// <param name="filter">
    /// Filter parameters for the stats period.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of sending statistics grouped by date.
    /// </returns>
    public Task<IList<SendingStatGroup>> ByDate(StatsFilter filter, CancellationToken cancellationToken = default);
}
