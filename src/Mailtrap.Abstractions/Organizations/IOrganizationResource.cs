namespace Mailtrap.Organizations;


/// <summary>
/// Represents organization resource.
/// </summary>
public interface IOrganizationResource : IRestResource
{
    /// <summary>
    /// Gets sub account collection resource for the organization, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Sub account collection resource for the organization, represented by this resource instance.
    /// </returns>
    public IOrganizationSubAccountCollectionResource SubAccounts();

    /// <summary>
    /// Gets resource for specific sub account, identified by <paramref name="subAccountId"/>.
    /// </summary>
    ///
    /// <param name="subAccountId">
    /// ID of sub account to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the sub account with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="subAccountId"/> is less than or equal to zero.
    /// </exception>
    public IOrganizationSubAccountResource SubAccount(long subAccountId);
}
