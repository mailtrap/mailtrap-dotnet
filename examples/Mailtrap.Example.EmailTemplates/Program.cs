using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.EmailTemplates;
using Mailtrap.EmailTemplates.Models;
using Mailtrap.EmailTemplates.Requests;
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

    // Get resource for email templates collection
    IEmailTemplateCollectionResource emailTemplatesResource = accountResource.EmailTemplates();

    // Get all email templates for account
    IList<EmailTemplate> emailTemplates = await emailTemplatesResource.GetAll();

    EmailTemplate? emailTemplate = emailTemplates.Count > 0 ? emailTemplates[0] : null;

    if (emailTemplate is null)
    {
        logger.LogWarning("No email template found. Creating.");

        // Create email template
        var createEmailTemplateRequest = new CreateEmailTemplateRequest("MyFirstEmailTemplate", "TestCategory", "TestSubject")
        {
            BodyHtml = "<h1>This is HTML body</h1>",
            BodyText = "This is text body"
        };
        emailTemplate = await emailTemplatesResource.Create(createEmailTemplateRequest);
    }
    else
    {
        logger.LogInformation("Email Template {Name} found.", emailTemplate.Name);
    }

    // Get resource for specific email template
    IEmailTemplateResource emailTemplateResource = accountResource.EmailTemplate(emailTemplate.Id);

    // Get details
    EmailTemplate emailTemplateResponse = await emailTemplateResource.GetDetails();
    logger.LogInformation("Email Template Name from resource: {Name}", emailTemplateResponse.Name);
    logger.LogInformation("Email Template Id from resource: {Id}", emailTemplateResponse.Id);

    // Update email template details
    var updateEmailTemplateRequest = new UpdateEmailTemplateRequest("updatedEmailTemplate", "NewCategory", "NewSubject")
    {
        BodyHtml = "<h1>This is updated HTML body</h1>",
        BodyText = "This is updated text body"
    };
    EmailTemplate updateEmailTemplateResponse = await emailTemplateResource.Update(updateEmailTemplateRequest);
    logger.LogInformation("Updated Email Template: Name={Name}, Id={Id}", updateEmailTemplateResponse.Name, updateEmailTemplateResponse.Id);

    // Delete email template
    // Beware that email template resource becomes invalid after deletion and should not be used anymore
    await emailTemplateResource.Delete();
    logger.LogInformation("Email Template Deleted.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
