namespace Mailtrap.IntegrationTests.SubscriptionTopics;


[TestFixture]
internal sealed class SubscriptionTopicIntegrationTests
{
    private const string Feature = "SubscriptionTopics";

    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";


    private static Uri CollectionUri => EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.SubscriptionTopicsSegment);


    [Test]
    public async Task GetAll_Success()
    {
        // Arrange
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        // No filter sends no query string: an empty parameter name matches the exact, empty query.
        using var clientScope = mockHttp.ConfigureWithQueryAndCreateClient(
            HttpMethod.Get,
            CollectionUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            clientConfig,
            queryParameterName: string.Empty);


        // Act
        var result = await clientScope.Client
            .SubscriptionTopics()
            .GetAll()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);

        var everyDomain = result.Data[0];
        everyDomain.Id.Should().Be(12);
        everyDomain.Name.Should().Be("Newsletter");
        everyDomain.Description.Should().Be("Monthly product news");
        everyDomain.DomainId.Should().BeNull();
        everyDomain.DeletedAt.Should().BeNull();
        everyDomain.CreatedAt.Should().Be(new DateTimeOffset(2026, 5, 1, 9, 0, 0, TimeSpan.Zero));
        everyDomain.UpdatedAt.Should().Be(new DateTimeOffset(2026, 5, 2, 9, 0, 0, TimeSpan.Zero));

        var oneDomain = result.Data[1];
        oneDomain.Id.Should().Be(15);
        oneDomain.Description.Should().BeNull();
        oneDomain.DomainId.Should().Be(4321);

        result.Pagination.Should().NotBeNull();
        result.Pagination!.Token.Should().Be(1);
        result.Pagination.PrevToken.Should().BeNull();
        result.Pagination.NextToken.Should().Be(2);
    }

    [Test]
    public async Task GetAll_WithFilter_Success()
    {
        // Arrange
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());

        var filter = new SubscriptionTopicListFilter
        {
            Token = 2,
            PerPage = 1
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, CollectionUri.AbsoluteUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithExactQueryString(new Dictionary<string, string>
            {
                [TokenQueryParameter] = "2",
                [PerPageQueryParameter] = "1"
            })
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .SubscriptionTopics()
            .GetAll(filter)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Data.Should().ContainSingle().Which.Id.Should().Be(15);
        result.Pagination!.Token.Should().Be(2);
        result.Pagination.PrevToken.Should().Be(1);
        result.Pagination.NextToken.Should().BeNull();
    }

    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var subscriptionTopicId = 12;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            CollectionUri.Append(subscriptionTopicId).AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            clientConfig);


        // Act
        var result = await clientScope.Client
            .SubscriptionTopic(subscriptionTopicId)
            .GetDetails()
            .ConfigureAwait(false);


        // Assert - the topic must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Id.Should().Be(subscriptionTopicId);
        result.Name.Should().Be("Newsletter");
        result.DomainId.Should().BeNull();
        result.DeletedAt.Should().BeNull();
    }

    [Test]
    public async Task GetDetails_Deleted_Success()
    {
        // Arrange
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var subscriptionTopicId = 9;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            CollectionUri.Append(subscriptionTopicId).AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            clientConfig);


        // Act
        var result = await clientScope.Client
            .SubscriptionTopic(subscriptionTopicId)
            .GetDetails()
            .ConfigureAwait(false);


        // Assert - a deleted topic still resolves, with its deletion time.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Id.Should().Be(subscriptionTopicId);
        result.DomainId.Should().Be(4321);
        result.DeletedAt.Should().Be(new DateTimeOffset(2026, 4, 20, 10, 30, 0, TimeSpan.Zero));
    }

    [Test]
    public async Task GetAll_Forbidden_WhenSubscriptionTopicsAreNotEnabled()
    {
        // Arrange
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());

        // Production answers this 403 with an empty body.
        using var responseContent = new StringContent(string.Empty);

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            CollectionUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.Forbidden,
            clientConfig);


        // Act
        var act = () => clientScope.Client
            .SubscriptionTopics()
            .GetAll();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == HttpStatusCode.Forbidden);

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
