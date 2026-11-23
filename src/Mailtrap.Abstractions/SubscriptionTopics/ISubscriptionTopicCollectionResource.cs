namespace Mailtrap.SubscriptionTopics;


/// <summary>
/// Represents the subscription topic collection resource.
/// </summary>
/// <remarks>
/// Subscription topics are read-only in the API: they are managed in the Mailtrap app.<br/>
/// For an account without subscription topics, the API responds with HTTP 403.
/// </remarks>
public interface ISubscriptionTopicCollectionResource : IRestResource
{
    /// <summary>
    /// Gets a paginated list of the account's subscription topics, ordered by identifier.<br/>
    /// Deleted topics are left out.
    /// </summary>
    ///
    /// <param name="filter">
    /// Optional pagination parameters.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// A page of subscription topics and the pagination metadata.
    /// </returns>
    public Task<SubscriptionTopicList> GetAll(SubscriptionTopicListFilter? filter = null, CancellationToken cancellationToken = default);
}
