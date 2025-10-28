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
