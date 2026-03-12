namespace Mailtrap.EmailLogs;


internal sealed class EmailLogResource : RestResource, IEmailLogResource
{
    public EmailLogResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<EmailLogMessage> GetDetails(CancellationToken cancellationToken = default)
        => await Get<EmailLogMessage>(cancellationToken).ConfigureAwait(false);
}