namespace Mailtrap.TestingMessages;


internal sealed class TestingMessageCollectionResource : RestResource, ITestingMessageCollectionResource
{
    private const string LastIdQueryParameter = "last_id";
    private const string PageQueryParameter = "page";
    private const string SearchFilterQueryParameter = "search";


    public TestingMessageCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<TestingMessage>> Fetch(
        TestingMessageFilter? filter = null,
        CancellationToken cancellationToken = default)
        => await GetList<TestingMessage>(CreateFetchUri(filter), cancellationToken).ConfigureAwait(false);


    private Uri CreateFetchUri(TestingMessageFilter? filter)
    {
        var uri = ResourceUri;

        if (filter?.LastId is not null)
        {
            var page = filter.LastId.Value.ToUriSegment();
            uri = uri.AppendQueryParameter(LastIdQueryParameter, page);
        }

        if (filter?.Page is not null)
        {
            var page = filter.Page.Value.ToUriSegment();
            uri = uri.AppendQueryParameter(PageQueryParameter, page);
        }

        if (!string.IsNullOrEmpty(filter?.SearchFilter))
        {
            var searchFilter = filter?.SearchFilter;
            uri = uri.AppendQueryParameter(SearchFilterQueryParameter, searchFilter);
        }

        return uri;
    }
}
