namespace Mailtrap.IntegrationTests.Suppressions;


[TestFixture]
internal sealed class SuppressionsIntegrationTests
{
    private const string Feature = "Suppressions";
    private const string EmailQueryParameter = "email";

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
        var client = mockHttp.ConfigureWithQueryAndCreateClient(
                        HttpMethod.Get,
                        _resourceUri.AbsoluteUri,
                        responseContent,
                        HttpStatusCode.OK,
                        _clientConfig,
                        queryParameterName: "");

        // Act
        var result = await client
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
        var client = mockHttp.ConfigureWithQueryAndCreateClient(
                        HttpMethod.Get,
                        _resourceUri.AbsoluteUri,
                        responseContent,
                        HttpStatusCode.OK,
                        _clientConfig,
                        queryParameterName: EmailQueryParameter,
                        queryParameterValue: filter.Email);

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
    public async Task Delete_Success()
    {
        // Arrange
        var suppressionId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var requestUri = _resourceUri.Append(suppressionId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<Suppression>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        var client = mockHttp.ConfigureAndCreateClient(
                        HttpMethod.Delete,
                        requestUri,
                        responseContent,
                        HttpStatusCode.OK,
                        _clientConfig);

        // Act
        var result = await client
            .Account(_accountId)
            .Suppression(suppressionId)
            .Delete();

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }
}
