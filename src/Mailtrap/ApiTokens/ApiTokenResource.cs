namespace Mailtrap.ApiTokens;


internal sealed class ApiTokenResource : RestResource, IApiTokenResource
{
    private const string ResetSegment = "reset";


    public ApiTokenResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<ApiToken> GetDetails(CancellationToken cancellationToken = default)
        => await Get<ApiToken>(cancellationToken).ConfigureAwait(false);

    public async Task Delete(CancellationToken cancellationToken = default)
        => await DeleteWithStatusCodeResult(cancellationToken).ConfigureAwait(false);

    public async Task<ApiTokenResetResponse> Reset(CancellationToken cancellationToken = default)
    {
        var uri = ResourceUri.Append(ResetSegment);

        var result = await RestResourceCommandFactory
            .CreatePost<ApiTokenResetResponse>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);

        return result;
    }
}
