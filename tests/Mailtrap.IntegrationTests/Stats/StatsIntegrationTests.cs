namespace Mailtrap.IntegrationTests.Stats;


[TestFixture]
internal sealed class StatsIntegrationTests
{
    private const string Feature = "Stats";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;

    public StatsIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.StatsSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
    }

    [Test]
    public async Task Get_Success()
    {
        // Arrange
        var filter = new StatsFilter { StartDate = "2026-01-01", EndDate = "2026-01-31" };

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
            .Account(_accountId)
            .Stats()
            .GetStats(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.DeliveryCount.Should().Be(150);
        result.DeliveryRate.Should().Be(0.95);
        result.BounceCount.Should().Be(8);
        result.BounceRate.Should().Be(0.05);
        result.OpenCount.Should().Be(120);
        result.OpenRate.Should().Be(0.8);
        result.ClickCount.Should().Be(60);
        result.ClickRate.Should().Be(0.5);
        result.SpamCount.Should().Be(2);
        result.SpamRate.Should().Be(0.013);
    }

    [Test]
    public async Task Get_Forbidden()
    {
        // Arrange
        var filter = new StatsFilter { StartDate = "2026-01-01", EndDate = "2026-01-31" };

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
            .Account(_accountId)
            .Stats()
            .GetStats(filter);

        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*'Access forbidden'*");

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task ByDomain_Success()
    {
        // Arrange
        var filter = new StatsFilter { StartDate = "2026-01-01", EndDate = "2026-01-31" };
        var requestUri = _resourceUri.Append("domains").AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: "start_date",
            queryParameterValue: filter.StartDate);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Stats()
            .ByDomain(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.ElementAt(0);
        first.Name.Should().Be("sending_domain_id");
        first.Value.Should().Be((long)1);
        first.Stats.DeliveryCount.Should().Be(100);

        var second = result.ElementAt(1);
        second.Name.Should().Be("sending_domain_id");
        second.Value.Should().Be((long)2);
        second.Stats.DeliveryCount.Should().Be(50);
    }

    [Test]
    public async Task ByCategory_Success()
    {
        // Arrange
        var filter = new StatsFilter { StartDate = "2026-01-01", EndDate = "2026-01-31" };
        var requestUri = _resourceUri.Append("categories").AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: "start_date",
            queryParameterValue: filter.StartDate);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Stats()
            .ByCategory(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Should().ContainSingle()
            .Which.Name.Should().Be("category");
        result.First().Value.Should().Be("Welcome email");
        result.First().Stats.DeliveryCount.Should().Be(100);
    }

    [Test]
    public async Task ByEmailServiceProvider_Success()
    {
        // Arrange
        var filter = new StatsFilter { StartDate = "2026-01-01", EndDate = "2026-01-31" };
        var requestUri = _resourceUri.Append("email_service_providers").AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: "start_date",
            queryParameterValue: filter.StartDate);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Stats()
            .ByEmailServiceProvider(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Should().ContainSingle()
            .Which.Name.Should().Be("email_service_provider");
        result.First().Value.Should().Be("Gmail");
        result.First().Stats.DeliveryCount.Should().Be(80);
    }

    [Test]
    public async Task ByDate_Success()
    {
        // Arrange
        var filter = new StatsFilter { StartDate = "2026-01-01", EndDate = "2026-01-31" };
        var requestUri = _resourceUri.Append("date").AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: "start_date",
            queryParameterValue: filter.StartDate);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Stats()
            .ByDate(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Should().ContainSingle()
            .Which.Name.Should().Be("date");
        result.First().Value.Should().Be("2026-01-01");
        result.First().Stats.DeliveryCount.Should().Be(5);
    }
}
