namespace Mailtrap.Suppressions;


internal sealed class SuppressionCollectionResource : RestResource, ISuppressionCollectionResource
{
    private const string EmailQueryParameter = "email";


    public SuppressionCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<Suppression>> Fetch(SuppressionFilter? filter = null, CancellationToken cancellationToken = default)
        => await GetList<Suppression>(CreateFetchUri(filter), cancellationToken).ConfigureAwait(false);


    private Uri CreateFetchUri(SuppressionFilter? filter)
    {
        return filter?.Email is not null && !string.IsNullOrWhiteSpace(filter.Email)
            ? ResourceUri.AppendQueryParameter(EmailQueryParameter, filter.Email)
            : ResourceUri;
    }
}
