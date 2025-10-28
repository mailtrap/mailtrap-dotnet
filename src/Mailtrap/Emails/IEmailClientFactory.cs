namespace Mailtrap.Emails;


/// <summary>
/// Factory to spawn instances of <see cref="IEmailClient{TRequest, TResponse}"/>.
/// </summary>
internal interface IEmailClientFactory
{
    public ISendEmailClient Create(bool isBulk = false, long? inboxId = default);
    public ISendEmailClient CreateDefault();
    public ISendEmailClient CreateTransactional();
    public ISendEmailClient CreateBulk();
    public ISendEmailClient CreateTest(long inboxId);

    public IBatchEmailClient CreateBatch(bool isBulk = false, long? inboxId = default);
    public IBatchEmailClient CreateBatchDefault();
    public IBatchEmailClient CreateBatchTransactional();
    public IBatchEmailClient CreateBatchBulk();
    public IBatchEmailClient CreateBatchTest(long inboxId);
}
