namespace Mailtrap.Inbound;


/// <summary>
/// Represents the inbound thread collection resource for an inbox.
/// </summary>
public interface IInboundThreadCollectionResource : IRestResource
{
    /// <summary>
    /// Lists conversation threads in the inbox, with cursor-based pagination.
    /// </summary>
    ///
    /// <param name="lastId">
    /// Optional pagination cursor from a previous response's <c>last_id</c>.
    /// <see langword="null"/> for the first page.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// A page of threads, with total count and next-page cursor.
    /// </returns>
    public Task<InboundThreadsListResponse> List(string? lastId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists conversation threads in the inbox matching <paramref name="search"/>, with cursor-based pagination.
    /// </summary>
    ///
    /// <param name="lastId">
    /// Optional pagination cursor from a previous response's <c>last_id</c>.
    /// </param>
    /// <param name="search">
    /// Optional search term.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// A page of matching threads, with total count and next-page cursor.
    /// </returns>
    public Task<InboundThreadsListResponse> List(string? lastId, string? search, CancellationToken cancellationToken = default);
}
