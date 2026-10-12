namespace Mailtrap.IntegrationTests.Inbound;


[TestFixture]
internal sealed class InboundThreadsIntegrationTests
{
    private const string Feature = "Inbound/Threads";
    private const string ThreadId = "thr_1";


    private static Uri ThreadsUri(long inboxId)
        => EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.InboundSegment,
                UrlSegmentsTestConstants.InboxesSegment)
            .Append(inboxId)
            .Append(UrlSegmentsTestConstants.ThreadsSegment);


    [Test]
    public async Task List_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ThreadsUri(inboxId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).Threads().List().ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Data.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.LastId.Should().Be("thr_2");
        result.Data[0].Subject.Should().Be("Support request");
        result.Data[0].MessageCount.Should().Be(3);
    }

    [Test]
    public async Task List_WithSearch_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ThreadsUri(inboxId)
            .AppendQueryParameters([new KeyValuePair<string, string>("search", "acme")])
            .AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).Threads().List(null, "acme").ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Data.Should().ContainSingle().Which.Subject.Should().Be("Acme order");
        result.TotalCount.Should().Be(3);
        result.LastId.Should().BeNull();
    }

    [Test]
    public async Task List_WithSearchAndCursor_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        const string Cursor = "WzE3NzgyNDE5MDAwMDAsIjE3MDAwMDAwMDAwMDAxMjMiXQ==";
        var requestUri = ThreadsUri(inboxId)
            .AppendQueryParameters(
            [
                new KeyValuePair<string, string>("last_id", Cursor),
                new KeyValuePair<string, string>("search", "acme corp")
            ])
            .AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithExactQueryString([
                new KeyValuePair<string, string>("last_id", Cursor),
                new KeyValuePair<string, string>("search", "acme corp")
            ])
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).Threads().List(Cursor, "acme corp").ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Data.Should().ContainSingle();
    }

    [Test]
    public async Task GetDetails_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ThreadsUri(inboxId).Append(ThreadId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).Thread(ThreadId).GetDetails().ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Messages.Should().HaveCount(3);

        var placeholder = result.Messages[0];
        placeholder.VisibilityStatus.Should().Be(ThreadMessageVisibilityStatus.Placeholder);
        placeholder.Delivery.Should().BeNull();
        placeholder.Forwards.Should().BeEmpty();

        var received = result.Messages[1];
        received.Direction.Should().Be(ThreadMessageDirection.Inbound);
        received.VisibilityStatus.Should().Be(ThreadMessageVisibilityStatus.Available);
        received.Delivery.Should().BeNull();
        received.Forwards.Should().HaveCount(2);
        received.Forwards[0].RuleId.Should().Be(7);
        received.Forwards[0].RuleName.Should().Be("Copy to support team");
        received.Forwards[0].Destination.Should().Be("team@example.com");
        received.Forwards[0].Status.Should().Be(ForwardOutcomeStatus.Forwarded);
        received.Forwards[0].Reason.Should().BeNull();
        received.Forwards[0].MessageId.Should().Be("f47ac10b-58cc-4372-a567-0e02b2c3d479");
        received.Forwards[1].RuleName.Should().BeNull();
        received.Forwards[1].Status.Should().Be(ForwardOutcomeStatus.Rejected);
        received.Forwards[1].Reason.Should().Be("loop_prevention");
        received.Forwards[1].MessageId.Should().BeNull();

        var sent = result.Messages[2];
        sent.Direction.Should().Be(ThreadMessageDirection.Outbound);
        sent.Forwards.Should().BeEmpty();
        sent.Delivery.Should().NotBeNull();
        sent.Delivery!.To.Should().Be("customer@example.com");
        sent.Delivery.Status.Should().Be(EmailLogStatus.Delivered);
        sent.Delivery.DeliveredAt.Should().Be(new DateTimeOffset(2026, 7, 30, 13, 0, 5, TimeSpan.Zero));
        sent.Delivery.BouncedAt.Should().BeNull();
    }

    [Test]
    public async Task Delete_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ThreadsUri(inboxId).Append(ThreadId).AbsoluteUri;

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Delete, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .Respond(HttpStatusCode.NoContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        await client.Inbound().Inbox(inboxId).Thread(ThreadId).Delete().ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
