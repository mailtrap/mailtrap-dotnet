---
uid: configuration-client-di
---


# Client Configuration - DI Container

[!INCLUDE [api-token-caution](../includes/api-token-caution.md)]

When using hosting model from MS, Mailtrap API client uses [Options Pattern](https://learn.microsoft.com/dotnet/core/extensions/options) for configuration under the hood.  
Any of available approaches to configure `IOptions<MailtrapClientOptions>` in the DI container can be used and client will consume them.  
Meanwhile, library provides few handy shortcut extensions to simplify configuring Mailtrap API client in @Microsoft.Extensions.DependencyInjection.IServiceCollection.  


## Configuration with @Microsoft.Extensions.Configuration.IConfiguration abstraction
Hosting model supports a number of configuration providers out-of-the-box:
 - configuration files
 - environment variables
 - command line arguments
 - etc.

More details can be found [here](https://learn.microsoft.com/dotnet/core/extensions/configuration)

Assuming the following Mailtrap configuration in `appsettings.json`
```json
"Mailtrap": {
  "ApiToken": "<API_TOKEN>",
  "UseBulkApi": true
}
```

Then section from configuration can be loaded and used to configure Mailtrap API client with it:
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mailtrap;

...

// Creating host builder with defaults
HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);
        
// Loading configuration section
IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

// Adding Mailtrap API client to the container
hostBuilder.Services.AddMailtrapClient(config);

// Building the host
IHost host = hostBuilder.Build();
```

Alternatively, more granular configuration flow can be used:
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mailtrap;

...

// Creating host builder with defaults
HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

// Loading configuration section
IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

// Configuring client options
hostBuilder.Services.Configure<MailtrapClientOptions>(config);

// Do any additional custom configuration, overrides, etc.
hostBuilder.Services.PostConfigure<MailtrapClientOptions>(options =>
{
    options.ApiToken = Environment.GetEnvironmentVariable("MAILTRAP_TOKEN");
});

// Adding Mailtrap API client core services to the container
// This won't add nor configure HttpClient, thus it has to be done separately
hostBuilder.Services.AddMailtrapServices();

// Adding and optionally configuring HttpClient
hostBuilder.Services
    .AddHttpClient("Mailtrap")
    .AddStandardResilienceHandler()
    .AddExtendedHttpClientLogging();

// Building the host
IHost host = hostBuilder.Build();
```


## Configuration with delegate
Mailtrap API client settings can be set up programmatically, using extension overload accepting configuration delegate:
```csharp
using Microsoft.Extensions.Hosting;
using Mailtrap;

...

// Creating host builder with defaults
HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);
        
// Adding Mailtrap API client to the container
hostBuilder.Services.AddMailtrapClient((MailtrapClientOptions options) =>
{
    options.ApiToken = "<API_TOKEN>";
    options.PrettyJson = true;
});

// Building the host
IHost host = hostBuilder.Build();
```


## Configuration with @Mailtrap.Configuration.MailtrapClientOptions instance
Similarly to the previous section, configuration can be done by passing pre-created instance of @Mailtrap.Configuration.MailtrapClientOptions:
```csharp
using Microsoft.Extensions.Hosting;
using Mailtrap;

...

// Creating host builder with defaults
HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

// Creating new instance of `MailtrapClientOptions`
var config = new MailtrapClientOptions("<API_TOKEN>")
{
    InboxId = 4321
};
        
// Adding Mailtrap API client to the container
hostBuilder.Services.AddMailtrapClient(config);

// Building the host
IHost host = hostBuilder.Build();
```


## Advanced @System.Net.Http.HttpClient configuration
All `AddMailtrapClient` extension method overloads return @Microsoft.Extensions.DependencyInjection.IHttpClientBuilder?displayProperty=name instance.  
Thus a fine-grain tuning of the @System.Net.Http.HttpClient configuration can be done:
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mailtrap;

...

// Creating host builder with defaults
HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);
        
// Loading configuration section
IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

// Adding Mailtrap API client to the container
IHttpClientBuilder httpClientBuilder = hostBuilder.Services.AddMailtrapClient(config);

// Do any required HttpClient configuration
// httpClientBuilder.AddStandardResilienceHandler();
// etc.

// Building the host
IHost host = hostBuilder.Build();
```


## What's next
Additional examples for client configuration within DI container can be found on [GitHub](https://github.com/mailtrap/mailtrap-dotnet/blob/main/examples/Mailtrap.Example.DependencyInjection/Program.cs)

[!INCLUDE [cookbook-link](../includes/cookbook-link.md)]

[!INCLUDE [examples-link](../includes/examples-link.md)]

[!INCLUDE [api-reference-link](../includes/api-reference-link.md)]
