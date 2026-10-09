namespace Mailtrap.Inbound;


/// <summary>
/// Represents the forward rule collection resource for an inbound inbox.
/// </summary>
public interface IInboundForwardRuleCollectionResource : IRestResource
{
    /// <summary>
    /// Returns all forward rules of the inbox.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of forward rules of the inbox.
    /// </returns>
    public Task<IList<InboundForwardRule>> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new forward rule on the inbox with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing forward rule details for creation.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetAll(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Created forward rule details.
    /// </returns>
    public Task<InboundForwardRule> Create(CreateInboundForwardRuleRequest request, CancellationToken cancellationToken = default);
}
