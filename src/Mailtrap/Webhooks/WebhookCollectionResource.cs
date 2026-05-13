namespace Mailtrap.Webhooks;


internal sealed class WebhookCollectionResource : RestResource, IWebhookCollectionResource
{
    public WebhookCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<Webhook>> GetAll(CancellationToken cancellationToken = default)
    {
        var response = await Get<WebhookListResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.Webhooks;
    }

    public async Task<CreateWebhookResponse> Create(CreateWebhookRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var response = await Create<CreateWebhookRequestDto, CreateWebhookResponseDto>(request.ToDto(), cancellationToken).ConfigureAwait(false);

        return response.Webhook;
    }
}
