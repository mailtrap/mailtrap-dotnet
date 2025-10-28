namespace Mailtrap.ContactExports;

/// <summary>
/// Implementation of Contact Export Collection resource.
/// </summary>
internal sealed class ContactExportCollectionResource : RestResource, IContactExportCollectionResource
{
    public ContactExportCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }

    public async Task<ContactExport> Create(CreateContactExportRequest request, CancellationToken cancellationToken = default)
        => await Create<CreateContactExportRequest, ContactExport>(request, cancellationToken).ConfigureAwait(false);
}
