namespace Mailtrap.Emails;


/// <summary>
/// Provider to get request URIs for <see cref="EmailClient{TRequest, TResponse}"/>.
/// </summary>
internal interface IEmailClientEndpointProvider
{
    public Uri GetRequestUri(bool isBatch, bool isBulk, long? inboxId);
}
