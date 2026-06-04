namespace Mailtrap.EmailCampaigns;


internal sealed class EmailCampaignCollectionResource : RestResource, IEmailCampaignCollectionResource
{
    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";
    private const string SearchQueryParameter = "search";


    public EmailCampaignCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<EmailCampaignList> GetAll(EmailCampaignListFilter? filter = null, CancellationToken cancellationToken = default)
    {
        var uri = CreateListUri(filter);

        var response = await RestResourceCommandFactory
            .CreateGet<EmailCampaignListResponseDto>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);

        return response.FromDto();
    }

    public async Task<EmailCampaign> Create(CreateEmailCampaignRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        return await Create<CreateEmailCampaignRequestDto, EmailCampaign>(request.ToDto(), cancellationToken).ConfigureAwait(false);
    }


    private Uri CreateListUri(EmailCampaignListFilter? filter)
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

        if (!string.IsNullOrEmpty(filter.Search))
        {
            uri = uri.AppendQueryParameter(SearchQueryParameter, filter.Search);
        }

        return uri;
    }
}
