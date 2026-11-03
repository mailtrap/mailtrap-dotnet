namespace Mailtrap.Emails;


/// <summary>
/// Factory to spawn instances of <see cref="IEmailClient{TRequest, TResponse}"/>.
/// </summary>
internal interface IEmailClientFactory
{
    public ISendEmailClient Create(bool isBulk = false, long? sandboxId = default);
    public ISendEmailClient CreateDefault();
    public ISendEmailClient CreateTransactional();
    public ISendEmailClient CreateBulk();
    public ISendEmailClient CreateSandbox(long sandboxId);

    public IBatchEmailClient CreateBatch(bool isBulk = false, long? sandboxId = default);
    public IBatchEmailClient CreateBatchDefault();
    public IBatchEmailClient CreateBatchTransactional();
    public IBatchEmailClient CreateBatchBulk();
    public IBatchEmailClient CreateBatchSandbox(long sandboxId);
}
