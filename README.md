![Mailtrap](assets/img/mailtrap-logo.svg)

[![GitHub Package Version](https://img.shields.io/github/v/release/mailtrap/mailtrap-dotnet?label=GitHub%20Packages)](https://github.com/orgs/mailtrap/packages/nuget/package/Mailtrap)
[![CI](https://github.com/mailtrap/mailtrap-dotnet/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/mailtrap/mailtrap-dotnet/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/mailtrap/mailtrap-dotnet/blob/main/LICENSE.md)

# Official Mailtrap .NET Client
Welcome to the official [Mailtrap](https://mailtrap.io/) .NET Client repository.  
This client allows you to quickly and easily integrate your .NET application with **v2.0** of the [Mailtrap API](https://api-docs.mailtrap.io/docs/mailtrap-api-docs/5tjdeg9545058-mailtrap-api).


## Prerequisites

To get the most out of this official Mailtrap.io .NET SDK:
- [Create a Mailtrap account](https://mailtrap.io/signup)
- [Verify your domain](https://mailtrap.io/sending/domains)
- Obtain [API token](https://mailtrap.io/api-tokens)
- Please ensure your project targets .NET implementation which supports [.NET Standard 2.0](https://dotnet.microsoft.com/platform/dotnet-standard#versions) specification.

## Supported functionality

The Mailtrap .NET client provides comprehensive access to the Mailtrap API v2.0, including:

### Email API/SMTP
- Send an email (Transactional and Bulk streams)
- Send an email with a template
- Send emails with attachments

### Email Sandbox (Testing)
- Send an email
- Send an email with a template
- Message management
- List inboxes
- Get inbox details
- Update inbox settings
- Create, read, update, and delete projects
- Project configuration and settings

### General
- Account access management
- Permissions management
- List accounts you have access to
- Billing information and usage statistics
- Domain verification

## Quick Start
The following few simple steps will bring Mailtrap API functionality into your .NET project.

### Install
The Mailtrap .NET client packages are available through GitHub Packages.

First, add the GitHub Packages source to your NuGet configuration:

```console
dotnet nuget add source https://nuget.pkg.github.com/mailtrap/index.json --name github-mailtrap
```

Then add Mailtrap package:

```console
dotnet add package Mailtrap -v 2.0.0 -s github-mailtrap
```

Optionally, you can add Mailtrap.Abstractions package:

```console
dotnet add package Mailtrap.Abstractions -v 2.0.0 -s github-mailtrap
```

### Configure
Add Mailtrap services to the DI container.

```csharp
using Mailtrap;
   
...
   
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

### Use
Now you can inject `IMailtrapClient` instance in any application service and use it to make API calls.

```csharp
using Mailtrap;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;
   

namespace MyAwesomeProject;


public sealed class SendEmailService : ISendEmailService
{
    private readonly IMailtrapClient _mailtrapClient;


    public SendEmailService(IMailtrapClient mailtrapClient)
    {
        _mailtrapClient = mailtrapClient;
    }


    public async Task DoWork()
    {
        try 
        {
            SendEmailRequest request = SendEmailRequest
                .Create()
                .From("john.doe@demomailtrap.com", "John Doe")
                .To("hero.bill@galaxy.net")
                .Subject("Invitation to Earth")
                .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");

            SendEmailResponse response = await _mailtrapClient
                .Email()
                .Send(request);
                
            string messageId = response.MessageIds.FirstOrDefault();
        }
        catch (MailtrapException mtex)
        {
            // handle Mailtrap API specific exceptions
        }
        catch (OperationCancelledException ocex)
        {
            // handle cancellation
        }
        catch (Exception ex)
        {
            // handle other exceptions
        }   
    }
}
```

## Examples

The repository includes comprehensive examples demonstrating various use cases and features:

### Email API/SMTP
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send an email (Transactional and Bulk streams)
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send an email with a template
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send a batch of emails (Transactional and Bulk streams)
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send emails with attachments

### Email Sandbox (Testing)
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send an email
- **[Email Sending](examples/Mailtrap.Example.Email.Send/)** - Send an email with a template
- **[Testing Messages](examples/Mailtrap.Example.TestingMessage/)** - Message management
- **[Attachments](examples/Mailtrap.Example.Attachment/)** - Working with email attachments in testing messages
- **[Inbox Management](examples/Mailtrap.Example.Inbox/)** - Inbox management
- **[Project Management](examples/Mailtrap.Example.Project/)** - Project management

### General
- **[Account Access](examples/Mailtrap.Example.AccountAccess/)** - Account access management
- **[Permissions](examples/Mailtrap.Example.Permissions/)** - Permissions management
- **[Account Management](examples/Mailtrap.Example.Account/)** - List accounts you have access to
- **[Billing](examples/Mailtrap.Example.Billing/)** - Billing information and usage statistics
- **[Sending Domains](examples/Mailtrap.Example.SendingDomain/)** - Domain verification
- **[Comprehensive API Usage](examples/Mailtrap.Example.ApiUsage/)** - Complete example showcasing multiple API features together

### Configuration & Setup
- **[Dependency Injection](examples/Mailtrap.Example.DependencyInjection/)** - Integration with ASP.NET Core DI container and configuration
- **[Factory Pattern](examples/Mailtrap.Example.Factory/)** - Using standalone client factory for scenarios without DI container

Each example includes detailed comments and demonstrates best practices for error handling, configuration, and resource management.

## Documentation
Please visit [Documentation Portal](https://mailtrap.github.io/mailtrap-dotnet/) for detailed setup, configuration and usage instructions.


## Contributing
We believe in the power of OSS and welcome any contributions to the library.  
Please refer to [Contributing Guide](CONTRIBUTING.md) for details.

## License
Licensed under the [MIT License](LICENSE.md).
