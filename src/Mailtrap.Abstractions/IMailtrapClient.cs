namespace Mailtrap;


/// <summary>
/// Mailtrap API client.
/// </summary>
public interface IMailtrapClient : IRestResource
{
    #region Account

    /// <summary>
    /// Gets account collection resource.
    /// </summary>
    /// 
    /// <returns>
    /// Account collection resource.
    /// </returns>
    public IAccountCollectionResource Accounts();

    /// <summary>
    /// Gets resource for specific account, identified by <paramref name="accountId"/>.
    /// </summary>
    ///
    /// <param name="accountId">
    /// ID of account to get resource for.
    /// </param>
    /// 
    /// <returns>
    /// Resource for the account with specified ID.
    /// </returns>
    public IAccountResource Account(long accountId);

    #endregion



    #region Inbound

    /// <summary>
    /// Gets the token-scoped root of the Inbound Email API.
    /// </summary>
    ///
    /// <returns>
    /// Inbound Email API root resource.
    /// </returns>
    public IInboundResource Inbound();

    #endregion



    #region Email Campaigns

    /// <summary>
    /// Gets the token-scoped email campaign collection resource.
    /// </summary>
    ///
    /// <returns>
    /// Email campaign collection resource.
    /// </returns>
    public IEmailCampaignCollectionResource EmailCampaigns();

    /// <summary>
    /// Gets resource for specific email campaign, identified by <paramref name="emailCampaignId"/>.
    /// </summary>
    ///
    /// <param name="emailCampaignId">
    /// ID of email campaign to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the email campaign with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="emailCampaignId"/> is less than or equal to zero.
    /// </exception>
    public IEmailCampaignResource EmailCampaign(long emailCampaignId);

    #endregion



    #region Marketing

    /// <summary>
    /// Gets the token-scoped Email Marketing statistics resource.
    /// </summary>
    ///
    /// <returns>
    /// Email Marketing statistics resource.
    /// </returns>
    public IMarketingStatsResource MarketingStats();

    #endregion



    #region Company Info

    /// <summary>
    /// Gets company info resource for the sending domain, identified by <paramref name="domainId"/>.
    /// </summary>
    ///
    /// <param name="domainId">
    /// ID of the sending domain to get the company info resource for.
    /// </param>
    ///
    /// <returns>
    /// Company info resource for the sending domain with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="domainId"/> is less than or equal to zero.
    /// </exception>
    public ICompanyInfoResource CompanyInfo(long domainId);

    #endregion



    #region Tracking Opt-outs

    /// <summary>
    /// Gets the tracking opt-out collection resource.
    /// </summary>
    ///
    /// <returns>
    /// Tracking opt-out collection resource.
    /// </returns>
    public ITrackingOptOutCollectionResource TrackingOptOuts();

    /// <summary>
    /// Gets resource for specific tracking opt-out, identified by <paramref name="trackingOptOutId"/>.
    /// </summary>
    ///
    /// <param name="trackingOptOutId">
    /// ID of the tracking opt-out to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the tracking opt-out with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentException">
    /// When <paramref name="trackingOptOutId"/> is <see langword="null"/> or empty.
    /// </exception>
    public ITrackingOptOutResource TrackingOptOut(string trackingOptOutId);

    #endregion



    #region Regular Emails

    /// <summary>
    /// <para>
    /// Gets default email client.
    /// </para>
    /// <para>
    /// Type of the client (transactional, bulk or test) is defined by configuration.
    /// </para>
    /// </summary>
    /// 
    /// <returns>
    /// <see cref="ISendEmailClient"/> instance that can be used to send emails to the API, specified by configuration.
    /// </returns>
    public ISendEmailClient Email();

    /// <summary>
    /// Factory method to create transactional email client.
    /// </summary>
    ///
    /// <returns>
    /// New <see cref="ISendEmailClient"/> instance that can be used to send transactional emails.
    /// </returns>
    public ISendEmailClient Transactional();

    /// <summary>
    /// Factory method to create bulk email client.
    /// </summary>
    /// 
    /// <returns>
    /// New <see cref="ISendEmailClient"/> instance that can be used to send bulk emails.
    /// </returns>
    public ISendEmailClient Bulk();

    /// <summary>
    /// Factory method to create test email client.
    /// </summary>
    ///
    /// <param name="inboxId">
    /// ID of the inbox to send test emails to.
    /// </param>
    ///
    /// <returns>
    /// New <see cref="ISendEmailClient"/> instance that can be used to send test emails to the specified <paramref name="inboxId"/>.
    /// </returns>
    public ISendEmailClient Test(long inboxId);

    #endregion



    #region Batch Emails

    /// <summary>
    /// <para>
    /// Gets default batch email client.
    /// </para>
    /// <para>
    /// Type of the client (transactional, bulk or test) is defined by configuration.
    /// </para>
    /// </summary>
    /// 
    /// <returns>
    /// <see cref="IBatchEmailClient"/> instance that can be used to send batch emails to the API, specified by configuration.
    /// </returns>
    public IBatchEmailClient BatchEmail();

    /// <summary>
    /// Factory method to create batch transactional email client.
    /// </summary>
    ///
    /// <returns>
    /// New <see cref="IBatchEmailClient"/> instance that can be used to send transactional emails in a batch.
    /// </returns>
    public IBatchEmailClient BatchTransactional();

    /// <summary>
    /// Factory method to create batch bulk email client.
    /// </summary>
    /// 
    /// <returns>
    /// New <see cref="IBatchEmailClient"/> instance that can be used to send bulk emails in a batch.
    /// </returns>
    public IBatchEmailClient BatchBulk();

    /// <summary>
    /// Factory method to create batch test email client.
    /// </summary>
    ///
    /// <param name="inboxId">
    /// ID of the inbox to send test emails to.
    /// </param>
    ///
    /// <returns>
    /// New <see cref="IBatchEmailClient"/> instance that can be used to send test emails to the specified <paramref name="inboxId"/> in a batch.
    /// </returns>
    public IBatchEmailClient BatchTest(long inboxId);

    #endregion
}
