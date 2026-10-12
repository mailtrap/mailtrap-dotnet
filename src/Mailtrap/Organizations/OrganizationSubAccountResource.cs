namespace Mailtrap.Organizations;


internal sealed class OrganizationSubAccountResource : RestResource, IOrganizationSubAccountResource
{
    public OrganizationSubAccountResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task Delete(CancellationToken cancellationToken = default)
        => await DeleteWithStatusCodeResult(cancellationToken).ConfigureAwait(false);
}
