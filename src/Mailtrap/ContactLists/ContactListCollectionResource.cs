namespace Mailtrap.ContactLists;

/// <summary>
/// Implementation of Contact List Collection resource.
/// </summary>
internal sealed class ContactListCollectionResource : RestResource, IContactListCollectionResource
{
    private const string SearchQueryParameter = "search";


    public ContactListCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }

    public async Task<IList<ContactList>> GetAll(ContactListListFilter? filter = null, CancellationToken cancellationToken = default)
        => await GetList<ContactList>(CreateListUri(filter), cancellationToken).ConfigureAwait(false);

    public async Task<ContactList> Create(ContactListRequest request, CancellationToken cancellationToken = default)
        => await Create<ContactListRequest, ContactList>(request, cancellationToken).ConfigureAwait(false);


    private Uri CreateListUri(ContactListListFilter? filter)
    {
        return filter?.Search is not null && !string.IsNullOrWhiteSpace(filter.Search)
            ? ResourceUri.AppendQueryParameter(SearchQueryParameter, filter.Search)
            : ResourceUri;
    }
}
