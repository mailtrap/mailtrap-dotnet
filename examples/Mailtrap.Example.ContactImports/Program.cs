using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Contacts;
using Mailtrap.Contacts.Requests;
using Mailtrap.ContactImports;
using Mailtrap.ContactImports.Models;
using Mailtrap.ContactImports.Requests;
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

    //Get resource for contact import collection
    IContactImportCollectionResource contactImportsResource = contactsResource.Imports();

    // Prepare list of contacts to import
    var contactImportList = new List<ContactImportRequest>
    {
        new("alice@mailtrap.io"),
        new("bob@mailtrap.io"),
        new("charlie@mailtrap.io"),
    };

    // Create contact import request
    var importRequest = new CreateContactImportRequest(contactImportList);

    // Import contacts in bulk
    ContactImport importResponse = await contactImportsResource.Create(importRequest);
    logger.LogInformation("Created contact import, Id: {ImportId}", importResponse.Id);

    // Get resource for specific contact import
    IContactImportResource contactImportResource = contactsResource.Import(importResponse.Id);

    // Get details of specific contact import
    ContactImport contactImportDetails = await contactImportResource.GetDetails();
    logger.LogInformation("Contact Import Details: {Id}", contactImportDetails.Id);
    logger.LogInformation("Contact Import Details: {Status}", contactImportDetails.Status);
    logger.LogInformation("Contact Import Details: {CreatedContactsCount}", contactImportDetails.CreatedContactsCount);
    logger.LogInformation("Contact Import Details: {UpdatedContactsCount}", contactImportDetails.UpdatedContactsCount);
    logger.LogInformation("Contact Import Details: {ContactsOverLimitCount}", contactImportDetails.ContactsOverLimitCount);

    if (contactImportDetails.Status == ContactImportStatus.Failed)
    {
        logger.LogWarning("Import failed!");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}
