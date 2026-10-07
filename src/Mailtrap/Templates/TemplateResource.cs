namespace Mailtrap.Templates;


internal sealed class TemplateResource : RestResource, ITemplateResource
{
    public TemplateResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<Template> GetDetails(CancellationToken cancellationToken = default)
    {
        var response = await Get<TemplateResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.Template;
    }

    public async Task<Template> Update(UpdateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var response = await Update<UpdateTemplateRequest, TemplateResponseDto>(request, cancellationToken).ConfigureAwait(false);

        return response.Template;
    }

    public async Task Delete(CancellationToken cancellationToken = default)
        => await DeleteWithStatusCodeResult(cancellationToken).ConfigureAwait(false);
}
