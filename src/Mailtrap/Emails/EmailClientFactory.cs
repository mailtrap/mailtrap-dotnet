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


    public ISendEmailClient Create(bool isBulk = false, long? inboxId = default)
    {
        var sendUri = _emailClientEndpointProvider.GetRequestUri(false, isBulk, inboxId);

        return new SendEmailClient(_restResourceCommandFactory, sendUri);
    }

    public ISendEmailClient CreateDefault() => Create(_clientConfiguration.UseBulkApi, _clientConfiguration.InboxId);

    public ISendEmailClient CreateTransactional() => Create();

    public ISendEmailClient CreateBulk() => Create(isBulk: true);

    public ISendEmailClient CreateTest(long inboxId) => Create(inboxId: inboxId);


    public IBatchEmailClient CreateBatch(bool isBulk = false, long? inboxId = default)
    {
        var batchUri = _emailClientEndpointProvider.GetRequestUri(true, isBulk, inboxId);

        return new BatchEmailClient(_restResourceCommandFactory, batchUri);
    }

    public IBatchEmailClient CreateBatchDefault() => CreateBatch(_clientConfiguration.UseBulkApi, _clientConfiguration.InboxId);

    public IBatchEmailClient CreateBatchTransactional() => CreateBatch();

    public IBatchEmailClient CreateBatchBulk() => CreateBatch(isBulk: true);

    public IBatchEmailClient CreateBatchTest(long inboxId) => CreateBatch(inboxId: inboxId);
}
