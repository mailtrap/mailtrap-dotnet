namespace Mailtrap;


/// <summary>
/// <see cref="IMailtrapOrganizationClient"/> implementation.
/// </summary>
internal sealed class MailtrapOrganizationClient : RestResource, IMailtrapOrganizationClient
{
    public MailtrapOrganizationClient(IRestResourceCommandFactory restResourceCommandFactory)
        : base(restResourceCommandFactory, Endpoints.ApiDefaultUrl.Append(UrlSegments.ApiRootSegment))
    { }


    public IOrganizationResource Organization(long organizationId)
    {
        Ensure.GreaterThanZero(organizationId, nameof(organizationId));

        return new OrganizationResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.OrganizationsSegment).Append(organizationId));
    }
}
