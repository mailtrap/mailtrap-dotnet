---
uid: send-email
---


# Sending Emails
This article covers different scenarios of forming and sending emails using Mailtrap API client.


## Creating request
Sending email API uses @Mailtrap.Emails.Requests.SendEmailRequest object to create a payload for the API request.  
Library provides two ways for creating instances of it.

### Fluent builder
There is a set of extensions in @Mailtrap.Emails.Requests.SendEmailRequestBuilder, which allow you to create requests in a fluent style:
```csharp
using Mailtrap.Emails.Requests;

...

var request = SendEmailRequest
    .Create()
    .From("john.doe@demomailtrap.com", "John Doe")
    .To("hero.bill@galaxy.net")
    .Cc("star.lord@galaxy.net")
    .Subject("Invitation to Earth")
    .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");
...
```

### Regular initialization
Alternatively, you can use object initialization and setup fields as usual:
```csharp
using Mailtrap.Emails.Requests;

...

var from = new EmailAddress("john.doe@demomailtrap.com", "John Doe");

var request = new SendEmailRequest
{
    From = from,
    ReplyTo = new EmailAddress("noreply@milkyway.gov"),
    Subject = "Invitation to Earth",
    TextBody = "Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John."
};

// You can specify up to 1000 recipients in each of To, Cc and Bcc fields.
// At least one of recipient collections must contain at least one recipient.
var to = new EmailAddress("hero.bill@galaxy.net");
request.To.Add(to);

var cc = new EmailAddress("star.lord@galaxy.net");
request.Cc.Add(cc);

request.Bcc.Add(from);
...
```

## Attaching files
You can attach files to emails, both for inlining (embedding) or as a standalone downloadable file:
```csharp
using Mailtrap.Emails.Requests;

...

var request = SendEmailRequest
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
    disposition: DispositionType.Attachment, // Downloadable
    mimeType: MediaTypeNames.Application.Pdf);

filePath = @"C:\files\logo.png";
fileName = "logo.png";
bytes = File.ReadAllBytes(filePath);
fileContent = Convert.ToBase64String(bytes);

request.Attach(
    content: fileContent,
    fileName: fileName,
    disposition: DispositionType.Inline, // For embedding
    mimeType: MediaTypeNames.Image.Png,
    contentId: "logo_1");
```


## Email from template
There is a possibility to create emails from predefined templates.  
You can create a template and obtain its ID [here](https://mailtrap.io/email_templates/).  
Then you can use it in emails:
```csharp
using Mailtrap.Emails.Requests;

...

var request = SendEmailRequest
    .Create()
    .From("john.doe@demomailtrap.com", "John Doe")
    .To("hero.bill@galaxy.net")
    .Template("60dca11e-0bc2-42ea-91a8-5ff196acb3f9") // ID of template obtained from https://mailtrap.io/email_templates/
    .TemplateVariables(new Dictionary<string, string>
    {
        { "name", "Bill" },
        { "sender", "John" }
    });
...
```

## Kitchen sink
Just to give an overview of all possible settings:
```csharp
using Mailtrap.Emails.Requests;

...

// Create new request
var request = SendEmailRequest.Create();

// Sender (Display name is optional)
request.From("john.doe@demomailtrap.com", "John Doe");

// Reply To (Display name is optional)
request.ReplyTo("no-reply@example.com");

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
    mimeType: MediaTypeNames.Application.Pdf);

// Add custom variables
request.CustomVariable("var_key", "var_value");

// Adding few at once also supported
request.CustomVariable(
    new("var_key_1", "var_value_1"),
    new("var_key_2", "var_value_2"),
    new("var_key_3", "var_value_3"));

// Add custom headers
request.Header("X-Custom-Header", "Custom Value");

// Adding few at once supported as well
request.Header(
    new("X-Custom-Header-1", "Custom Value 1"),
    new("X-Custom-Header-2", "Custom Value 2"),
    new("X-Custom-Header-3", "Custom Value 3"));
```


## Request validation
After creating a request instance, it is recommended to perform a validation on a client side to ensure sending won't throw validation exceptions and to minimize unnecessary HTTP round-trips. @Mailtrap.Emails.Requests.SendEmailRequest implements @Mailtrap.Core.Validation.IValidatable interface, which can be used to perform that task.  

@Mailtrap.Core.Validation.IValidatable.Validate method verifies request data and returns a @Mailtrap.Core.Validation.ValidationResult instance that contains validation result:
```csharp
using Mailtrap.Emails.Requests;

...

var request = new SendEmailRequest();

var validationResult = request.Validate();

if (validationResult.IsValid)
{
    // Send
}
else
{
    // Handle invalid request
}
```

Alternatively, you can use @Mailtrap.Core.Validation.ValidationResult.EnsureValidity(System.String) method as a gate, that throws @System.ArgumentException if validation fails:
```csharp
using Mailtrap.Emails.Requests;

...

try 
{
    var request = new SendEmailRequest();

    request.Validate().EnsureValidity(nameof(request)); // Will throw if request isn't valid.
}
catch (ArgumentException aex)
{
    // Handle validation issues
}
```

> [!NOTE]  
> Client implementation uses the latter approach internally, to ensure request validity before processing.  


## Using send API
Finally, after you have formed a valid request, you can send it:
```csharp
using Mailtrap;
using Mailtrap.Emails.Models;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;

...


private readonly IMailtrapClient _mailtrapClient;

try 
{
    SendEmailRequest request = SendEmailRequest
        .Create()
        .From("john.doe@demomailtrap.com", "John Doe")
        .To("hero.bill@galaxy.net")
        .Subject("Invitation to Earth")
        .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");

    if (request.Validate().IsValid)
    {
        using var cts = new CancellationTokenSource();

        SendEmailResponse response = await _mailtrapClient
            .Email()  // Will send email using API defined in client configuration
            .Send(request, cts.Token);
      
        string messageId = response.MessageIds.FirstOrDefault();
    }
    else
    {
        // handle validation issues
    }
}
catch (MailtrapApiException mtex)
{
   // handle Mailtrap specific exceptions
}
catch (ArgumentException aex)
{
   // handle request validation issues
}
catch (JsonException jex)
{
   // handle serialization issues
}
catch (HttpRequestException hrex)
{
   // handle HTTP errors
}
catch (OperationCancelledException ocex)
{
   // handle cancellation
}
catch (Exception ex)
{
   // handle other exceptions
}   
```

> [!IMPORTANT]  
> @Mailtrap.IMailtrapClient.Email will use send API defined by client configuration.


Additionally, you can always use specific send API (transactional, bulk or test) explicitly, to route emails to:
```csharp
var inboxId = 12345;
IEmailClient emailClient = _mailtrapClient.Test(inboxId); // Emails will be sent using Email Testing API
// IEmailClient emailClient = _mailtrapClient.Transactional(); // Emails will be sent using Email Sending API
// IEmailClient emailClient = _mailtrapClient.Bulk(); // Emails will be sent using Bulk Sending API

var response = await emailClient.Send(request);
```

> [!TIP]  
> @Mailtrap.IMailtrapClient.Transactional, @Mailtrap.IMailtrapClient.Bulk and @Mailtrap.IMailtrapClient.Test(System.Int64)
> are factory methods that will create new @Mailtrap.Emails.IEmailClient instance every time when called.  
> Thus in case when you need to perform multiple `Send()` calls to the same endpoint it will be good idea
> to spawn client once and then reuse it:
> ```csharp
> IEmailClient emailClient = _mailtrapClient.Bulk(); // Caching client instance
> 
> foreach(var request in requests)
> {
>     var response = await emailClient.Send(request);
> }
>```


## See also
More examples available [here](https://github.com/mailtrap/mailtrap-dotnet/tree/main/examples).
