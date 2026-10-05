namespace Mailtrap.Inbound;


internal sealed class InboundThreadCollectionResource : RestResource, IInboundThreadCollectionResource
{
    private const string LastIdParameter = "last_id";
    private const string SearchParameter = "search";


    public InboundThreadCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public Task<InboundThreadsListResponse> List(string? lastId = null, CancellationToken cancellationToken = default)
        => List(lastId, null, cancellationToken);

    public Task<InboundThreadsListResponse> List(string? lastId, string? search, CancellationToken cancellationToken = default)
    {
        var parameters = new List<KeyValuePair<string, string>>();

        if (!string.IsNullOrEmpty(lastId))
        {
            parameters.Add(new KeyValuePair<string, string>(LastIdParameter, lastId!));
        }

        if (!string.IsNullOrEmpty(search))
        {
            parameters.Add(new KeyValuePair<string, string>(SearchParameter, search!));
        }

        var uri = parameters.Count == 0
            ? ResourceUri
            : ResourceUri.AppendQueryParameters(parameters);

        return RestResourceCommandFactory
            .CreateGet<InboundThreadsListResponse>(uri)
            .Execute(cancellationToken);
    }
}
