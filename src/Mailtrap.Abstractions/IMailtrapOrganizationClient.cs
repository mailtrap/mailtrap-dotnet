namespace Mailtrap;


/// <summary>
/// Mailtrap organization-scoped API client.
/// </summary>
///
/// <remarks>
/// Exposes organization-level operations such as managing sub accounts.
/// Organization-scoped endpoints live under <c>/api/organizations/{organization_id}/...</c>
/// and require token permissions for the targeted organization.
/// </remarks>
public interface IMailtrapOrganizationClient : IRestResource
{
    /// <summary>
    /// Gets resource for specific organization, identified by <paramref name="organizationId"/>.
    /// </summary>
    ///
    /// <param name="organizationId">
    /// ID of organization to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the organization with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="organizationId"/> is less than or equal to zero.
    /// </exception>
    public IOrganizationResource Organization(long organizationId);
}
