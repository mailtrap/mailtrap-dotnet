namespace Mailtrap.Emails;


/// <summary>
/// <see cref="IBatchEmailClient"/> implementation.
/// </summary>
internal sealed class BatchEmailClient : EmailClient<BatchEmailRequest, BatchEmailResponse>, IBatchEmailClient
{
    /// <inheritdoc />
    public BatchEmailClient(IRestResourceCommandFactory restResourceCommandFactory, Uri batchUri)
        : base(restResourceCommandFactory, batchUri) { }
}
