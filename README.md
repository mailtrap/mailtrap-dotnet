![Mailtrap](assets/img/mailtrap-logo.svg)

[![GitHub Package Version](https://img.shields.io/github/v/release/mailtrap/mailtrap-dotnet?label=GitHub%20Packages)](https://github.com/orgs/mailtrap/packages/nuget/package/Mailtrap)
[![CI](https://github.com/mailtrap/mailtrap-dotnet/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/mailtrap/mailtrap-dotnet/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/mailtrap/mailtrap-dotnet/blob/main/LICENSE.md)

# Official Mailtrap .NET Client
Welcome to the official [Mailtrap](https://mailtrap.io/) .NET Client repository.  
This client allows you to quickly and easily integrate your .NET application with **v2.0** of the [Mailtrap API](https://api-docs.mailtrap.io/docs/mailtrap-api-docs/5tjdeg9545058-mailtrap-api).

---

## Prerequisites

To get the most out of this official Mailtrap.io .NET SDK:

- [Create a Mailtrap account](https://mailtrap.io/signup)
- [Verify your domain](https://mailtrap.io/sending/domains)
- Obtain [API token](https://mailtrap.io/api-tokens)
- Please ensure your project targets a .NET implementation that supports [.NET Standard 2.0](https://dotnet.microsoft.com/platform/dotnet-standard#versions) specification.

---

## Installation

The Mailtrap .NET client packages are available through GitHub Packages.

Add the GitHub Packages source to your NuGet configuration:

```bash
dotnet nuget add source https://nuget.pkg.github.com/mailtrap/index.json --name github-mailtrap
```

Then add the Mailtrap package:

```bash
dotnet add package Mailtrap -v 2.0.0 -s github-mailtrap
```

Optionally, you can add the Mailtrap.Abstractions package:

```bash
dotnet add package Mailtrap.Abstractions -v 2.0.0 -s github-mailtrap
```

---

## Framework tools integration

If you are using a framework such as .NET Core, you can integrate the Mailtrap client easily through dependency injection:

```csharp
using Mailtrap;

hostBuilder.ConfigureServices((context, services) =>
{
    services.AddMailtrapClient(options =>
    {
        // Definitely, hardcoding a token isn't a good idea.
        // This example uses it for simplicity, but in real-world scenarios
        // you should consider more secure approaches for storing secrets.
            
        // Environment variables can be an option, as well as other solutions:
        // https://learn.microsoft.com/aspnet/core/security/app-secrets
        // or https://learn.microsoft.com/aspnet/core/security/key-vault-configuration
        options.ApiToken = "<API_TOKEN>";
    });
});
```

This automatically registers an `IMailtrapClient` instance for injection.

---

## Usage

### Minimal usage (Sending)

The simplest way to send an email with only the required parameters:

```csharp
using Mailtrap;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;

try
{
    var apiToken = "<API-TOKEN>";
    using var mailtrapClientFactory = new MailtrapClientFactory(apiToken);
    IMailtrapClient mailtrapClient = mailtrapClientFactory.CreateClient();
    SendEmailRequest request = SendEmailRequest
        .Create()
        .From("hello@demomailtrap.co", "Mailtrap Test")
        .To("world@demomailtrap.co")
        .Subject("You are awesome!")
        .Text("Congrats for sending test email with Mailtrap!");
    SendEmailResponse? response = await mailtrapClient
        .Email()
        .Send(request);
}
catch (MailtrapException mtex)
{
    // handle Mailtrap API specific exceptions
}
catch (OperationCanceledException ocex)
{
    // handle cancellation
}
catch (Exception ex)
{
    // handle other exceptions
}  
```

### Sending - Testing (Sandbox vs Production)

Mailtrap allows you to switch between sandbox (testing) and production sending environments easily.

**Difference:** In sandbox mode, you use `.Test(inboxId)` instead of `.Email()`, and provide an `inboxId`.

```csharp
var apiToken = "<API-TOKEN>";
var inboxId = <INBOX-ID>; // Only needed for sandbox
using var mailtrapClientFactory = new MailtrapClientFactory(apiToken);
IMailtrapClient mailtrapClient = mailtrapClientFactory.CreateClient();
SendEmailRequest request = SendEmailRequest
    .Create()
    .From("hello@demomailtrap.co", "Mailtrap Test")
    .To("world@demomailtrap.co")
    .Subject("You are awesome!")
    .Text("Congrats for sending test email with Mailtrap!");
SendEmailResponse? response = await mailtrapClient
    .Test(inboxId)
    .Send(request);
```

This allows you to validate sending logic without delivering actual emails to production recipients.

### Bulk stream example

Bulk stream sending differs only by using `.Bulk()` instead of `.Email()`:

```csharp
SendEmailResponse? response = await mailtrapClient
    .Bulk()
    .Send(request);
```

This enables optimized delivery for large batches of transactional or marketing messages.

### Full-featured example of Sending

A more advanced example showing full email sending features, including templates, attachments, and validation:

```csharp
using System.Net.Mime;
using Mailtrap;
using Mailtrap.Core.Models;
using Mailtrap.Core.Validation;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;

try
{
    var apiToken = "<API-TOKEN>";
    using var mailtrapClientFactory = new MailtrapClientFactory(apiToken);
    IMailtrapClient mailtrapClient = mailtrapClientFactory.CreateClient();

    SendEmailRequest request = FullyFeaturedRequest();

    SendEmailResponse? response = await SendEmailAsync(mailtrapClient, FullyFeaturedRequest());
    Console.Out.Write(response);

    response = await SendEmailAsync(mailtrapClient, TemplateBasedRequest());
    Console.Out.Write(response);
}
catch (Exception ex)
{
    await Console.Out.WriteAsync("An error occurred while sending email: " + ex.ToString());
    return;
}

private static async Task<SendEmailResponse?> SendEmailAsync(IMailtrapClient mailtrapClient, SendEmailRequest request)
{
    ArgumentNullException.ThrowIfNull(mailtrapClient);
    ArgumentNullException.ThrowIfNull(request);

    ValidationResult validationResult = request.Validate();
    if (!validationResult.IsValid)
    {
        // Malformed email request, use validationResult to get errors
        return null;
    }

    return await mailtrapClient.Email().Send(request);
}

private static SendEmailRequest FullyFeaturedRequest()
{
    return SendEmailRequest
        .Create()
        .From("john.doe@demomailtrap.com", "John Doe")
        .ReplyTo("reply@example.com")
        .Category("Your Category")
        .To("hero.bill@galaxy.net")
        .Cc("star.lord@galaxy.net")
        .Bcc("noreply@example.com")
        .Subject("Invitation to Earth")
        .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.")
        .Html(
            @"<html>
                <body>
                    <p><br>Hey!</br>
                    Learn the best practices of building HTML emails and play with ready-to-go templates.</p>
                    <p><a href=""https://mailtrap.io/blog/build-html-email/""> Mailtrap's Guide on How to Build HTML Email</a> is live on our blog</p>
                    <img src=""cid:logo"">
                </body>
            </html>"
        )
        .Header("X-Message-Source", "domain.com") // Custom email headers (optional)
        .CustomVariable("user_id", "45982")
        .CustomVariable("batch_id", "PSJ-12")
        .Attach(
            content: Convert.ToBase64String(File.ReadAllBytes("./preview.pdf")),
            fileName: "preview.pdf",
            disposition: DispositionType.Attachment,
            mimeType: MediaTypeNames.Application.Pdf);
}

private static SendEmailRequest TemplateBasedRequest()
{
    return SendEmailRequest
        .Create()
        .From("example@your-domain-here.com", "Mailtrap Test")
        .ReplyTo("reply@your-domain-here.com")
        .To("example@gmail.com", "Jon")
        .Template("bfa432fd-0000-0000-0000-8493da283a69")
        .TemplateVariables(new Dictionary<string, object>
        {
            { "user_name", "Jon Bush" },
            { "next_step_link", "https://mailtrap.io/" },
            { "get_started_link", "https://mailtrap.io/" },
            { "onboarding_video_link", "some_video_link" },
            { "company", new Dictionary<string, object>
                {
                    { "name", "Best Company" },
                    { "address", "Its Address" }
                }
            },
            { "products", new object[]
                {
                    new Dictionary<string, object> { { "name", "Product 1" }, { "price", 100 } },
                    new Dictionary<string, object> { { "name", "Product 2" }, { "price", 200 } }
                }
            },
            { "isBool", true },
            { "int", 123 }
        });
}
```

### Other examples

#### Email API/SMTP
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send an email (Transactional and Bulk streams), Send an email with a template and/or with attachments,
- **[Batch Email Sending](examples/Mailtrap.Example.Email.BatchSend/)** - Batch email sending, sending with a template and/or with attachments.

#### Email Sandbox (Testing)
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send an email, send an email with a template,
- **[Testing Messages](examples/Mailtrap.Example.TestingMessage/)** - Message management,
- **[Attachments](examples/Mailtrap.Example.Attachment/)** - Working with email attachments in testing messages,
- **[Inbox Management](examples/Mailtrap.Example.Inbox/)** - Inbox management,
- **[Project Management](examples/Mailtrap.Example.Project/)** - Project management.

#### Contacts Management
- **[Contacts Management](examples/Mailtrap.Example.Contacts/)** - Contacts management,
- **[Contact Events](examples/Mailtrap.Example.ContactsEvents/)** - Contact Events management,
- **[Contact Exports](examples/Mailtrap.Example.ContactsExports/)** - Contact Exports management,
- **[Contact Imports](examples/Mailtrap.Example.ContactsImports/)** - Contact Imports management,
- **[Contact Fields](examples/Mailtrap.Example.ContactsFields/)** - Contact Fields management,
- **[Contact Lists](examples/Mailtrap.Example.ContactsLists/)** - Contact Lists management.

#### General
- **[Account Access](examples/Mailtrap.Example.AccountAccess/)** - Account access management,
- **[Permissions](examples/Mailtrap.Example.Permissions/)** - Permissions management,
- **[Account Management](examples/Mailtrap.Example.Account/)** - List accounts you have access to,
- **[Billing](examples/Mailtrap.Example.Billing/)** - Billing information and usage statistics,
- **[Sending Domains](examples/Mailtrap.Example.SendingDomain/)** - Domain verification,
- **[Email Templates](examples/Mailtrap.Example.EmailTemplates/)** - Email Templates management,
- **[Comprehensive API Usage](examples/Mailtrap.Example.ApiUsage/)** - Complete example showcasing multiple API features together.

#### Configuration & Setup
- **[Dependency Injection](examples/Mailtrap.Example.DependencyInjection/)** - Integration with ASP.NET Core DI container and configuration
- **[Factory Pattern](examples/Mailtrap.Example.Factory/)** - Using standalone client factory for scenarios without DI container

Each example includes detailed comments and demonstrates best practices for error handling, configuration, and resource management.

---

## Supported functionality

Currently, with this SDK, you can:

### Email API/SMTP

- Send an email (Transactional and Bulk streams)
- Send an email with a template
- Send emails with attachments
- Send batch of emails (Transactional and Bulk streams, with template and attachments)
- Sending domain management

### Email Sandbox (Testing)

- Send an email
- Send an email with a template
- Message management
- Inbox management
- Project management

### Contacts Management

- Contacts management
- Fields management
- Lists management
- Import/Export management
- Events management

### General

- Email Templates management
- Account access management
- Permissions management
- Billing information and usage statistics
- Domain verification

---

## Documentation
Please visit [Documentation Portal](https://mailtrap.github.io/mailtrap-dotnet/) for detailed setup, configuration and usage instructions.

## Contributing
We believe in the power of OSS and welcome any contributions to the library on [GitHub](https://github.com/mailtrap/mailtrap-dotnet).
Please refer to [Contributing Guide](CONTRIBUTING.md) for details.
This project is intended to be a safe, welcoming space for collaboration, and contributors are expected to adhere to the [code of conduct](CODE_OF_CONDUCT.md).

## License
The package is available as open source under the terms of the [MIT License](https://opensource.org/licenses/MIT).

## Code of Conduct

Everyone interacting in the Mailtrap project's codebases, issue trackers, chat rooms and mailing lists is expected to follow the [code of conduct](CODE_OF_CONDUCT.md).
