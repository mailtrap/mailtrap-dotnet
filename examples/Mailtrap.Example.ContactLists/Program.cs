using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Contacts;
using Mailtrap.ContactLists;
using Mailtrap.ContactLists.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mailtrap.ContactLists.Requests;


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

    // Get resource for contact lists collection
    IContactListCollectionResource contactListsResource = contactsResource.Lists();

    // Get all contact lists for account
    IList<ContactList> contactLists = await contactListsResource.GetAll();

    // Filter contact lists by name (case-insensitive prefix match)
    var searchFilter = new ContactListListFilter { Search = "news" };
    IList<ContactList> matchingContactLists = await contactListsResource.GetAll(searchFilter);
    logger.LogInformation("Found {Count} contact list(s) matching search filter.", matchingContactLists.Count);

    ContactList? contactList = contactLists.Count > 0 ? contactLists[0] : null;

    if (contactList is null)
    {
        logger.LogWarning("No contacts list found. Creating.");

        // Create contact list
        var createContactsListRequest = new ContactListRequest("MyFirstContactList");
        contactList = await contactListsResource.Create(createContactsListRequest);
    }
    else
    {
        logger.LogInformation("Contact List {Name} found.", contactList.Name);
    }

    // Get resource for specific contact list
    IContactListResource contactListResource = contactsResource.List(contactList.Id);

    // Get details
    ContactList contactListResponse = await contactListResource.GetDetails();
    logger.LogInformation("Contact List Name from resource: {Name}", contactListResponse.Name);
    logger.LogInformation("Contact List Id from resource: {Id}", contactListResponse.Id);

    // Update contact list details
    var updateContactListRequest = new ContactListRequest("updatedContactList");
    ContactList updateContactListResponse = await contactListResource.Update(updateContactListRequest);
    logger.LogInformation("Updated Contact List: Name={Name}, Id={Id}", updateContactListResponse.Name, updateContactListResponse.Id);

    // Delete contact List
    // Beware that contact list resource becomes invalid after deletion and should not be used anymore
    await contactListResource.Delete();
    logger.LogInformation("Contact List Deleted.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
