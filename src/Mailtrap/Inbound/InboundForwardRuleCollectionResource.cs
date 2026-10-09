namespace Mailtrap.Inbound;


internal sealed class InboundForwardRuleCollectionResource : RestResource, IInboundForwardRuleCollectionResource
{
    public InboundForwardRuleCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<InboundForwardRule>> GetAll(CancellationToken cancellationToken = default)
    {
        var response = await Get<InboundForwardRuleListResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.ForwardRules;
    }

    public async Task<InboundForwardRule> Create(CreateInboundForwardRuleRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var response = await Create<CreateInboundForwardRuleRequest, InboundForwardRuleResponseDto>(request, cancellationToken).ConfigureAwait(false);

        return response.ForwardRule;
    }
}
