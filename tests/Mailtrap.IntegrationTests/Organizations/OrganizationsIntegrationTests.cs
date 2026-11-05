namespace Mailtrap.IntegrationTests.Organizations;


[TestFixture]
internal sealed class OrganizationsIntegrationTests
{
    private const string Feature = "Organizations";

    private readonly long _organizationId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public OrganizationsIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _organizationId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.OrganizationsSegment)
            .Append(_organizationId)
            .Append(UrlSegmentsTestConstants.SubAccountsSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }


    [Test]
    public async Task GetAll_Success()
    {
        // Arrange
        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<List<SubAccount>>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.OrganizationClient
            .Organization(_organizationId)
            .SubAccounts()
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
        var request = new CreateSubAccountRequest { Account = new SubAccountAttributes { Name = "New Team Account" } };

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<SubAccount>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Post,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.OrganizationClient
            .Organization(_organizationId)
            .SubAccounts()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var subAccountId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(subAccountId).AbsoluteUri;

        using var responseContent = new StringContent(string.Empty);

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Delete,
            requestUri,
            responseContent,
            HttpStatusCode.NoContent,
            _clientConfig);

        // Act
        await clientScope.OrganizationClient
            .Organization(_organizationId)
            .SubAccount(subAccountId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Delete_NotFound()
    {
        // Arrange
        var subAccountId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(subAccountId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Delete,
            requestUri,
            responseContent,
            HttpStatusCode.NotFound,
            _clientConfig);

        // Act
        var act = () => clientScope.OrganizationClient
            .Organization(_organizationId)
            .SubAccount(subAccountId)
            .Delete();

        // Assert
        var assertion = await act.Should().ThrowAsync<HttpRequestFailedException>();
        assertion.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
