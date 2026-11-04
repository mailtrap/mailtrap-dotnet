using Mailtrap;
using Mailtrap.Inboxes;
using Mailtrap.TestingMessages;
using Mailtrap.TestingMessages.Models;
using Mailtrap.TestingMessages.Requests;
using Mailtrap.TestingMessages.Responses;
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
    var sandboxId = 54321;
    IInboxResource inboxResource = mailtrapClient
        .Account(accountId)
        .Sandbox(sandboxId);

    // Get resource for message collection
    ITestingMessageCollectionResource messagesResource = inboxResource.Messages();

    // Fetch messages from the sandbox
    var messageFilter = new TestingMessageFilter
    {
        SearchFilter = "Supervision request"
    };
    IList<TestingMessage> messages = await messagesResource.Fetch(messageFilter);

    ITestingMessageResource messageResource;

    TestingMessage? message = messages.FirstOrDefault();
    if (message is not null)
    {
        // Get resource for message
        messageResource = inboxResource.Message(message.Id);

        // Get message details
        message = await messageResource.GetDetails();
        logger.LogInformation("Message: {Message}", message);
    }

    // Fetch messages from the sandbox
    messageFilter = new TestingMessageFilter
    {
        SearchFilter = "Greetings"
    };
    messages = await messagesResource.Fetch(messageFilter);

    if (messages.Count == 0)
    {
        logger.LogWarning("No greetings messages found.");
    }
    else
    {
        logger.LogInformation("Found {Count} greetings messages.", messages.Count);
    }

    // Another fetch
    messageFilter = new TestingMessageFilter
    {
        SearchFilter = "hero.bill"
    };
    messages = await messagesResource.Fetch(messageFilter);

    message = messages.FirstOrDefault();

    if (message is null)
    {
        logger.LogWarning("No messages found.");
        return;
    }

    // Get resource for message
    messageResource = inboxResource.Message(message.Id);

    // Get message details
    message = await messageResource.GetDetails();
    logger.LogInformation("Message: {Message}", message);

    // Get message headers
    TestingMessageHeaders headers = await messageResource.GetHeaders();
    logger.LogInformation("Message headers: {Headers}", headers.Headers);

    // Get raw message content
    string rawMessageContent = await messageResource.AsRaw();
    logger.LogInformation("Raw message:\n{Message}", rawMessageContent);

    // Get EML message content
    string emlMessageContent = await messageResource.AsEml();
    logger.LogInformation("EML message:\n{Message}", emlMessageContent);

    if (message.TextBodySize > 0)
    {
        // Get plain text message body
        string textMessageBody = await messageResource.GetTextBody();
        logger.LogInformation("Plain text message body:\n{Message}", textMessageBody);
    }

    if (message.HtmlBodySize > 0)
    {
        // Get HTML message body
        string htmlMessageBody = await messageResource.GetHtmlBody();
        logger.LogInformation("HTML message body:\n{Message}", htmlMessageBody);

        // Get HTML message source
        string htmlMessageSource = await messageResource.GetHtmlSource();
        logger.LogInformation("HTML message source:\n{Message}", htmlMessageSource);

        // Get HTML analysis report for message 
        TestingMessageHtmlReport htmlReport = await messageResource.GetHtmlAnalysisReport();
        logger.LogInformation("HTML analysis report: {Report}", htmlReport.Report);
    }

    // Get spam report for message 
    TestingMessageSpamReport spamReport = await messageResource.GetSpamReport();
    logger.LogInformation("Spam report: {Report}", spamReport.Report);

    // Update message details
    var updateMessageRequest = new UpdateTestingMessageRequest(true);
    TestingMessage updatedMessage = await messageResource.Update(updateMessageRequest);
    logger.LogInformation("Updated Message: {Message}", updatedMessage);

    // Forward message
    var forwardMessageRequest = new ForwardTestingMessageRequest("forward@domain.com");
    ForwardTestingMessageResponse forwardedMessage = await messageResource.Forward(forwardMessageRequest);
    logger.LogInformation("Message forwarded: {Forward}", forwardedMessage.Message);

    // Delete message
    // Beware that resource becomes invalid after deletion and should not be used anymore
    TestingMessage deletedMessage = await messageResource.Delete();
    logger.LogInformation("Deleted Message: {Message}", deletedMessage);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}
