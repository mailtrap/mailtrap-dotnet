namespace Mailtrap.Webhooks;


internal sealed class WebhookResource : RestResource, IWebhookResource
{
    public WebhookResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<Webhook> GetDetails(CancellationToken cancellationToken = default)
    {
        var response = await Get<WebhookResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.Webhook;
    }

    public async Task<Webhook> Update(UpdateWebhookRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var response = await Update<UpdateWebhookRequestDto, WebhookResponseDto>(request.ToDto(), cancellationToken).ConfigureAwait(false);

        return response.Webhook;
    }

    public async Task<Webhook> Delete(CancellationToken cancellationToken = default)
    {
        var response = await Delete<WebhookResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.Webhook;
    }
}
