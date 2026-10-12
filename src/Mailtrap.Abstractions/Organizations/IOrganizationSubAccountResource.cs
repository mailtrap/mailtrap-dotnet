namespace Mailtrap.Organizations;


/// <summary>
/// Represents organization sub account resource.
/// </summary>
public interface IOrganizationSubAccountResource : IRestResource
{
    /// <summary>
    /// Deletes the sub account, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Nothing is returned upon successful deletion.
    /// </returns>
    ///
    /// <remarks>
    /// <para>
    /// Requires sub account management permissions for the organization.
    /// </para>
    /// <para>
    /// Deletion is permanent and removes all data of the sub account.<br />
    /// Deleting the last sub account of the organization deletes the organization as well.
    /// </para>
    /// <para>
    /// After deletion the sub account is no longer available, so a repeated call results in a 404 (Not Found) error.
    /// </para>
    /// <para>
    /// The operation is rate limited to 10 requests per minute per organization.
    /// </para>
    /// </remarks>
    public Task Delete(CancellationToken cancellationToken = default);
}
