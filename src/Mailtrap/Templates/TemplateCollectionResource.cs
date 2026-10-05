namespace Mailtrap.Templates;


internal sealed class TemplateCollectionResource : RestResource, ITemplateCollectionResource
{
    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";


    public TemplateCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<TemplateList> GetAll(TemplateListFilter? filter = null, CancellationToken cancellationToken = default)
    {
        var uri = CreateListUri(filter);

        var response = await RestResourceCommandFactory
            .CreateGet<TemplateListResponseDto>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);

        return response.FromDto();
    }

    public async Task<Template> Create(CreateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var response = await Create<CreateTemplateRequest, TemplateResponseDto>(request, cancellationToken).ConfigureAwait(false);

        return response.Template;
    }


    private Uri CreateListUri(TemplateListFilter? filter)
    {
        var uri = ResourceUri;

        if (filter is null)
        {
            return uri;
        }

        if (filter.Token is not null)
        {
            uri = uri.AppendQueryParameter(TokenQueryParameter, filter.Token.Value.ToUriSegment());
        }

        if (filter.PerPage is not null)
        {
            uri = uri.AppendQueryParameter(PerPageQueryParameter, filter.PerPage.Value.ToUriSegment());
        }

        return uri;
    }
}
