using System.Net;

using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Contacts;
using Mailtrap.Contacts.Models;
using Mailtrap.Contacts.Requests;
using Mailtrap.ContactImports;
using Mailtrap.ContactImports.Models;
using Mailtrap.ContactImports.Requests;
using Mailtrap.Core.Exceptions;
using Mailtrap.SubscriptionTopics.Models;
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

    // Set a subscription topic preference for one of the contacts; the others keep theirs.
    ContactSubscriptionTopic? topicPreference = await FirstTopicPreference(mailtrapClient, accountResource, ContactStatus.Subscribed);
    if (topicPreference is not null)
    {
        contactImportList[0].SubscriptionTopics = [topicPreference];
    }

    // Create contact import request
    var importRequest = new CreateContactImportRequest(contactImportList);

    // Import contacts in bulk
    ContactImport importResponse;
    try
    {
        importResponse = await contactImportsResource.Create(importRequest);
    }
    catch (HttpRequestFailedException ex) when (ex.StatusCode == HttpStatusCode.UnprocessableEntity && topicPreference is not null)
    {
        // Topic preferences can be unavailable for an account even when it has topics:
        // the API then rejects the import, so import the contacts without them.
        logger.LogWarning("The import's subscription topic preferences were rejected: {Error} Importing without them.", ex.Message);
        importRequest.Contacts[0].SubscriptionTopics = null;
        importResponse = await contactImportsResource.Create(importRequest);
    }

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

internal sealed partial class Program
{
    // The account's first subscription topic, on its sending domain or, for a topic that covers
    // every domain, on the account's first domain; null for an account without topics.
    private static async Task<ContactSubscriptionTopic?> FirstTopicPreference(
        IMailtrapClient mailtrapClient,
        IAccountResource accountResource,
        ContactStatus status)
    {
        try
        {
            SubscriptionTopicList topics = await mailtrapClient.SubscriptionTopics().GetAll();
            SubscriptionTopic? topic = topics.Data.FirstOrDefault();

            if (topic is null)
            {
                return null;
            }

            long? domainId = topic.DomainId ?? (await accountResource.Domains().GetAll()).FirstOrDefault()?.Id;

            return domainId is null
                ? null
                : new ContactSubscriptionTopic { SubscriptionTopicId = topic.Id, DomainId = domainId.Value, Status = status };
        }
        catch (HttpRequestFailedException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            // Subscription topics are not available for this account.
            return null;
        }
    }
}
