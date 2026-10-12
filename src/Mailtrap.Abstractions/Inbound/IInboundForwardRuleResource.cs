namespace Mailtrap.Inbound;


/// <summary>
/// Represents an inbound forward rule resource.
/// </summary>
public interface IInboundForwardRuleResource : IRestResource
{
    /// <summary>
    /// Gets details of the forward rule, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Requested forward rule details.
    /// </returns>
    public Task<InboundForwardRule> GetDetails(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the forward rule, represented by the current resource instance, with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Forward rule details for update.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Updated forward rule details.
    /// </returns>
    public Task<InboundForwardRule> Update(UpdateInboundForwardRuleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes the forward rule, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// A task that represents the asynchronous delete operation.
    /// </returns>
    public Task Delete(CancellationToken cancellationToken = default);
}
