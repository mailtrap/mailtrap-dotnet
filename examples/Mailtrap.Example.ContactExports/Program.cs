using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Contacts;
using Mailtrap.ContactExports;
using Mailtrap.ContactExports.Models;
using Mailtrap.ContactExports.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

hostBuilder.Services.AddMailtrapClient(config);

using IHost host = hostBuilder.Build();

ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
IMailtrapClient mailtrapClient = host.Services.GetRequiredService<IMailtrapClient>();

try
{
    var accountId = 12345;
    IAccountResource accountResource = mailtrapClient.Account(accountId);

    // Get resource for contacts collection
    IContactCollectionResource contactsResource = accountResource.Contacts();

    //Get resource for contact export collection
    IContactExportCollectionResource contactExportsResource = contactsResource.Exports();

    // Prepare filters for contacts export
    var filters = new List<ContactExportFilterBase>
    {
        new ContactExportListIdFilter(new List<int> { 123, 456 }),
        new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Subscribed)
    };

    // Construct contact export request
    var exportRequest = new CreateContactExportRequest(filters);

    // Create contact export
    ContactExport exportResponse = await contactExportsResource.Create(exportRequest);
    logger.LogInformation("Created contact export, Id: {ExportId}", exportResponse.Id);

    // Get resource for specific contact export
    IContactExportResource contactExportResource = contactsResource.Export(exportResponse.Id);

    // Get details of specific contact export
    ContactExport contactExportDetails = await contactExportResource.GetDetails();
    logger.LogInformation("Contact Export Details: {Id}", contactExportDetails.Id);
    logger.LogInformation("Contact Export Details: {Status}", contactExportDetails.Status);
    logger.LogInformation("Contact Export Details: {CreatedAt}", contactExportDetails.CreatedAt);
    logger.LogInformation("Contact Export Details: {UpdatedAt}", contactExportDetails.UpdatedAt);
    logger.LogInformation("Contact Export Details: {Url}", contactExportDetails.Url);

    if (contactExportDetails.Status == ContactExportStatus.Finished && contactExportDetails.Url is not null)
    {
        logger.LogInformation("File ready for download");
        logger.LogInformation("Export URL: {Url}", contactExportDetails.Url);
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}
