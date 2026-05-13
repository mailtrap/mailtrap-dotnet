namespace Mailtrap.Organizations;


internal sealed class OrganizationResource : RestResource, IOrganizationResource
{
    public OrganizationResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public IOrganizationSubAccountCollectionResource SubAccounts()
        => new OrganizationSubAccountCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.SubAccountsSegment));
}
