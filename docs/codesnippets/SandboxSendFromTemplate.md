---
uid: snippets.sandbox-send-template-email
---

# Sandbox send
Short example of sending email via sandbox link.

```csharp
using Mailtrap;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;
try
{
    var apiToken = "<API-TOKEN>";
    var sandboxId = <SANDBOX-ID>;                        // ID of sandbox
    using var mailtrapClientFactory = new MailtrapClientFactory(apiToken);
    IMailtrapClient mailtrapClient = mailtrapClientFactory.CreateClient();
    SendEmailRequest request = SendEmailRequest
        .Create()
        .From("hello@demomailtrap.co", "Mailtrap Test")
        .To("world@demomailtrap.co")
        .Template("<TEMPLATE-ID>")                        // ID of Email template
        .TemplateVariables(new Dictionary<string, object> // Optional parameters
        {
            { "company_info_name", "Test_Company_info_name" },
            { "company_info_address", "Test_Company_info_address" },
            { "company_info_city", "Test_Company_info_city" },
            { "company_info_zip_code", "Test_Company_info_zip_code" },
            { "company_info_country", "Test_Company_info_country" }
        });
    SendEmailResponse? response = await mailtrapClient
        .Test(sandboxId)
        .Send(request);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while sending email: {0}", ex);
}
```

[!INCLUDE [api-token-caution](../includes/api-token-caution.md)]
