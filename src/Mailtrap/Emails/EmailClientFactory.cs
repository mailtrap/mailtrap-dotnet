namespace Mailtrap.Emails;


/// <summary>
/// <see cref="IEmailClientFactory"/> implementation.
/// </summary>
internal sealed class EmailClientFactory : IEmailClientFactory
{
    private readonly MailtrapClientOptions _clientConfiguration;
    private readonly IEmailClientEndpointProvider _emailClientEndpointProvider;
    private readonly IRestResourceCommandFactory _restResourceCommandFactory;


    public EmailClientFactory(
        IOptions<MailtrapClientOptions> clientOptions,
        IEmailClientEndpointProvider emailClientEndpointProvider,
        IRestResourceCommandFactory restResourceCommandFactory)
    {
        Ensure.NotNull(clientOptions, nameof(clientOptions));
        Ensure.NotNull(emailClientEndpointProvider, nameof(emailClientEndpointProvider));
        Ensure.NotNull(restResourceCommandFactory, nameof(restResourceCommandFactory));

        _clientConfiguration = clientOptions.Value;
        _emailClientEndpointProvider = emailClientEndpointProvider;
        _restResourceCommandFactory = restResourceCommandFactory;
    }


    public ISendEmailClient Create(bool isBulk = false, long? sandboxId = default)
    {
        var sendUri = _emailClientEndpointProvider.GetRequestUri(false, isBulk, sandboxId);

        return new SendEmailClient(_restResourceCommandFactory, sendUri);
    }

    public ISendEmailClient CreateDefault() => Create(_clientConfiguration.UseBulkApi, _clientConfiguration.SandboxId);

    public ISendEmailClient CreateTransactional() => Create();

    public ISendEmailClient CreateBulk() => Create(isBulk: true);

    public ISendEmailClient CreateSandbox(long sandboxId) => Create(sandboxId: sandboxId);


    public IBatchEmailClient CreateBatch(bool isBulk = false, long? sandboxId = default)
    {
        var batchUri = _emailClientEndpointProvider.GetRequestUri(true, isBulk, sandboxId);

        return new BatchEmailClient(_restResourceCommandFactory, batchUri);
    }

    public IBatchEmailClient CreateBatchDefault() => CreateBatch(_clientConfiguration.UseBulkApi, _clientConfiguration.SandboxId);

    public IBatchEmailClient CreateBatchTransactional() => CreateBatch();

    public IBatchEmailClient CreateBatchBulk() => CreateBatch(isBulk: true);

    public IBatchEmailClient CreateBatchSandbox(long sandboxId) => CreateBatch(sandboxId: sandboxId);
}
