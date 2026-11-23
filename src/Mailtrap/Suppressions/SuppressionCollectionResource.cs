namespace Mailtrap.Suppressions;


internal sealed class SuppressionCollectionResource : RestResource, ISuppressionCollectionResource
{
    private const string EmailQueryParameter = "email";
    private const string StartTimeQueryParameter = "start_time";
    private const string EndTimeQueryParameter = "end_time";
    private const string LastIdQueryParameter = "last_id";


    public SuppressionCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<Suppression>> Fetch(SuppressionFilter? filter = null, CancellationToken cancellationToken = default)
        => await GetList<Suppression>(CreateFetchUri(filter), cancellationToken).ConfigureAwait(false);

    public async Task<Suppression> Create(CreateSuppressionRequest request, CancellationToken cancellationToken = default)
    {
        var response = await Create<CreateSuppressionRequest, SuppressionResponseDto>(request, cancellationToken).ConfigureAwait(false);

        return response.Suppression;
    }


    private Uri CreateFetchUri(SuppressionFilter? filter)
    {
        if (filter is null)
        {
            return ResourceUri;
        }

        var result = ResourceUri;

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            result = result.AppendQueryParameter(EmailQueryParameter, filter.Email);
        }

        if (filter.StartTime is not null)
        {
            result = result.AppendQueryParameter(StartTimeQueryParameter, filter.StartTime.Value.ToString("O"));
        }

        if (filter.EndTime is not null)
        {
            result = result.AppendQueryParameter(EndTimeQueryParameter, filter.EndTime.Value.ToString("O"));
        }

        if (!string.IsNullOrWhiteSpace(filter.LastId))
        {
            result = result.AppendQueryParameter(LastIdQueryParameter, filter.LastId);
        }

        return result;
    }
}
