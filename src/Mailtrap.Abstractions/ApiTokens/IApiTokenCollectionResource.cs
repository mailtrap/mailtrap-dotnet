namespace Mailtrap.ApiTokens;


/// <summary>
/// Represents API token collection resource.
/// </summary>
public interface IApiTokenCollectionResource : IRestResource
{
    /// <summary>
    /// List all API tokens visible to the current API token.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of API token details.
    /// </returns>
    public Task<IList<ApiToken>> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new API token.
    /// </summary>
    ///
    /// <param name="request">
    /// API token creation request.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Created API token details, including the full token value.
    /// </returns>
    ///
    /// <remarks>
    /// The full token value is only returned once at creation — store it securely.
    /// </remarks>
    public Task<CreateApiTokenResponse> Create(CreateApiTokenRequest request, CancellationToken cancellationToken = default);
}
