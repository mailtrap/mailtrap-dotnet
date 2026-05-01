namespace Mailtrap;


/// <summary>
/// Default <see cref="IMailtrapClientFactory"/> implementation.
/// </summary>
/// 
/// <remarks>
/// <see cref="IMailtrapClient"/> instances, produced by the factory, can be used in any manner,
/// since they are designed to use unit-of-work pattern under the hood, ensuring proper disposal of resources,
/// as soon as any operation is completed.
/// </remarks>

public sealed class MailtrapClientFactory : IMailtrapClientFactory
{
    private readonly ServiceProvider _serviceProvider;

    #region Constructors

    /// <summary>
    /// Constructor to create a new instance of <see cref="MailtrapClientFactory"/> 
    /// with provided <paramref name="configuration"/>
    /// and optional <see cref="HttpClient"/> configuration delegate.
    /// </summary>
    /// 
    /// <param name="configuration">
    /// Options instance to configure factory.
    /// </param>
    /// 
    /// <param name="configure">
    /// Optional delegate to configure underlying <see cref="HttpClient"/>.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="configuration"/> is <see langword="null"/>.
    /// </exception>
    public MailtrapClientFactory(
        MailtrapClientOptions configuration,
        Action<IHttpClientBuilder>? configure = default)
    {
        Ensure.NotNull(configuration, nameof(configuration));

        var serviceCollection = new ServiceCollection();

        var httpClientBuilder = serviceCollection.AddMailtrapClient(configuration);

        configure?.Invoke(httpClientBuilder);

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// Constructor to create a new instance of <see cref="MailtrapClientFactory"/> 
    /// using provided <paramref name="configuration"/>
    /// and preconfigured external <see cref="HttpClient"/> instance.
    /// </summary>
    /// 
    /// <param name="configuration">
    /// Options instance to configure factory.
    /// </param>
    /// 
    /// <param name="httpClient">
    /// External instance to use within factory.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="configuration"/> or <paramref name="httpClient"/> is <see langword="null"/>.
    /// </exception>
    /// 
    /// <remarks>
    /// Factory won't dispose captured <paramref name="httpClient"/> instance upon disposal,
    /// so it is responsibility of the caller to manage its lifecycle properly.
    /// </remarks>
    public MailtrapClientFactory(
        MailtrapClientOptions configuration,
        HttpClient httpClient)
    {
        Ensure.NotNull(configuration, nameof(configuration));
        Ensure.NotNull(httpClient, nameof(httpClient));

        var serviceCollection = new ServiceCollection();

        serviceCollection.Configure<MailtrapClientOptions>(options => options.Init(configuration));

        serviceCollection.AddMailtrapServices<StaticHttpClientProvider>();

        serviceCollection.AddSingleton(httpClient);

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// Shortcut constructor to create a new instance of <see cref="MailtrapClientFactory"/> 
    /// using provided <paramref name="apiToken"/>
    /// and optional <see cref="HttpClient"/> configuration delegate.
    /// <para>
    /// All other configuration settings will be set to default values.
    /// </para>
    /// </summary>
    /// 
    /// <param name="apiToken">
    /// API authentication token.
    /// </param>
    /// 
    /// <param name="configure">
    /// Optional delegate to configure underlying <see cref="HttpClient"/>.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="apiToken"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    public MailtrapClientFactory(
        string apiToken,
        Action<IHttpClientBuilder>? configure = default)
        : this(new MailtrapClientOptions(apiToken), configure)
    { }

    /// <summary>
    /// Shortcut constructor to create a new instance of <see cref="MailtrapClientFactory"/>
    /// using provided <paramref name="apiToken"/>
    /// and preconfigured external <see cref="HttpClient"/> instance.
    /// <para>
    /// All other configuration settings will be set to default values.
    /// </para>
    /// </summary>
    /// 
    /// <param name="apiToken">
    /// API authentication token.
    /// </param>
    /// 
    /// <param name="httpClient">
    /// External <see cref="HttpClient"/> instance to use within factory.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="apiToken"/> is <see langword="null"/> or <see cref="string.Empty"/>. <br/>
    /// When <paramref name="httpClient"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Factory won't dispose captured <paramref name="httpClient"/> instance upon disposal,
    /// so it is responsibility of the caller to manage its lifecycle properly.
    /// </remarks>
    public MailtrapClientFactory(
        string apiToken,
        HttpClient httpClient)
        : this(new MailtrapClientOptions(apiToken), httpClient)
    { }

    #endregion



    #region IMailtrapClientFactory

    /// <inheritdoc/>
    public IMailtrapClient CreateClient() => _serviceProvider.GetRequiredService<IMailtrapClient>();

    /// <inheritdoc/>
    public IMailtrapOrganizationClient CreateOrganizationClient() => _serviceProvider.GetRequiredService<IMailtrapOrganizationClient>();

    #endregion



    #region IDisposable

    /// <summary>
    /// Disposes internal <see cref="IServiceProvider"/> instance,
    /// that is used to spawn <see cref="IMailtrapClient"/> implementation instances.
    /// </summary>
    public void Dispose() => _serviceProvider.Dispose();

    #endregion
}
