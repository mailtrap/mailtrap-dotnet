using System.Net;

using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Contacts;
using Mailtrap.Contacts.Models;
using Mailtrap.Contacts.Requests;
using Mailtrap.Contacts.Responses;
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

    var contactEmail = "example@mailtrap.io";

    // Get resource for contacts collection
    IContactCollectionResource contactsResource = accountResource.Contacts();

    // Get all contacts for account
    // TODO: Enable when GetAll is implemented
    IList<Contact> contacts = /*await contactsResource.GetAll()*/ [];

    Contact? contact = contacts
        .FirstOrDefault(p => string.Equals(p.Email, contactEmail, StringComparison.OrdinalIgnoreCase));


    if (contact is null)
    {
        logger.LogWarning("No contact found. Creating.");

        // Create contact
        var createContactRequest = new CreateContactRequest(contactEmail);
        createContactRequest.Fields.Add("first_name", "John");
        createContactRequest.Fields.Add("last_name", "Doe");
        CreateContactResponse createContactResponse = await contactsResource.Create(createContactRequest);
        contact = createContactResponse.Contact;
    }
    else
    {
        logger.LogInformation("Contact found.");
    }

    // Get resource for specific contact
    IContactResource contactResource = accountResource.Contact(contact.Id);

    // Get details
    ContactResponse contactResponse = await contactResource.GetDetails();
    Contact contactDetails = contactResponse.Contact;
    logger.LogInformation("Contact: {Contact}", contactDetails);

    // Update contact details
    var updateContactRequest = new UpdateContactRequest("test@mailtrap.io");

    // Unsubscribe the contact from one subscription topic on one sending domain.
    // Pairs of topic and domain left out keep their status.
    ContactSubscriptionTopic? topicPreference = await FirstTopicPreference(mailtrapClient, accountResource, ContactStatus.Unsubscribed);
    if (topicPreference is not null)
    {
        updateContactRequest.SubscriptionTopics = [topicPreference];
    }

    UpdateContactResponse updateContactResponse = await contactResource.Update(updateContactRequest);
    Contact updatedContact = updateContactResponse.Contact;
    logger.LogInformation("Updated Contact: {Contact}", updatedContact);

    // Null when the account has no subscription topics, or when they couldn't be read this time.
    foreach (ContactSubscriptionTopic preference in updatedContact.SubscriptionTopics ?? [])
    {
        logger.LogInformation("Topic {TopicId} on domain {DomainId}: {Status}",
            preference.SubscriptionTopicId, preference.DomainId, preference.Status);
    }

    // Delete contact
    // Beware that contact resource becomes invalid after deletion and should not be used anymore
    await contactResource.Delete();
    logger.LogInformation("Contact Deleted.");
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
