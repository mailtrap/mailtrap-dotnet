namespace Mailtrap.Webhooks;


/// <summary>
/// Represents webhook collection resource.
/// </summary>
public interface IWebhookCollectionResource : IRestResource
{
    /// <summary>
    /// Returns all webhooks for the account.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of webhook details.
    /// </returns>
    public Task<IList<Webhook>> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new webhook with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing webhook details for creation.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetAll(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Created webhook details, including the <see cref="CreateWebhookResponse.SigningSecret"/>.
    /// </returns>
    ///
    /// <remarks>
    /// The signing secret is only returned at creation time - store it securely.
    /// </remarks>
    public Task<CreateWebhookResponse> Create(CreateWebhookRequest request, CancellationToken cancellationToken = default);
}