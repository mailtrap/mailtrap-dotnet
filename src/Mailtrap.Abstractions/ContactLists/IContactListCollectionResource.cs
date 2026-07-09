namespace Mailtrap.ContactLists;

/// <summary>
/// Represents the contact list collection resource.
/// </summary>
public interface IContactListCollectionResource : IRestResource
{
    /// <summary>
    /// Gets collection of contact list details.
    /// </summary>
    ///
    /// <param name="filter">
    /// Optional filter to apply when fetching contact lists.<br />
    /// When <see cref="ContactListListFilter.Search"/> is specified, only contact lists whose name
    /// starts with the provided value (case-insensitive prefix match) are returned.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// A collection of contact list details.
    /// </returns>
    public Task<IList<ContactList>> GetAll(ContactListListFilter? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new contact list with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// The request containing contact list details for creation.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetAll(ContactListListFilter, CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Created contact list details.
    /// </returns>
    public Task<ContactList> Create(ContactListRequest request, CancellationToken cancellationToken = default);
}
