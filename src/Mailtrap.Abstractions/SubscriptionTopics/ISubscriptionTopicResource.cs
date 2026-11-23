namespace Mailtrap.SubscriptionTopics;


/// <summary>
/// Represents a single subscription topic resource.
/// </summary>
/// <remarks>
/// <inheritdoc cref="ISubscriptionTopicCollectionResource" path="/remarks"/>
/// </remarks>
public interface ISubscriptionTopicResource : IRestResource
{
    /// <summary>
    /// Gets the details of the subscription topic, represented by this resource instance.<br/>
    /// A deleted topic is returned too, with <see cref="SubscriptionTopic.DeletedAt"/> set.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Subscription topic details.
    /// </returns>
    public Task<SubscriptionTopic> GetDetails(CancellationToken cancellationToken = default);
}
