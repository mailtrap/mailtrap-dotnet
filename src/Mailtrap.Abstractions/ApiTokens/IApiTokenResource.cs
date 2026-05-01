namespace Mailtrap.ApiTokens;


/// <summary>
/// Represents a single API token resource.
/// </summary>
public interface IApiTokenResource : IRestResource
{
    /// <summary>
    /// Get details of the API token represented by this resource.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// API token details.
    /// </returns>
    public Task<ApiToken> GetDetails(CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently delete the API token represented by this resource.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    public Task Delete(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reset the API token represented by this resource.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// New API token details, including the full token value.
    /// </returns>
    ///
    /// <remarks>
    /// Expires the requested token and creates a new token with the same permissions.
    /// The old token stops working after a short grace period. The response includes
    /// the new token value — store it securely; it is only returned once.
    /// </remarks>
    public Task<ApiTokenResetResponse> Reset(CancellationToken cancellationToken = default);
}
