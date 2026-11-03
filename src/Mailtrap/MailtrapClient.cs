namespace Mailtrap;


/// <summary>
/// <see cref="IMailtrapClient"/> implementation.
/// </summary>
internal sealed class MailtrapClient : RestResource, IMailtrapClient
{
    private const string AccountsSegment = "accounts";

    private readonly IEmailClientFactory _emailClientFactory;
    private readonly ISendEmailClient _defaultSendEmailClient;
    private readonly IBatchEmailClient _defaultBatchEmailClient;


    /// <summary>
    /// Default instance constructor.
    /// </summary>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="emailClientFactory"/> is <see langword="null"/>.
    /// </exception>
    public MailtrapClient(IEmailClientFactory emailClientFactory, IRestResourceCommandFactory restResourceCommandFactory)
        : base(restResourceCommandFactory, Endpoints.ApiDefaultUrl.Append(UrlSegments.ApiRootSegment))
    {
        Ensure.NotNull(emailClientFactory, nameof(emailClientFactory));

        _emailClientFactory = emailClientFactory;
        _defaultSendEmailClient = emailClientFactory.CreateDefault();
        _defaultBatchEmailClient = emailClientFactory.CreateBatchDefault();
    }



    #region Account

    /// <inheritdoc/>
    public IAccountCollectionResource Accounts()
        => new AccountCollectionResource(RestResourceCommandFactory, ResourceUri.Append(AccountsSegment));

    /// <inheritdoc/>
    public IAccountResource Account(long accountId)
        => new AccountResource(RestResourceCommandFactory, ResourceUri.Append(AccountsSegment).Append(accountId));

    #endregion



    #region Inbound

    /// <inheritdoc/>
    public IInboundResource Inbound()
        => new InboundResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.InboundSegment));

    #endregion



    #region Email Campaigns

    /// <inheritdoc/>
    public IEmailCampaignCollectionResource EmailCampaigns()
        => new EmailCampaignCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.EmailCampaignsSegment));

    /// <inheritdoc/>
    public IEmailCampaignResource EmailCampaign(long emailCampaignId)
    {
        Ensure.GreaterThanZero(emailCampaignId, nameof(emailCampaignId));

        return new EmailCampaignResource(
            RestResourceCommandFactory,
            ResourceUri.Append(UrlSegments.EmailCampaignsSegment).Append(emailCampaignId));
    }

    #endregion



    #region Marketing

    /// <inheritdoc/>
    public IMarketingStatsResource MarketingStats()
        => new MarketingStatsResource(
            RestResourceCommandFactory,
            ResourceUri.Append(UrlSegments.MarketingSegment, UrlSegments.StatsSegment));

    #endregion



    #region Company Info

    /// <inheritdoc/>
    public ICompanyInfoResource CompanyInfo(long domainId)
    {
        Ensure.GreaterThanZero(domainId, nameof(domainId));

        return new CompanyInfoResource(
            RestResourceCommandFactory,
            ResourceUri
                .Append(UrlSegments.DomainsSegment)
                .Append(domainId)
                .Append(UrlSegments.CompanyInfoSegment));
    }

    #endregion



    #region Tracking Opt-outs

    /// <inheritdoc/>
    public ITrackingOptOutCollectionResource TrackingOptOuts()
        => new TrackingOptOutCollectionResource(
            RestResourceCommandFactory,
            ResourceUri.Append(UrlSegments.TrackingOptOutsSegment));

    /// <inheritdoc/>
    public ITrackingOptOutResource TrackingOptOut(string trackingOptOutId)
    {
        Ensure.NotNullOrEmpty(trackingOptOutId, nameof(trackingOptOutId));

        return new TrackingOptOutResource(
            RestResourceCommandFactory,
            ResourceUri
                .Append(UrlSegments.TrackingOptOutsSegment)
                .Append(trackingOptOutId));
    }

    #endregion



    #region Regular Emails

    /// <inheritdoc/>
    public ISendEmailClient Email() => _defaultSendEmailClient;

    /// <inheritdoc/>
    public ISendEmailClient Transactional() => _emailClientFactory.CreateTransactional();

    /// <inheritdoc/>
    public ISendEmailClient Bulk() => _emailClientFactory.CreateBulk();

    /// <inheritdoc/>
    public ISendEmailClient Sandbox(long sandboxId) => _emailClientFactory.CreateTest(sandboxId);

    /// <inheritdoc/>
    [Obsolete("Test(long) is deprecated and will be removed in a future major release; use Sandbox(long) instead.")]
    public ISendEmailClient Test(long inboxId) => Sandbox(inboxId);

    #endregion



    #region Batch Emails

    /// <inheritdoc/>
    public IBatchEmailClient BatchEmail() => _defaultBatchEmailClient;

    /// <inheritdoc/>
    public IBatchEmailClient BatchTransactional() => _emailClientFactory.CreateBatchTransactional();

    /// <inheritdoc/>
    public IBatchEmailClient BatchBulk() => _emailClientFactory.CreateBatchBulk();

    /// <inheritdoc/>
    public IBatchEmailClient BatchSandbox(long sandboxId) => _emailClientFactory.CreateBatchTest(sandboxId);

    /// <inheritdoc/>
    [Obsolete("BatchTest(long) is deprecated and will be removed in a future major release; use BatchSandbox(long) instead.")]
    public IBatchEmailClient BatchTest(long inboxId) => BatchSandbox(inboxId);

    #endregion
}
