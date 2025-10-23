namespace Mailtrap.ContactExports;

/// <summary>
/// Implementation of Contact Export API operations.
/// </summary>
internal sealed class ContactExportResource : RestResource, IContactExportResource
{
    public ContactExportResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }

    public async Task<ContactExport> GetDetails(CancellationToken cancellationToken = default)
        => await Get<ContactExport>(cancellationToken).ConfigureAwait(false);
}
