namespace Mailtrap.SubscriptionTopics;


internal sealed class SubscriptionTopicCollectionResource : RestResource, ISubscriptionTopicCollectionResource
{
    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";


    public SubscriptionTopicCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<SubscriptionTopicList> GetAll(SubscriptionTopicListFilter? filter = null, CancellationToken cancellationToken = default)
        => await RestResourceCommandFactory
            .CreateGet<SubscriptionTopicList>(CreateListUri(filter))
            .Execute(cancellationToken)
            .ConfigureAwait(false);


    private Uri CreateListUri(SubscriptionTopicListFilter? filter)
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
