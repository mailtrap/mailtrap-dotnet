using Mailtrap;
using Mailtrap.Attachments;
using Mailtrap.Attachments.Models;
using Mailtrap.Core.Models;
using Mailtrap.TestingMessages;
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
    var messageId = 1122334455;
    ITestingMessageResource messageResource = mailtrapClient
        .Account(accountId)
        .Sandbox(sandboxId)
        .Message(messageId);

    // Get resource for attachments collection
    IAttachmentCollectionResource attachmentsResource = messageResource.Attachments();

    var attachmentFilter = new EmailAttachmentFilter
    {
        Disposition = DispositionType.Attachment
    };

    IList<EmailAttachment> attachments = await attachmentsResource.Fetch(attachmentFilter);

    EmailAttachment? attachment = attachments.FirstOrDefault();

    if (attachment is null)
    {
        logger.LogWarning("No attachments found.");

        return;
    }

    // Get resource for specific attachment
    IAttachmentResource attachmentResource = messageResource.Attachment(attachment.Id);

    // Get attachment details
    attachment = await attachmentResource.GetDetails();
    logger.LogInformation("Attachment: {Attachment}", attachment);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}
