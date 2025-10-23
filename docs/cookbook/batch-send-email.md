---
uid: batch-send-email
---

# Sending Batch Emails
This article covers scenarios for forming and sending batch emails using the Mailtrap API client.

## Creating batch request
Batch email API uses @Mailtrap.Emails.Requests.BatchEmailRequest to create a payload for the API request.  
You can set up a base request and a collection of individual requests.

### Fluent builder
You can use the fluent builder extensions in @Mailtrap.Emails.Requests.BatchEmailRequestBuilder to create batch requests in a fluent style:
```csharp
using Mailtrap.Emails.Requests;

...

var batchRequest = BatchEmailRequest
    .Create()
    .Base(b => b
        .From("john.doe@demomailtrap.com", "John Doe")
        .Subject("Batch Invitation"))
    .Requests(r => r
        .To("hero.bill@galaxy.net")
        .Text("Dear Bill,\n\nSee you soon!"))
    .Requests(SendEmailRequest.Create()
        .From("john.cena@demomailtrap.com", " It's a John Cena") // Overwrite the base attributes
        .To("hero.bill@galaxy.net")
        .Cc("star.lord@galaxy.net")
        .Subject("Invitation to Earth"));
...
```

### Regular initialization
Alternatively, you can use object initialization:
```csharp
using Mailtrap.Emails.Requests;

...

var baseRequest = new EmailRequest
{
    From = new EmailAddress("john.doe@demomailtrap.com", "John Doe"),
    Subject = "Batch Invitation"
};

// You can specify up to 1000 recipients in total across the To, Cc, and Bcc fields (i.e. To + CC + Bcc <=1000).
// At least one of recipient collections must contain at least one recipient.
var requests = new List<SendEmailRequest>
{
    new SendEmailRequest
    {
        To = { new EmailAddress("hero.bill@galaxy.net") },
        TextBody = "Dear Bill,\n\nSee you soon!"
    },
    new SendEmailRequest
    {
        Cc = { new EmailAddress("star.lord@galaxy.net") },
        TextBody = "Dear Lord,\n\nSee you soon!"
    },
    new SendEmailRequest
    {
        Bcc = { new EmailAddress("sara.conor@galaxy.net") },
        TextBody = "Dear Sara,\n\nKeep you posted!"
    }
};

var batchRequest = new BatchEmailRequest
{
    Base = baseRequest,
    Requests = requests
};
```

> [!NOTE]
> You can specify up to 500 requests in a single batch.  
> Each request will inherit properties from the base request unless overridden.

## Attaching files
You can attach files to Base and individual requests in the batch, just as with single emails in two ways for inlining (embedding) or as a standalone downloadable file:
```csharp
using Mailtrap.Emails.Requests;

...

var batchRequest = BatchEmailRequest
    .Create()
    .Base(b => b
        .From("john.doe@demomailtrap.com", "John Doe")
        .Subject("Latest Notes")
        .Attach(
            content: Convert.ToBase64String(File.ReadAllBytes(@"C:\files\note.pdf")),
            fileName: "note.pdf",
            disposition: DispositionType.Attachment, // Downloadable
            mimeType: MediaTypeNames.Application.Pdf))
    .Requests(r => r
        .To("hero.bill@galaxy.net")
        .Text("Dear Bill,\n\nSee you soon!")
        .Attach(
            content: Convert.ToBase64String(File.ReadAllBytes(@"C:\files\preview.pdf")),
            fileName: "preview.pdf",
            disposition: DispositionType.Inline, // For embedding
            mimeType: MediaTypeNames.Application.Pdf));
```

## Email from template
You can use templates in batch requests as well. 
You can create a template and obtain its ID via Mailtrap.EmailTemplates APIs.
Then you can use it in emails:
```csharp
using Mailtrap.Emails.Requests;

...

var batchRequest = BatchEmailRequest
    .Create()
    .Base(b => b.From("john.doe@demomailtrap.com", "John Doe"))
    .Requests(r => r
        .To("hero.bill@galaxy.net")
        .Template("60dca11e-0bc2-42ea-91a8-5ff196acb3f9")
        .TemplateVariables(new Dictionary<string, string>
        {
            { "name", "Bill" },
            { "sender", "John" }
        }));
```

## Kitchen sink
Overview of all possible settings for batch requests:
```csharp
using Mailtrap.Emails.Requests;

...

var batchRequest = BatchEmailRequest.Create();

batchRequest.Base(b => b
    .From("john.doe@demomailtrap.com", "John Doe")
    .ReplyTo("no-reply@example.com")
    .Subject("Batch Invitation"));

// Categorize
batchRequest.Base
    .Category("Invitation");

// HTML body.
// At least one of Text or Html body must be specified.
batchRequest.Base
    .Html(
        "<h2>Greetings, Bill!</h2>" +
        "<p>It will be a great pleasure to see you on our blue planet next weekend.</p>" +
        "<p>Regards,<br/>" +
        "John</p>");

// Add custom variables
batchRequest.Base
    .CustomVariable("var_key", "var_value");

// Add custom headers
batchRequest.Base
    .Header(
    new("X-Custom-Header-1", "Custom Value 1"),
    new("X-Custom-Header-2", "Custom Value 2"),
    new("X-Custom-Header-3", "Custom Value 3"));

batchRequest.Requests(r => r
    .From("john@demo.com", "John R. Doe")
    .To("hero.bill@galaxy.net")
    .Cc("steel.rat@galaxy.net", "James")
    .Bcc(new EmailAddress("first@domain.com"), new EmailAddress("second@domain.com"))
    .Html("<h2>Greetings, Bill!</h2><p>See you soon!</p>")
    .Text("Dear Bill,\n\nSee you soon!")
    .Attach(
        content: Convert.ToBase64String(File.ReadAllBytes(@"C:\files\preview.pdf")),
        fileName: "preview.pdf",
        disposition: DispositionType.Attachment,
        mimeType: MediaTypeNames.Application.Pdf)
    .CustomVariable("var_key", "var_value")
    .Header("X-Custom-Header", "Custom Value"));
```

## Request validation
After creating a batch request, validate it on a client side to ensure it meets API requirements and sending won't throw validation exceptions it also will minimize unnecessary HTTP round-trips.
@Mailtrap.Emails.Requests.BatchEmailRequest implements @Mailtrap.Core.Validation.IValidatable interface, which can be used to perform that task.  

@Mailtrap.Core.Validation.IValidatable.Validate method verifies request data and returns a @Mailtrap.Core.Validation.ValidationResult instance that contains validation result:
```csharp
using Mailtrap.Emails.Requests;

...

var batchRequest = new BatchEmailRequest();

var validationResult = batchRequest.Validate();

if (validationResult.IsValid)
{
    // Send
}
else
{
    // Handle invalid batch request
}
```

Alternatively, use `EnsureValidity` to throw on validation errors:
```csharp
using Mailtrap.Emails.Requests;

...

try 
{
    var batchRequest = new BatchEmailRequest();

    batchRequest.Validate().EnsureValidity(nameof(batchRequest)); // Will throw if request isn't valid.
}
catch (ArgumentException aex)
{
    // Handle validation issues
}
```

> [!NOTE]
> The client validates batch requests before sending.

## Using batch send API
After forming a valid batch request, send it using the batch API:
```csharp
using Mailtrap;
using Mailtrap.Emails.Models;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;

...

private readonly IMailtrapClient _mailtrapClient;

try 
{
    BatchEmailRequest batchRequest = BatchEmailRequest
        .Create()
        .Base(b => b.From("john.doe@demomailtrap.com", "John Doe"))
        .Requests(r => r.To("hero.bill@galaxy.net").Text("Dear Bill,\n\nSee you soon!"));

    if (batchRequest.Validate().IsValid)
    {
        using var cts = new CancellationTokenSource();

        BatchSendEmailResponse response = await _mailtrapClient
            .BatchEmail()
            .Send(batchRequest, cts.Token);

        var messageIds = response.MessageIds;
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
catch (OperationCanceledException ocex)
{
   // handle cancellation
}
catch (Exception ex)
{
   // handle other exceptions
}   
```

> [!IMPORTANT]
> @Mailtrap.IMailtrapClient.BatchEmail will use the batch send API as defined in client configuration.

Additionally, you can always use specific send API (transactional, bulk or test) explicitly, to route emails to:
```csharp
var inboxId = 12345;
IBatchEmailClient batchEmailClient = _mailtrapClient.BatchTest(inboxId); // Batch Emails will be sent using Email Testing API
// IBatchEmailClient batchEmailClient = _mailtrapClient.BatchTransactional(); // Batch Emails will be sent using Email Sending API
// IBatchEmailClient batchEmailClient = _mailtrapClient.BatchBulk(); // Batch Emails will be sent using Bulk Sending API

var response = await batchEmailClient.Send(request);
```

> [!TIP]  
> @Mailtrap.IMailtrapClient.BatchTransactional, @Mailtrap.IMailtrapClient.BatchBulk and @Mailtrap.IMailtrapClient.BatchTest(System.Int64)
> are factory methods that will create new @Mailtrap.Emails.IBatchEmailClient instance every time when called.  
> Thus, if you need to perform multiple `Send()` calls to the same endpoint, it will be a good idea
> to spawn client once and then reuse it:
> ```csharp
> IBatchEmailClient batchEmailClient = _mailtrapClient.BatchBulk(); // Caching client instance
> 
> foreach(var request in batchRequests)
> {
>     var response = await batchEmailClient.Send(request);
> }
>```

## See also
More examples available [Mailtrap .NET examples on GitHub](https://github.com/railsware/mailtrap-dotnet/tree/main/examples).
