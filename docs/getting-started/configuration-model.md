---
uid: configuration-model
---


# Configuration Model

[!INCLUDE [api-token-caution](../includes/api-token-caution.md)]

Regardless of the configuration approach used for Mailtrap API client setup, all of them are using unified configuration model @Mailtrap.Configuration.MailtrapClientOptions under the hood.

In the common case, the only required configuration setting is @Mailtrap.Configuration.MailtrapClientOptions.ApiToken.  
Default is empty string, which will cause configuration validation to fail if left untouched.  

There is a dedicated [constructor](xref:Mailtrap.Configuration.MailtrapClientOptions.%23ctor(System.String)), that takes string containing API token as a single parameter:
```csharp
using Mailtrap.Configuration;

...

var config = new MailtrapClientOptions("<API_TOKEN>");
```  

Other parameters receive defaults:
 - JSON minification is disabled
 - Default method for sending emails is set to transactional

But in case of need they can be changed to custom values during object construction or using configuration binding.
```cs
var config = new MailtrapClientOptions("<API_TOKEN>")
{
    PrettyJson = true,
    UseBulkApi = true
};
```


## Authentication
@Mailtrap.Configuration.MailtrapClientOptions.ApiToken is used to set a token to authorize client requests to Mailtrap API.  
This is the only required setting, that needs to be set explicitly, since by default it is set to empty string,
which isn't a valid value and will throw an exception if left unchanged.
```cs
var config = new MailtrapClientOptions("<API_TOKEN>");
```

## Default Send API
A combination of @Mailtrap.Configuration.MailtrapClientOptions.UseBulkApi and @Mailtrap.Configuration.MailtrapClientOptions.SandboxId parameters
is used to define default API for sending emails.  

By default, if these parameters are not specified, transactional API will be used:
```cs
var config = new MailtrapClientOptions("<API_TOKEN>");

...

// Will send email as transactional.
var response = await _mailtrapClient.Email().Send(request);
```

If @Mailtrap.Configuration.MailtrapClientOptions.SandboxId contains valid Sandbox ID, default send API will be set to sandbox,
and all emails sent through @Mailtrap.IMailtrapClient.Email will be routed to the specified sandbox.

```cs
var config = new MailtrapClientOptions("<API_TOKEN>")
{
    SandboxId = 1234,
    UseBulkApi = true // Will be ignored, since SandboxId is set
};

...

// Will send email to the sandbox with ID '1234', specified by configuration.
var response = await _mailtrapClient.Email().Send(request);
```

In case Sandbox is not specified (set to null), default send API will be set to transactional or bulk,
depending on the @Mailtrap.Configuration.MailtrapClientOptions.UseBulkApi flag.

```cs
var config = new MailtrapClientOptions("<API_TOKEN>")
{
    UseBulkApi = true
};

...
// Will send email as bulk.
var response = await _mailtrapClient.Email().Send(request);
```


## Serialization
@Mailtrap.Configuration.MailtrapClientOptions.PrettyJson flag can be opted-in to produce pretty JSON for outgoing HTTP requests,
which can be helpful for debugging and log analysis.

By default it is set to `false`, and JSON that is sent in HTTP requests is minified.
```cs
var config = new MailtrapClientOptions("<API_TOKEN>");
```

```json
{"to":[{"email":"john_doe@example.com","name":"John Doe"}],"from":{"email":"sales@example.com","name":"Example Sales Team"},...}
```

But when opted-in, outgoing HTTP request body will contain human-friendly JSON.

```cs
var config = new MailtrapClientOptions("<API_TOKEN>")
{
    PrettyJson = true
};
```

```json
{
  "to": [
    {
      "email": "john_doe@example.com",
      "name": "John Doe"
    }
  ],
  "from": {
    "email": "sales@example.com",
    "name": "Example Sales Team"
  },
  ...
}
```


## What's next
If you are using DI container in your application, please visit [this article](xref:configuration-client-di) for detailed instructions how to configure and inject Mailtrap API client.

Alternatively, you can use [standalone factory](xref:configuration-client-factory) for configuring and spawning Mailtrap API client instances.
