namespace Mailtrap;


/// <summary>
/// A set of extension methods to configure Mailtrap API client services in <see cref="IServiceCollection"/>.
/// </summary>
public static class MailtrapClientServiceCollectionExtensions
{
    /// <summary>
    /// Adds required Mailtrap API client services to the <paramref name="services"/> collection.
    /// </summary>
    /// 
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> instance to configure.
    /// </param>
    /// 
    /// <returns>
    /// Updated <see cref="IServiceCollection"/> instance, so additional configuration calls can be chained.
    /// </returns>
    ///
    /// <remarks>
    /// This helper method exists for advanced scenarios, when you need to customize Mailtrap API client services setup
    /// along with fine-tuning of <see cref="HttpClient"/> configuration.<br />
    /// Please refer to documentation/samples for additional details.    
    /// </remarks>
    public static IServiceCollection AddMailtrapServices(this IServiceCollection services)
        => AddMailtrapServices<FactoryHttpClientProvider>(services);

    /// <summary>
    /// Adds Mailtrap API client to the <paramref name="services"/> collection.
    /// </summary>
    /// 
    /// <param name="services">
    /// <inheritdoc cref="AddMailtrapServices(IServiceCollection)" path="/param[@name='services']"/>
    /// </param>
    /// 
    /// <returns>
    /// The <see cref="IHttpClientBuilder"/> instance for configured <see cref="HttpClient"/>,
    /// so additional configuration calls can be chained.
    /// </returns>
    public static IHttpClientBuilder AddMailtrapClient(this IServiceCollection services)
    {
        Ensure.NotNull(services, nameof(services));

        return services
            .AddMailtrapServices()
            .AddHttpClient(Client.Name);
    }

    /// <summary>
    /// Adds Mailtrap API client to the <paramref name="services"/> collection
    /// and configures it using provided <paramref name="configuration"/> section.
    /// </summary>
    /// 
    /// <param name="services">
    /// <inheritdoc cref="AddMailtrapClient(IServiceCollection)" path="/param[@name='services']"/>
    /// </param>
    /// 
    /// <param name="configuration">
    /// <see cref="IConfiguration"/> instance to configure settings for Mailtrap API client.
    /// </param>
    /// 
    /// <inheritdoc cref="AddMailtrapClient(IServiceCollection)" path="/returns"/>
    public static IHttpClientBuilder AddMailtrapClient(this IServiceCollection services, IConfiguration configuration)
    {
        Ensure.NotNull(services, nameof(services));
        Ensure.NotNull(configuration, nameof(configuration));

        services.Configure<MailtrapClientOptions>(configuration);

        return services.AddMailtrapClient();
    }

    /// <summary>
    /// Adds Mailtrap API client to the <paramref name="services"/> collection
    /// and configures it using provided <paramref name="configure"/> delegate.
    /// </summary>
    /// 
    /// <param name="services">
    /// <inheritdoc cref="AddMailtrapClient(IServiceCollection)" path="/param[@name='services']"/>
    /// </param>
    /// 
    /// <param name="configure">
    /// Delegate to configure settings for Mailtrap API client.
    /// </param>
    /// 
    /// <inheritdoc cref="AddMailtrapClient(IServiceCollection)" path="/returns"/>
    public static IHttpClientBuilder AddMailtrapClient(this IServiceCollection services, Action<MailtrapClientOptions> configure)
    {
        Ensure.NotNull(services, nameof(services));
        Ensure.NotNull(configure, nameof(configure));

        services.Configure(configure);

        return services.AddMailtrapClient();
    }

    /// <summary>
    /// Adds Mailtrap API client to the <paramref name="services"/> collection
    /// and configures it using provided <paramref name="options"/>.
    /// </summary>
    /// 
    /// <param name="services">
    /// <inheritdoc cref="AddMailtrapClient(IServiceCollection)" path="/param[@name='services']"/>
    /// </param>
    /// 
    /// <param name="options">
    /// Options to configure settings for Mailtrap API client.
    /// </param>
    /// 
    /// <inheritdoc cref="AddMailtrapClient(IServiceCollection)" path="/returns"/>
    public static IHttpClientBuilder AddMailtrapClient(this IServiceCollection services, MailtrapClientOptions options)
    {
        Ensure.NotNull(services, nameof(services));
        Ensure.NotNull(options, nameof(options));

        return services.AddMailtrapClient(o => o.Init(options));
    }


    internal static IServiceCollection AddMailtrapServices<T>(this IServiceCollection services) where T : class, IHttpClientProvider
    {
        Ensure.NotNull(services, nameof(services));

        services
            .AddOptions()
            .PostConfigure<MailtrapClientOptions>(options =>
            {
                MailtrapClientOptionsValidator.Instance
                    .Validate(options)
                    .ToMailtrapValidationResult()
                    .EnsureValidity(nameof(MailtrapClientOptions));
            });

        services.TryAddSingleton<IHttpClientProvider, T>();
        services.TryAddSingleton<IHttpRequestMessageFactory, HttpRequestMessageFactory>();
        services.TryAddSingleton<IHttpRequestContentFactory, HttpRequestContentFactory>();
        services.TryAddSingleton<IHttpResponseHandlerFactory, HttpResponseHandlerFactory>();
        services.TryAddSingleton<IRestResourceCommandFactory, RestResourceCommandFactory>();
        services.TryAddSingleton<IEmailClientEndpointProvider, EmailClientEndpointProvider>();
        services.TryAddSingleton<IEmailClientFactory, EmailClientFactory>();

        services.TryAddTransient(services => services.GetRequiredService<IEmailClientFactory>().CreateDefault());
        services.TryAddTransient(services => services.GetRequiredService<IEmailClientFactory>().CreateBatchDefault());

        services.TryAddTransient<IMailtrapClient, MailtrapClient>();

        return services;
    }
}
