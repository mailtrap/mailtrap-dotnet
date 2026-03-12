namespace Mailtrap.EmailLogs;


internal sealed class EmailLogCollectionResource : RestResource, IEmailLogCollectionResource
{
    private const string SearchAfterParameter = "search_after";


    public EmailLogCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<EmailLogsListResponse> List(
        EmailLogsListFilter? filter = null,
        string? searchAfter = null,
        CancellationToken cancellationToken = default)
    {
        var uri = CreateListUri(filter, searchAfter);
        return await RestResourceCommandFactory
            .CreateGet<EmailLogsListResponse>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);
    }


    private Uri CreateListUri(EmailLogsListFilter? filter, string? searchAfter)
    {
        var parameters = new List<KeyValuePair<string, string>>();

        if (!string.IsNullOrEmpty(searchAfter))
        {
            parameters.Add(new KeyValuePair<string, string>(SearchAfterParameter, searchAfter!));
        }

        if (filter is not null)
        {
            foreach (var pair in filter.ToQueryParameters())
            {
                parameters.Add(pair);
            }
        }

        return parameters.Count == 0
            ? ResourceUri
            : ResourceUri.AppendQueryParameters(parameters);
    }
}
