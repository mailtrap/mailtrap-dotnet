namespace Mailtrap.Inbound;


internal sealed class InboundForwardRuleResource : RestResource, IInboundForwardRuleResource
{
    public InboundForwardRuleResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<InboundForwardRule> GetDetails(CancellationToken cancellationToken = default)
    {
        var response = await Get<InboundForwardRuleResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.ForwardRule;
    }

    public async Task<InboundForwardRule> Update(UpdateInboundForwardRuleRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var response = await Update<UpdateInboundForwardRuleRequest, InboundForwardRuleResponseDto>(request, cancellationToken).ConfigureAwait(false);

        return response.ForwardRule;
    }

    public Task Delete(CancellationToken cancellationToken = default)
        => DeleteWithStatusCodeResult(cancellationToken);
}
