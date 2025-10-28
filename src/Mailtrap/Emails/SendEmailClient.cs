namespace Mailtrap.Emails;


/// <summary>
/// Client for sending emails through Mailtrap's API. Implements <see cref="ISendEmailClient"/>.
/// </summary>
internal sealed class SendEmailClient : EmailClient<SendEmailRequest, SendEmailResponse>, ISendEmailClient
{
    /// <inheritdoc />
    public SendEmailClient(IRestResourceCommandFactory restResourceCommandFactory, Uri sendUri)
        : base(restResourceCommandFactory, sendUri) { }
}
