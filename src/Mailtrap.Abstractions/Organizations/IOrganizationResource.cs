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
}
