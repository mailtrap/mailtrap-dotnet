---
uid: configuration-client-factory
---


# Client Configuration - Standalone Factory

[!INCLUDE [api-token-caution](../includes/api-token-caution.md)]

While [DI pattern](xref:configuration-client-di) is recommended approach to configure and use Mailtrap API client, alternatively, @Mailtrap.MailtrapClientFactory can be used to spawn client instances, when usage of DI container isn't possible or desired.  
In most cases factory configuration is pretty similar to the DI container one.

[!INCLUDE [factory-disposal-caution](../includes/factory-disposal-caution.md)]


## Basic usage
In the simplest scenario getting client is as simple as the following:
```csharp
using Mailtrap;

...

// Creating factory instance
using var factory = new MailtrapClientFactory("<API_TOKEN>");

// Spawning client
IMailtrapClient client = factory.CreateClient();
```


## Configuration with @Mailtrap.Configuration.MailtrapClientOptions instance
Alternatively, the same way as with DI container, factory can be configured using pre-created instance of @Mailtrap.Configuration.MailtrapClientOptions with additional parameters set to non-default values:
```csharp
using Mailtrap;
using Mailtrap.Configuration;

...

// Creating new instance of `MailtrapClientOptions`
var config = new MailtrapClientOptions("<API_TOKEN>")
{
    InboxId = 12345
};

// Creating factory instance        
using var factory = new MailtrapClientFactory(config);

// Spawning Mailtrap API client
IMailtrapClient client = factory.CreateClient();
```


## Advanced @System.Net.Http.HttpClient configuration
Optionally, a delegate for advanced HTTP pipeline configuration, which takes @Microsoft.Extensions.DependencyInjection.IHttpClientBuilder as a parameter, can be passed as a second parameter to factory constructor:
```csharp
using Mailtrap;

...

// Creating factory instance
using var factory = new MailtrapClientFactory("<API_TOKEN>", (IHttpClientBuilder builder) =>
{
    // Do any required HttpClient configuration
    // builder.AddStandardResilienceHandler();
    // builder.AddExtendedHttpClientLogging();
    // etc.
});

// Spawning Mailtrap API client
IMailtrapClient client = factory.CreateClient();
```

As an alternative to the aforementioned scenario, external pre-configured instance of @System.Net.Http.HttpClient can be passed instead of delegate.
```csharp
using System.Net.Http;
using Mailtrap;

...

// Assuming there is some already existing instance.
using var httpClient = new HttpClient();

// Creating factory instance
using var factory = new MailtrapClientFactory("<API_TOKEN>", httpClient);

// Spawning Mailtrap API client
IMailtrapClient client = factory.CreateClient();
```
> [!IMPORTANT]  
> Please consider that in the latter scenario the caller is responsible of the @System.Net.Http.HttpClient lifetime control and proper disposal.


## What's next
Additional examples of the Mailtrap API client factory configuration and usage can be found on [GitHub](https://github.com/mailtrap/mailtrap-dotnet/blob/main/examples/Mailtrap.Example.Factory/Program.cs)

[!INCLUDE [cookbook-link](../includes/cookbook-link.md)]

[!INCLUDE [examples-link](../includes/examples-link.md)]

[!INCLUDE [api-reference-link](../includes/api-reference-link.md)]
