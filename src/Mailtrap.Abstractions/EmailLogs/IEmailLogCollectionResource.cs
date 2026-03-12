namespace Mailtrap.EmailLogs;


/// <summary>
/// Represents the email logs collection resource for an account.
/// </summary>
public interface IEmailLogCollectionResource : IRestResource
{
    /// <summary>
    /// Lists email logs with optional filters and cursor-based pagination.
    /// </summary>
    ///
    /// <param name="filter">
    /// Optional filter. When null, no filter query params are added.
    /// </param>
    /// <param name="searchAfter">
    /// Optional cursor (message_id UUID from previous response's next_page_cursor) for the next page.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Response with messages, total count, and next page cursor.
    /// </returns>
    public Task<EmailLogsListResponse> List(
        EmailLogsListFilter? filter = null,
        string? searchAfter = null,
        CancellationToken cancellationToken = default);
}
