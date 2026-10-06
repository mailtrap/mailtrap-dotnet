using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Templates;
using Mailtrap.Templates.Models;
using Mailtrap.Templates.Requests;
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

    // Get resource for templates collection
    ITemplateCollectionResource templatesResource = accountResource.Templates();

    // Get the first page of templates, 50 per page
    var filter = new TemplateListFilter { Token = 1, PerPage = 50 };
    TemplateList page = await templatesResource.GetAll(filter);

    logger.LogInformation("Found {Count} templates on the first page.", page.Data.Count);
    logger.LogInformation("Next page token: {NextToken}", page.Pagination?.NextToken);

    // Create a dedicated template so the update and delete below never touch an existing one
    var createTemplateRequest = new CreateTemplateRequest
    {
        Name = "MyFirstTemplate",
        Category = "TestCategory",
        Subject = "TestSubject",
        BodyHtml = "<h1>This is HTML body</h1>",
        BodyText = "This is text body"
    };
    Template template = await templatesResource.Create(createTemplateRequest);
    logger.LogInformation("Created template {Name} with Id {Id}.", template.Name, template.Id);

    // Get resource for specific template
    ITemplateResource templateResource = accountResource.Template(template.Id);

    // Get details
    Template templateResponse = await templateResource.GetDetails();
    logger.LogInformation("Template Name from resource: {Name}", templateResponse.Name);
    logger.LogInformation("Template Id from resource: {Id}", templateResponse.Id);

    // Update template details, only the properties that are set are changed
    var updateTemplateRequest = new UpdateTemplateRequest
    {
        Name = "updatedTemplate",
        BodyHtml = "<h1>This is updated HTML body</h1>"
    };
    Template updateTemplateResponse = await templateResource.Update(updateTemplateRequest);
    logger.LogInformation("Updated Template: Name={Name}, Id={Id}", updateTemplateResponse.Name, updateTemplateResponse.Id);

    // Delete template
    // Beware that template resource becomes invalid after deletion and should not be used anymore
    await templateResource.Delete();
    logger.LogInformation("Template Deleted.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
