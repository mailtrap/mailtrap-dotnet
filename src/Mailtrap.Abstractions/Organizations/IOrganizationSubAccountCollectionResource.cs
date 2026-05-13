namespace Mailtrap.Organizations;


/// <summary>
/// Represents organization sub account collection resource.
/// </summary>
public interface IOrganizationSubAccountCollectionResource : IRestResource
{
    /// <summary>
    /// List sub accounts of the organization.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Collection of sub accounts.
    /// </returns>
    public Task<IList<SubAccount>> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new sub account under the organization.
    /// </summary>
    ///
    /// <param name="request">
    /// Sub account creation request.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Created sub account details.
    /// </returns>
    public Task<SubAccount> Create(CreateSubAccountRequest request, CancellationToken cancellationToken = default);
}
