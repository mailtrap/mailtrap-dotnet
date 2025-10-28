namespace Mailtrap.Suppressions;


/// <summary>
/// Represents suppressions collection resource.
/// </summary>
public interface ISuppressionCollectionResource : IRestResource
{
    /// <summary>
    /// List and search suppressions
    /// </summary>
    /// <param name="filter">
    /// Optional filter to apply when fetching suppressions.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    /// <returns>
    /// Collection of suppression details.
    /// </returns>
    /// <remarks>
    /// The endpoint returns up to 1000 suppressions per request.
    /// </remarks>
    public Task<IList<Suppression>> Fetch(SuppressionFilter? filter = null, CancellationToken cancellationToken = default);
}
