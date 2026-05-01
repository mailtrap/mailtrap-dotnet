namespace Mailtrap.Organizations;


internal sealed class OrganizationSubAccountCollectionResource : RestResource, IOrganizationSubAccountCollectionResource
{
    public OrganizationSubAccountCollectionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<IList<SubAccount>> GetAll(CancellationToken cancellationToken = default)
        => await GetList<SubAccount>(cancellationToken).ConfigureAwait(false);

    public async Task<SubAccount> Create(CreateSubAccountRequest request, CancellationToken cancellationToken = default)
        => await Create<CreateSubAccountRequest, SubAccount>(request, cancellationToken).ConfigureAwait(false);
}
