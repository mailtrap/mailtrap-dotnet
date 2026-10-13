namespace Mailtrap.Marketing;


internal sealed class MarketingStatsResource : RestResource, IMarketingStatsResource
{
    private const string StartDateParam = "start_date";
    private const string EndDateParam = "end_date";
    private const string SendingDomainIdsParam = "sending_domain_ids[]";
    private const string SourcesParam = "sources[]";
    private const string EmailServiceProvidersParam = "email_service_providers[]";


    public MarketingStatsResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<MarketingStats> GetStats(MarketingStatsFilter filter, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(filter, nameof(filter));

        var uri = BuildFilterUri(filter);

        var response = await RestResourceCommandFactory
            .CreateGet<MarketingStatsResponseDto>(uri)
            .Execute(cancellationToken)
            .ConfigureAwait(false);

        return response.Stats;
    }


    private Uri BuildFilterUri(MarketingStatsFilter filter)
    {
        Ensure.NotNullOrEmpty(filter.StartDate, nameof(filter.StartDate));
        Ensure.NotNullOrEmpty(filter.EndDate, nameof(filter.EndDate));

        var uri = ResourceUri
            .AppendQueryParameter(StartDateParam, filter.StartDate)
            .AppendQueryParameter(EndDateParam, filter.EndDate);

        foreach (var id in filter.SendingDomainIds)
        {
            uri = uri.AppendQueryParameter(SendingDomainIdsParam, id.ToString(CultureInfo.InvariantCulture));
        }

        foreach (var source in filter.Sources)
        {
            uri = uri.AppendQueryParameter(SourcesParam, source);
        }

        foreach (var provider in filter.EmailServiceProviders)
        {
            uri = uri.AppendQueryParameter(EmailServiceProvidersParam, provider);
        }

        return uri;
    }
}
