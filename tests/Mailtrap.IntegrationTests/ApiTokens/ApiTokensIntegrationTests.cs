namespace Mailtrap.IntegrationTests.ApiTokens;


[TestFixture]
internal sealed class ApiTokensIntegrationTests
{
    private const string Feature = "ApiTokens";
    private const string ResetSegment = "reset";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ApiTokensIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.ApiTokensSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }


    [Test]
    public async Task GetAll_Success()
    {
        // Arrange
        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<List<ApiToken>>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .ApiTokens()
            .GetAll()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var request = new CreateApiTokenRequest { Name = "My API Token" };
        request.Resources.Add(new ApiTokenAccessRequest(ResourceType.Account, 3229, AccessLevel.Admin));

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<CreateApiTokenResponse>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Post,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .ApiTokens()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var apiTokenId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(apiTokenId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ApiToken>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .ApiToken(apiTokenId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var apiTokenId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(apiTokenId).AbsoluteUri;

        using var responseContent = new StringContent(string.Empty);

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Delete,
            requestUri,
            responseContent,
            HttpStatusCode.NoContent,
            _clientConfig);

        // Act
        await clientScope.Client
            .Account(_accountId)
            .ApiToken(apiTokenId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Reset_Success()
    {
        // Arrange
        var apiTokenId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(apiTokenId).Append(ResetSegment).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ApiTokenResetResponse>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Post,
            requestUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .ApiToken(apiTokenId)
            .Reset()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }
}
