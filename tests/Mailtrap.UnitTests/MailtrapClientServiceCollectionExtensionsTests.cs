namespace Mailtrap.UnitTests;


[TestFixture]
internal sealed class MailtrapClientServiceCollectionExtensionsTests
{
    private string _apiToken { get; } = "token";


    [Test]
    public void AddMailtrapClient_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        var act = () => MailtrapClientServiceCollectionExtensions.AddMailtrapClient(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_ShouldAddRequiredServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient();

        VerifyHttpClientFactory(serviceCollection);

        VerifyMailtrapServices(serviceCollection);
    }


    [Test]
    public void AddMailtrapClient_Configuration_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        var config = Mock.Of<IConfiguration>();

        var act = () => MailtrapClientServiceCollectionExtensions.AddMailtrapClient(null!, config);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_Configuration_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
    {
        var services = Mock.Of<ServiceCollection>();
        IConfigurationSection? config = null;

        var act = () => services.AddMailtrapClient(config!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_Configuration_ShouldConfigureOptions_WhenConfigurationProvided()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            ["Mailtrap:ApiToken"] = _apiToken
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient(configuration.GetSection("Mailtrap"));

        VerifyOptions(serviceCollection);

        var services = serviceCollection.BuildServiceProvider();

        var options = services.GetRequiredService<IOptions<MailtrapClientOptions>>();

        options.Should().NotBeNull();
        options.Value.Should().NotBeNull();
        options.Value.ApiToken.Should().Be(_apiToken);
    }

    [Test]
    public void AddMailtrapClient_Configuration_ShouldAddRequiredServices()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            ["Mailtrap:Authentication:ApiToken"] = _apiToken
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient(configuration.GetSection("Mailtrap"));

        VerifyMailtrap(serviceCollection);
    }


    [Test]
    public void AddMailtrapClient_Delegate_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        var act = () => MailtrapClientServiceCollectionExtensions.AddMailtrapClient(null!, options => { });

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_Delegate_ShouldThrowArgumentNullException_WhenDelegateIsNull()
    {
        var services = Mock.Of<ServiceCollection>();
        Action<MailtrapClientOptions>? config = null;

        var act = () => services.AddMailtrapClient(config!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_Delegate_ShouldConfigureOptions_WhenConfigurationProvided()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient(options =>
        {
            options.ApiToken = _apiToken;
        });

        VerifyOptions(serviceCollection);

        var services = serviceCollection.BuildServiceProvider();

        var options = services.GetRequiredService<IOptions<MailtrapClientOptions>>();

        options.Should().NotBeNull();
        options.Value.Should().NotBeNull();
        options.Value.ApiToken.Should().Be(_apiToken);
    }

    [Test]
    public void AddMailtrapClient_Delegate_ShouldAddRequiredServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient(
            options =>
            {
                options.ApiToken = _apiToken;
            });

        VerifyMailtrap(serviceCollection);
    }


    [Test]
    public void AddMailtrapClient_Options_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        var config = Mock.Of<MailtrapClientOptions>();

        var act = () => MailtrapClientServiceCollectionExtensions.AddMailtrapClient(null!, config);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_Options_ShouldThrowArgumentNullException_WhenOptionsIsNull()
    {
        var services = Mock.Of<ServiceCollection>();
        MailtrapClientOptions? config = null;

        var act = () => services.AddMailtrapClient(config!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapClient_Options_ShouldConfigureOptions_WhenConfigurationProvided()
    {
        var config = new MailtrapClientOptions(_apiToken);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient(config);

        VerifyOptions(serviceCollection);

        var services = serviceCollection.BuildServiceProvider();

        var options = services.GetRequiredService<IOptions<MailtrapClientOptions>>();

        options.Should().NotBeNull();
        options.Value.Should().NotBeNull();
        options.Value.ApiToken.Should().Be(_apiToken);
    }

    [Test]
    public void AddMailtrapClient_Options_ShouldAddRequiredServices()
    {
        var config = new MailtrapClientOptions(_apiToken);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapClient(config);

        VerifyMailtrap(serviceCollection);
    }


    [Test]
    public void AddMailtrapServices_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        var act = () => MailtrapClientServiceCollectionExtensions.AddMailtrapServices(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddMailtrapServices_ShouldAddRequiredServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMailtrapServices();

        VerifyMailtrapServices(serviceCollection);
    }



    private static void VerifyOptions(ServiceCollection serviceCollection)
    {
        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IConfigureOptions<MailtrapClientOptions>) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationInstance != null);
    }

    private static void VerifyHttpClientFactory(ServiceCollection serviceCollection)
    {
        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IHttpClientFactory) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationFactory != null);
    }

    private static void VerifyMailtrapServices(ServiceCollection serviceCollection)
    {
        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IOptions<>) &&
            s.Lifetime == ServiceLifetime.Singleton);

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IHttpClientProvider) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationType == typeof(FactoryHttpClientProvider));

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IHttpRequestContentFactory) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationType == typeof(HttpRequestContentFactory));

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IHttpRequestMessageFactory) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationType == typeof(HttpRequestMessageFactory));

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IEmailClientEndpointProvider) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationType == typeof(EmailClientEndpointProvider));

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IEmailClientFactory) &&
            s.Lifetime == ServiceLifetime.Singleton &&
            s.ImplementationType == typeof(EmailClientFactory));

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IMailtrapClient) &&
            s.Lifetime == ServiceLifetime.Transient &&
            s.ImplementationType == typeof(MailtrapClient));

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(ISendEmailClient) &&
            s.Lifetime == ServiceLifetime.Transient &&
            s.ImplementationFactory != null);

        serviceCollection.Should().Contain(s =>
            s.ServiceType == typeof(IBatchEmailClient) &&
            s.Lifetime == ServiceLifetime.Transient &&
            s.ImplementationFactory != null);
    }

    private static void VerifyMailtrap(ServiceCollection serviceCollection)
    {
        VerifyOptions(serviceCollection);

        VerifyMailtrapServices(serviceCollection);

        VerifyHttpClientFactory(serviceCollection);
    }
}
