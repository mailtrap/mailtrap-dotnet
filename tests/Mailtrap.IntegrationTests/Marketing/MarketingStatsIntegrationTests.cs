namespace Mailtrap.IntegrationTests.Marketing;


[TestFixture]
internal sealed class MarketingStatsIntegrationTests
{
    private const string Feature = "Marketing";

    private const string StartDate = "2026-01-01";
    private const string EndDate = "2026-01-31";

    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;


    public MarketingStatsIntegrationTests()
    {
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.MarketingSegment,
                UrlSegmentsTestConstants.StatsSegment);

        var token = TestContext.CurrentContext.Random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
    }


    [Test]
    public async Task GetStats_Success()
    {
        // Arrange
        var filter = new MarketingStatsFilter { StartDate = StartDate, EndDate = EndDate };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: "start_date",
            queryParameterValue: filter.StartDate);


        // Act
        var result = await clientScope.Client
            .MarketingStats()
            .GetStats(filter)
            .ConfigureAwait(false);


        // Assert - the stats must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.DeliveryCount.Should().Be(1450);
        result.DeliveryRate.Should().Be(0.9667);
        result.BounceCount.Should().Be(50);
        result.BounceRate.Should().Be(0.0333);
        result.OpenCount.Should().Be(820);
        result.OpenRate.Should().Be(0.5655);
        result.ClickCount.Should().Be(310);
        result.ClickRate.Should().Be(0.2138);
        result.SpamCount.Should().Be(5);
        result.SpamRate.Should().Be(0.0034);
        result.UnsubscriptionCount.Should().Be(12);
        result.UnsubscriptionRate.Should().Be(0.0083);
        result.SentCount.Should().Be(1500);
    }

    [Test]
    public async Task GetStats_WithFilters_SerializesArrayParameters()
    {
        // Arrange
        var filter = new MarketingStatsFilter { StartDate = StartDate, EndDate = EndDate };
        filter.SendingDomainIds.Add(3938);
        filter.SendingDomainIds.Add(3939);
        filter.Sources.Add("campaigns");
        filter.EmailServiceProviders.Add("Google");

        using var responseContent = await Feature.LoadFileToStringContent(fileName: "GetStats_Success");

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, _resourceUri.AbsoluteUri)
            .WithExactQueryString([
                new KeyValuePair<string, string>("start_date", StartDate),
                new KeyValuePair<string, string>("end_date", EndDate),
                new KeyValuePair<string, string>("sending_domain_ids[]", "3938"),
                new KeyValuePair<string, string>("sending_domain_ids[]", "3939"),
                new KeyValuePair<string, string>("sources[]", "campaigns"),
                new KeyValuePair<string, string>("email_service_providers[]", "Google")
            ])
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .MarketingStats()
            .GetStats(filter)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.SentCount.Should().Be(1500);
    }

    [Test]
    public async Task GetStats_Forbidden()
    {
        // Arrange
        var filter = new MarketingStatsFilter { StartDate = StartDate, EndDate = EndDate };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.Forbidden,
            _clientConfig,
            queryParameterName: "start_date",
            queryParameterValue: filter.StartDate);


        var act = () => clientScope.Client
            .MarketingStats()
            .GetStats(filter);


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*'Access forbidden'*");

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
