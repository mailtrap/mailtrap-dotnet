namespace Mailtrap.ApiTokens;


internal sealed class ApiTokenCollectionResource : RestResource, IApiTokenCollectionResource
{
    public ApiTokenCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<ApiToken>> GetAll(CancellationToken cancellationToken = default)
        => await GetList<ApiToken>(cancellationToken).ConfigureAwait(false);

    public async Task<CreateApiTokenResponse> Create(CreateApiTokenRequest request, CancellationToken cancellationToken = default)
        => await Create<CreateApiTokenRequest, CreateApiTokenResponse>(request, cancellationToken).ConfigureAwait(false);
}
