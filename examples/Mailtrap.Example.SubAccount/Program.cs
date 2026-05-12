using Mailtrap;
using Mailtrap.Organizations;
using Mailtrap.Organizations.Models;
using Mailtrap.Organizations.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

hostBuilder.Services.AddMailtrapClient(config);

using IHost host = hostBuilder.Build();

ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();

// Sub accounts live under /api/organizations/{organization_id}/... so they are
// accessed via the organization-scoped client, not IMailtrapClient.
IMailtrapOrganizationClient organizationClient = host.Services.GetRequiredService<IMailtrapOrganizationClient>();

try
{
    var organizationId = 12345;
    IOrganizationResource organizationResource = organizationClient.Organization(organizationId);

    // Get resource for sub accounts collection
    IOrganizationSubAccountCollectionResource subAccountsResource = organizationResource.SubAccounts();

    // List sub accounts of the organization
    IList<SubAccount> subAccounts = await subAccountsResource.GetAll();
    logger.LogInformation("Found {Count} sub account(s).", subAccounts.Count);

    foreach (SubAccount subAccount in subAccounts)
    {
        logger.LogInformation("Sub Account: Id={Id}, Name={Name}", subAccount.Id, subAccount.Name);
    }

    // Create a new sub account under the organization
    var createRequest = new CreateSubAccountRequest
    {
        Account = new SubAccountAttributes
        {
            Name = "Demo Sub Account"
        }
    };
    SubAccount createdSubAccount = await subAccountsResource.Create(createRequest);
    logger.LogInformation(
        "Created Sub Account: Id={Id}, Name={Name}",
        createdSubAccount.Id,
        createdSubAccount.Name);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
