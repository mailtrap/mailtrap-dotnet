namespace Mailtrap.ContactExports;

/// <summary>
/// Represents contact export collection resource.
/// </summary>
public interface IContactExportCollectionResource : IRestResource
{
    /// <summary>
    /// Creates a new contact export with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing contact export details.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// <see cref="ContactExport"/> containing the export Id and status.
    /// </returns>
    public Task<ContactExport> Create(CreateContactExportRequest request, CancellationToken cancellationToken = default);
}
