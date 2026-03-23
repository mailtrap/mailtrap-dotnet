namespace Mailtrap.Stats;


internal sealed class StatsResource : RestResource, IStatsResource
{
    private const string DomainsSegment = "domains";
    private const string CategoriesSegment = "categories";
    private const string EmailServiceProvidersSegment = "email_service_providers";
    private const string DateSegment = "date";

    private const string StartDateParam = "start_date";
    private const string EndDateParam = "end_date";
    private const string SendingDomainIdsParam = "sending_domain_ids[]";
    private const string SendingStreamsParam = "sending_streams[]";
    private const string CategoriesParam = "categories[]";
    private const string EmailServiceProvidersParam = "email_service_providers[]";

    private static readonly Dictionary<string, string> s_groupKeys = new()
    {
        [DomainsSegment] = "sending_domain_id",
        [CategoriesSegment] = "category",
        [EmailServiceProvidersSegment] = "email_service_provider",
        [DateSegment] = "date"
    };


    public StatsResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<SendingStats> GetStats(StatsFilter filter, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(filter, nameof(filter));

        var uri = BuildFilterUri(ResourceUri, filter);
        return await RestResourceCommandFactory
            .CreateGet<SendingStats>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IList<SendingStatGroup>> ByDomain(StatsFilter filter, CancellationToken cancellationToken = default)
        => await GetGroupedStats(DomainsSegment, filter, cancellationToken).ConfigureAwait(false);

    public async Task<IList<SendingStatGroup>> ByCategory(StatsFilter filter, CancellationToken cancellationToken = default)
        => await GetGroupedStats(CategoriesSegment, filter, cancellationToken).ConfigureAwait(false);

    public async Task<IList<SendingStatGroup>> ByEmailServiceProvider(StatsFilter filter, CancellationToken cancellationToken = default)
        => await GetGroupedStats(EmailServiceProvidersSegment, filter, cancellationToken).ConfigureAwait(false);

    public async Task<IList<SendingStatGroup>> ByDate(StatsFilter filter, CancellationToken cancellationToken = default)
        => await GetGroupedStats(DateSegment, filter, cancellationToken).ConfigureAwait(false);


    private async Task<IList<SendingStatGroup>> GetGroupedStats(
        string group, StatsFilter filter, CancellationToken cancellationToken)
    {
        Ensure.NotNull(filter, nameof(filter));

        var uri = BuildFilterUri(ResourceUri.Append(group), filter);
        var response = await RestResourceCommandFactory
            .CreateGet<IList<JsonElement>>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);
        var groupKey = s_groupKeys[group];

        return response.Select(item =>
        {
            var groupProp = item.GetProperty(groupKey);
            return new SendingStatGroup
            {
                Name = groupKey,
                Value = groupProp.ValueKind == JsonValueKind.Number
                    ? groupProp.GetInt64()
                    : groupProp.GetString() ?? string.Empty,
                Stats = item.TryGetProperty("stats", out var statsProp)
                    ? statsProp.Deserialize<SendingStats>() ?? new SendingStats()
                    : new SendingStats()
            };
        }).ToList<SendingStatGroup>();
    }

    private static Uri BuildFilterUri(Uri baseUri, StatsFilter filter)
    {
        Ensure.NotNullOrEmpty(filter.StartDate, nameof(filter.StartDate));
        Ensure.NotNullOrEmpty(filter.EndDate, nameof(filter.EndDate));

        var uri = baseUri
            .AppendQueryParameter(StartDateParam, filter.StartDate)
            .AppendQueryParameter(EndDateParam, filter.EndDate);

        foreach (var id in filter.SendingDomainIds)
        {
            uri = uri.AppendQueryParameter(SendingDomainIdsParam, id.ToString(CultureInfo.InvariantCulture));
        }

        foreach (var stream in filter.SendingStreams)
        {
            uri = uri.AppendQueryParameter(SendingStreamsParam, stream);
        }

        foreach (var category in filter.Categories)
        {
            uri = uri.AppendQueryParameter(CategoriesParam, category);
        }

        foreach (var provider in filter.EmailServiceProviders)
        {
            uri = uri.AppendQueryParameter(EmailServiceProvidersParam, provider);
        }

        return uri;
    }
}
