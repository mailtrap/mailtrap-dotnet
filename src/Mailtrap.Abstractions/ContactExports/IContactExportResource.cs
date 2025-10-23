namespace Mailtrap.ContactExports;

/// <summary>
/// Represents contact export resource.
/// </summary>
public interface IContactExportResource : IRestResource
{
    /// <summary>
    /// Gets details of the contact export, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Requested contact export details.
    /// </returns>
    public Task<ContactExport> GetDetails(CancellationToken cancellationToken = default);
}
