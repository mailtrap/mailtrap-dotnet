namespace Mailtrap.EmailLogs;


/// <summary>
/// Represents a single email log message resource.
/// </summary>
public interface IEmailLogResource : IRestResource
{
    /// <summary>
    /// Gets the email log message details including events and raw message URL.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// The sending message with events and raw_message_url.
    /// </returns>
    public Task<EmailLogMessage> GetDetails(CancellationToken cancellationToken = default);
}
