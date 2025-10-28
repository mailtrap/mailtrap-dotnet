namespace Mailtrap.Contacts;

/// <summary>
/// Represents Contact collection resource.
/// </summary>
public interface IContactCollectionResource : IRestResource
{
    /// <summary>
    /// Gets contact export collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// <see cref="IContactExportCollectionResource"/> for the account, represented by this resource instance.
    /// </returns>
    public IContactExportCollectionResource Exports();

    /// <summary>
    /// Gets resource for a specific contact export identified by <paramref name="exportId"/>.
    /// </summary>
    ///
    /// <param name="exportId">
    /// Unique Contact Export ID to get resource for.
    /// </param>
    ///
    /// <returns>
    /// <see cref="IContactExportResource"/> for the contact export with the specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="exportId"/> is less than or equal to zero.
    /// </exception>
    public IContactExportResource Export(long exportId);

    /// <summary>
    /// Gets contact import collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// <see cref="IContactImportCollectionResource"/> for the account, represented by this resource instance.
    /// </returns>
    public IContactImportCollectionResource Imports();

    /// <summary>
    /// Gets resource for a specific contact import identified by <paramref name="importId"/>.
    /// </summary>
    ///
    /// <param name="importId">
    /// Unique Contact Import ID to get resource for.
    /// </param>
    ///
    /// <returns>
    /// <see cref="IContactImportResource"/> for the contact import with the specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="importId"/> is less than or equal to zero.
    /// </exception>
    public IContactImportResource Import(long importId);

    /// <summary>
    /// Gets contact list collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// <see cref="IContactListCollectionResource"/> for the account, represented by this resource instance.
    /// </returns>
    public IContactListCollectionResource Lists();

    /// <summary>
    /// Gets resource for a specific contact list identified by <paramref name="listId"/>.
    /// </summary>
    ///
    /// <param name="listId">
    /// Unique Contact List ID to get resource for.
    /// </param>
    ///
    /// <returns>
    /// <see cref="IContactListResource"/> for the contact list with the specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="listId"/> is less than or equal to zero.
    /// </exception>
    public IContactListResource List(long listId);

    /// <summary>
    /// Gets contact field collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// <see cref="IContactFieldCollectionResource"/> for the account, represented by this resource instance.
    /// </returns>
    public IContactFieldCollectionResource Fields();

    /// <summary>
    /// Gets resource for a specific contact field identified by <paramref name="fieldId"/>.
    /// </summary>
    ///
    /// <param name="fieldId">
    /// Unique Contact Field ID to get resource for.
    /// </param>
    ///
    /// <returns>
    /// <see cref="IContactFieldResource"/> for the contact field with the specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="fieldId"/> is less than or equal to zero.
    /// </exception>
    public IContactFieldResource Field(long fieldId);

    /// <summary>
    /// Gets contact event collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <param name="contactId">
    /// Unique Contact ID to get resource for.
    /// </param>
    ///
    /// <returns>
    /// <see cref="IContactEventCollectionResource"/> for the account, represented by this resource instance.
    /// </returns>
    public IContactEventCollectionResource Events(string contactId);

    /// <summary>
    /// Creates a new contact with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing contact details for creation.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Created contact details.
    /// </returns>
    public Task<CreateContactResponse> Create(CreateContactRequest request, CancellationToken cancellationToken = default);
}
