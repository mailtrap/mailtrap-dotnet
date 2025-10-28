---
uid: snippets.simple-send-email
---

# Send an email
Short example of sending a simple email.

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
        .Category("Integration Test")
        .Text("Congrats for sending test email with Mailtrap!");
    SendEmailResponse? response = await mailtrapClient
        .Email()
        .Send(request);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while sending email: {0}", ex);
}
```

[!INCLUDE [api-token-caution](../includes/api-token-caution.md)]
