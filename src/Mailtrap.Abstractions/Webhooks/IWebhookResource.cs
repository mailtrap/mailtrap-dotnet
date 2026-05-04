namespace Mailtrap.Webhooks;


/// <summary>
/// Represents webhook resource.
/// </summary>
public interface IWebhookResource : IRestResource
{
    /// <summary>
    /// Gets details of the webhook, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Requested webhook details.
    /// </returns>
    public Task<Webhook> GetDetails(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the webhook, represented by the current resource instance, with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Webhook details for update. Only properties set on the request are sent.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Updated webhook details.
    /// </returns>
    public Task<Webhook> Update(UpdateWebhookRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes the webhook, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Deleted webhook details.
    /// </returns>
    public Task<Webhook> Delete(CancellationToken cancellationToken = default);
}
