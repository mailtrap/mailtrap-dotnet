using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using Mailtrap;
using Mailtrap.Configuration;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;
using Microsoft.Extensions.DependencyInjection;


/// <summary>
/// Example demonstrating how to use standalone Mailtrap API client factory to spawn client instances.
/// </summary>
[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Example")]
internal sealed class Program
{
    private static async Task Main()
    {
        try
        {
            // Factory implements IDisposable and should be disposed properly after use.
            using MailtrapClientFactory mailtrapClientFactory = CreateFactoryWithToken();

            IMailtrapClient mailtrapClient = mailtrapClientFactory.CreateClient();

            SendEmailRequest request = SendEmailRequest
                .Create()
                .From("john.doe@demomailtrap.com", "John Doe")
                .To("hero.bill@galaxy.net")
                .Subject("Invitation to Earth")
                .Text("Dear Bill,\n\nIt will be a great pleasure to see you on our blue planet next weekend.\n\nBest regards, John.");

            SendEmailResponse? response = await mailtrapClient
                .Email() // Default client, depends on configuration
                .Send(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while sending email: {0}", ex);
            Environment.FailFast(ex.Message);
            throw;
        }
    }


    /// <summary>
    /// The simplest case, which uses API token to configure Mailtrap API client factory.
    /// </summary>
    private static MailtrapClientFactory CreateFactoryWithToken()
    {
        // For sure you should not hardcode apiKey for security reasons, and use environment variables
        // or configuration files instead.
        var apiToken = "<API-KEY>";

        return new MailtrapClientFactory(apiToken);
    }

    /// <summary>
    /// Other simple one, which uses configuration object to configure Mailtrap API client factory.
    /// </summary>
    private static MailtrapClientFactory CreateFactoryWithOptions()
    {
        var config = new MailtrapClientOptions("<API-KEY>")
        {
            SandboxId = 12345
        };

        return new MailtrapClientFactory(config);
    }

    /// <summary>
    /// Advanced scenario, where custom standalone HttpClient is used.
    /// </summary>
    private static MailtrapClientFactory CreateFactoryWithStandaloneHttpClient()
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        // Please ensure you manage HttpClient and HttpMeassageHandler lifecycle properly
        // in the real-world application
        var httpMessageHandler = new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(1)
        };
        var httpClient = new HttpClient(httpMessageHandler);
#pragma warning restore CA2000 // Dispose objects before losing scope

        return new MailtrapClientFactory("<API-TOKEN>", httpClient);
    }

    /// <summary>
    /// More advanced scenario, where HttpClient is configured with custom fine-grain settings delegate.
    /// </summary>
    private static MailtrapClientFactory CreateFactoryWithAdvancedHttpClientConfiguration()
    {
        var config = new MailtrapClientOptions
        {
            ApiToken = "<API-KEY>", // Required
            UseBulkApi = true
        };

        return new MailtrapClientFactory(config, httpClientBuilder =>
        {
            // Adding resilience handler
            httpClientBuilder.AddStandardResilienceHandler();

            // Adding hedging handler
            httpClientBuilder.AddStandardHedgingHandler();

            // Configuring HttpClient
            httpClientBuilder.ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
            });

            // Adding proxy
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler()
                {
                    Proxy = new WebProxy("proxy.mailtrap.io", 8080),
                    CheckCertificateRevocationList = true
                };
            });

            // Configure detailed HTTP request logging
            httpClientBuilder.AddExtendedHttpClientLogging();
        });
    }
}
