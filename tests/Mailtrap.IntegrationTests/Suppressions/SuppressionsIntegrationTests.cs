namespace Mailtrap.IntegrationTests.Suppressions;


[TestFixture]
internal sealed class SuppressionsIntegrationTests
{
    private const string Feature = "Suppressions";
    private const string EmailQueryParameter = "email";
    private const string StartTimeQueryParameter = "start_time";
    private const string EndTimeQueryParameter = "end_time";
    private const string LastIdQueryParameter = "last_id";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SuppressionsIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.SuppressionsSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }

    [Test]
    public async Task Fetch_WithoutFilter_Success()
    {
        // Arrange
        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<List<Suppression>>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: "");

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Suppressions()
            .Fetch()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Fetch_WithFilter_Success()
    {
        // Arrange
        var filter = new SuppressionFilter { Email = "recipient1@example.com" };

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<List<Suppression>>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            queryParameterName: EmailQueryParameter,
            queryParameterValue: filter.Email);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Suppressions()
            .Fetch(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }


    [Test]
    public async Task Fetch_WithAllFilters_Success()
    {
        // Arrange
        var filter = new SuppressionFilter
        {
            Email = "recipient1@example.com",
            StartTime = new DateTimeOffset(2025, 9, 1, 0, 0, 0, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2025, 9, 30, 0, 0, 0, TimeSpan.Zero),
            LastId = "2fe148b8-b019-431f-ab3f-107663fdf868"
        };

        using var responseContent = await Feature.LoadFileToStringContent(nameof(Fetch_WithFilter_Success));
        var expectedResponse = await responseContent.DeserializeStringContentAsync<List<Suppression>>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, _resourceUri.AbsoluteUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithExactQueryString(new Dictionary<string, string>
            {
                [EmailQueryParameter] = filter.Email,
                [StartTimeQueryParameter] = filter.StartTime.Value.ToString("O"),
                [EndTimeQueryParameter] = filter.EndTime.Value.ToString("O"),
                [LastIdQueryParameter] = filter.LastId
            })
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Suppressions()
            .Fetch(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }


    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var request = new CreateSuppressionRequest
        {
            Email = "recipient@example.com",
            DomainId = 42,
            SendingStream = SendingStream.Transactional,
            Type = SuppressionType.ManualImport
        };

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<SuppressionResponseDto>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Post,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            request);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Suppressions()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse.Suppression);
    }


    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var suppressionId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var requestUri = _resourceUri.Append(suppressionId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<Suppression>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Delete,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Suppression(suppressionId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }
}
