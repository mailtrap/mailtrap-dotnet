using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Mailtrap;
using Mailtrap.Core.Models;
using Mailtrap.Core.Validation;
using Mailtrap.Emails.Models;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



/// <summary>
/// Example to demonstrate different ways of creating SendEmailRequest
/// <see langword="with"/>variety of parameters and attributes.
/// </summary>
[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Example")]
internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

        IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

        hostBuilder.Services.AddMailtrapClient(config);

        using IHost host = hostBuilder.Build();

        ILogger logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            IMailtrapClient mailtrapClient = host.Services.GetRequiredService<IMailtrapClient>();

            SendEmailRequest request = BasicRequest();

            // It is better to validate request before sending,
            // since send method will do that anyway and throw an exception
            // in case of validation failure.
            ValidationResult validationResult = request.Validate();

            if (!validationResult.IsValid)
            {
                logger.LogError("Malformed email request:\n{ValidationResult}", validationResult.ToString("\n"));
                return;
            }

            SendEmailResponse? response = await mailtrapClient.Email().Send(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while sending email.");
            Environment.FailFast(ex.Message);
            throw;
        }
    }


    /// <summary>
    /// Very basic example of creating SendEmailRequest.
    /// </summary>
    private static SendEmailRequest BasicRequest()
    {
        var from = new EmailAddress("john.doe@demomailtrap.com", "John Doe");
        var to = new EmailAddress("hero.bill@galaxy.net");
        var cc = new EmailAddress("star.lord@galaxy.net");

        var request = new SendEmailRequest
        {
            From = from,
            Subject = "Invitation to Earth",
            TextBody = "Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John."
        };

        // You can specify up to 1000 recipients in total across the To, Cc, and Bcc fields.
        // At least one of recipient collections must contain at least one recipient.
        request.To.Add(to);
        request.Cc.Add(cc);
        request.Bcc.Add(from);

        return request;
    }

    /// <summary>
    /// You can use SendEmailRequestBuilder to create request in a fluent style.<br/>
    /// Much cleaner and easier to read.
    /// </summary>
    private static SendEmailRequest UsingFluentStyle()
    {
        return SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Cc("star.lord@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");
    }

    /// <summary>
    /// In case of using templates, you can specify predefined template ID instead of subject, category and body. <br/>
    /// Subject, Category, HTML and text body must be left empty in this scenario.
    /// </summary>
    private static SendEmailRequest EmailFromTemplate()
    {
        return SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Template("60dca11e-0bc2-42ea-91a8-5ff196acb3f9") // ID of template obtained/created via EmailTemplates API
            .TemplateVariables(new Dictionary<string, string>
            {
                { "name", "Bill" },
                { "sender", "John" }
            });
    }

    /// <summary>
    /// You can attach files to the email
    /// </summary>
    private static SendEmailRequest WithAttachments()
    {
        SendEmailRequest request = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");

        var filePath = @"C:\files\preview.pdf";
        var fileName = "preview.pdf";

        var bytes = File.ReadAllBytes(filePath);
        var fileContent = Convert.ToBase64String(bytes);

        request.Attach(
            content: fileContent,
            fileName: fileName,
            disposition: DispositionType.Attachment,
            mimeType: MediaTypeNames.Application.Pdf);

        filePath = @"C:\files\logo.png";
        fileName = "logo.png";

        bytes = File.ReadAllBytes(filePath);
        fileContent = Convert.ToBase64String(bytes);

        request.Attach(
            content: fileContent,
            fileName: fileName,
            disposition: DispositionType.Inline,
            mimeType: MediaTypeNames.Image.Png,
            contentId: "logo_1");

        return request;
    }

    /// <summary>
    /// Everything in one place.
    /// </summary>
    private static SendEmailRequest RegularKitchenSink()
    {
        var request = new SendEmailRequest
        {
            From = new("john.doe@demomailtrap.com", "John Doe"),
            Subject = "Invitation to Earth"
        };

        request.To.Add(new("hero.bill@galaxy.net"));
        request.Cc.Add(new("ursa@ursamajor.gov"));
        request.Bcc.Add(new("aliens@milkyway.net"));

        request.ReplyTo = new("no-reply@earth.com");

        // HTML body.
        // At least one of the Text or Html body must be specified.
        request.HtmlBody =
            "<h2>Greetings, Bill!</h2>" +
            "<p>It will be a great pleasure to see you on our blue planet next weekend.</p>" +
            "<p>Regards,<br/>" +
            "John</p>";

        // Plain text body.
        // Will be used in case HTML body is missing or is not supported by the recipient.
        request.TextBody =
            "Dear Bill,\n\n" +
            "It will be a great pleasure to see you on our blue planet next weekend.\n\n" +
            "Best regards, John.";

        // Set category for better classification.
        request.Category = "Invitation";

        // Add an attachment
        var filePath = @"C:\files\preview.pdf";
        var fileName = "preview.pdf";
        var bytes = File.ReadAllBytes(filePath);
        var fileContent = Convert.ToBase64String(bytes);
        var attachment = new Attachment(
            content: fileContent,
            fileName: fileName,
            disposition: DispositionType.Attachment,
            mimeType: MediaTypeNames.Application.Pdf,
            contentId: "attachment_1");

        request.Attachments.Add(attachment);

        // Add custom variables
        request.CustomVariables.Add("var_key_1", "var_value_1");

        // Alternatively, you can use indexer
        request.CustomVariables["var_key_2"] = "var_value_2";

        // Add custom headers
        request.Headers.Add("X-Custom-Header-1", "Custom Value 1");

        return request;
    }

    /// <summary>
    /// Everything in one place, using fluent style.
    /// </summary>
    private static SendEmailRequest FluentStyleKitchenSink()
    {
        var request = SendEmailRequest.Create();

        // Sender (Display name is optional)
        request.From("john.doe@demomailtrap.com", "John Doe");

        // Reply To
        request.ReplyTo("info@domain.com");

        // You can use simple email as recipient
        request.To("hero.bill@galaxy.net");

        // Add additional recipients by subsequent calls (Display name is optional)
        request.To("steel.rat@galaxy.net", "James");

        // Alternatively, existing EmailAddress instance can be used.
        var vipEmail = new EmailAddress("star.lord@galaxy.net");
        request.Cc(vipEmail);

        // Alternatively, you could pass a collection at once
        request.Bcc(
            new EmailAddress("first@domain.com"),
            new EmailAddress("second@domain.com"));

        // Subject
        request.Subject("Invitation to Earth");

        // HTML body.
        // At least one of Text or Html body must be specified.
        request.Html(
            "<h2>Greetings, Bill!</h2>" +
            "<p>It will be a great pleasure to see you on our blue planet next weekend.</p>" +
            "<p>Regards,<br/>" +
            "John</p>");

        // Plain text body.
        // Will be used in case HTML body is missing or is not supported by the recipient.
        request.Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");

        // Categorize
        request.Category("Invitation");

        // Add an attachment
        var filePath = @"C:\files\preview.pdf";
        var fileName = "preview.pdf";
        var bytes = File.ReadAllBytes(filePath);
        var fileContent = Convert.ToBase64String(bytes);

        request.Attach(
            content: fileContent,
            fileName: fileName,
            disposition: DispositionType.Attachment,
            mimeType: MediaTypeNames.Application.Pdf,
            contentId: "attachment_1");

        // Add custom variables
        request.CustomVariable("var_key", "var_value");

        // Adding few at once also supported
        request.CustomVariable(
            new("var_key_1", "var_value_1"),
            new("var_key_2", "var_value_2"),
            new("var_key_3", "var_value_3"));

        // Add custom headers
        request.Header("X-Custom-Header", "Custom Value");

        request.Header(
            new("X-Custom-Header-1", "Custom Value 1"),
            new("X-Custom-Header-2", "Custom Value 2"),
            new("X-Custom-Header-3", "Custom Value 3"));

        return request;
    }
}
