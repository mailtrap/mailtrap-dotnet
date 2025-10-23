namespace Mailtrap.Contacts;

/// <summary>
/// Implementation of Contact Collection resource.
/// </summary>
internal sealed class ContactCollectionResource : RestResource, IContactCollectionResource
{
    private const string ExportsSegment = "exports";
    private const string ImportsSegment = "imports";
    private const string ListsSegment = "lists";
    private const string FieldsSegment = "fields";
    private const string EventsSegment = "events";

    public ContactCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }

    public IContactExportCollectionResource Exports()
        => new ContactExportCollectionResource(RestResourceCommandFactory, ResourceUri.Append(ExportsSegment));

    public IContactExportResource Export(long exportId)
            => new ContactExportResource(RestResourceCommandFactory, ResourceUri.Append(ExportsSegment).Append(exportId));

    public IContactImportCollectionResource Imports()
        => new ContactImportCollectionResource(RestResourceCommandFactory, ResourceUri.Append(ImportsSegment));

    public IContactImportResource Import(long importId)
            => new ContactImportResource(RestResourceCommandFactory, ResourceUri.Append(ImportsSegment).Append(importId));

    public IContactListCollectionResource Lists()
        => new ContactListCollectionResource(RestResourceCommandFactory, ResourceUri.Append(ListsSegment));

    public IContactListResource List(long listId)
            => new ContactListResource(RestResourceCommandFactory, ResourceUri.Append(ListsSegment).Append(listId));

    public IContactFieldCollectionResource Fields()
        => new ContactFieldCollectionResource(RestResourceCommandFactory, ResourceUri.Append(FieldsSegment));

    public IContactFieldResource Field(long fieldId)
            => new ContactFieldResource(RestResourceCommandFactory, ResourceUri.Append(FieldsSegment).Append(fieldId));

    public IContactEventCollectionResource Events(string contactId)
        => new ContactEventCollectionResource(RestResourceCommandFactory, ResourceUri.Append(contactId).Append(EventsSegment));

    public async Task<IList<Contact>> GetAll(CancellationToken cancellationToken = default)
        => await GetList<Contact>(cancellationToken).ConfigureAwait(false);

    public async Task<CreateContactResponse> Create(CreateContactRequest request, CancellationToken cancellationToken = default)
        => await Create<CreateContactRequestDto, CreateContactResponse>(request.ToDto(), cancellationToken).ConfigureAwait(false);
}
