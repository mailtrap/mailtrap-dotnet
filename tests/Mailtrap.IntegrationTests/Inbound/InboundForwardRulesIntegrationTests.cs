namespace Mailtrap.IntegrationTests.Inbound;


[TestFixture]
internal sealed class InboundForwardRulesIntegrationTests
{
    private const string Feature = "Inbound/ForwardRules";


    private static Uri ForwardRulesUri(long inboxId)
        => EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.InboundSegment,
                UrlSegmentsTestConstants.InboxesSegment)
            .Append(inboxId)
            .Append(UrlSegmentsTestConstants.ForwardRulesSegment);


    [Test]
    public async Task GetAll_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ForwardRulesUri(inboxId).AbsoluteUri;

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

        var result = await client.Inbound().Inbox(inboxId).ForwardRules().GetAll().ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Should().NotBeNull().And.HaveCount(2);
        result[0].Id.Should().Be(7);
        result[0].Name.Should().Be("Copy billing mail to finance");
        result[0].CreatedAt.Should().Be(new DateTimeOffset(2026, 5, 8, 10, 30, 0, TimeSpan.Zero));
        result[0].Conditions.Should().ContainSingle();
        result[0].Conditions[0].MatchType.Should().Be(ForwardRuleMatchType.Sender);
        result[0].Conditions[0].Operator.Should().Be(ForwardRuleOperator.EndsWith);
        result[0].Conditions[0].Value.Should().Be("@billing.example.com");
        result[0].Conditions[0].HeaderKey.Should().BeNull();
        result[0].Destinations.Should().ContainSingle().Which.Email.Should().Be("finance@example.com");
        result[1].Conditions.Should().BeEmpty();
        result[1].Destinations.Should().ContainSingle().Which.Email.Should().Be("archive@example.com");
    }

    [Test]
    public async Task GetDetails_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var ruleId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ForwardRulesUri(inboxId).Append(ruleId).AbsoluteUri;

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

        var result = await client.Inbound().Inbox(inboxId).ForwardRule(ruleId).GetDetails().ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Id.Should().Be(9);
        result.Name.Should().Be("Escalate urgent tickets");
        result.UpdatedAt.Should().Be(new DateTimeOffset(2026, 5, 8, 12, 15, 0, TimeSpan.Zero));
        result.Conditions.Should().HaveCount(2);
        result.Conditions[0].MatchType.Should().Be(ForwardRuleMatchType.Header);
        result.Conditions[0].Operator.Should().Be(ForwardRuleOperator.Equal);
        result.Conditions[0].HeaderKey.Should().Be("X-Priority-Level");
        result.Conditions[1].MatchType.Should().Be(ForwardRuleMatchType.Recipient);
        result.Conditions[1].Operator.Should().Be(ForwardRuleOperator.StartsWith);
        result.Destinations.Select(d => d.Email).Should().Equal("oncall@example.com", "lead@example.com");
    }

    [Test]
    public async Task Create_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ForwardRulesUri(inboxId).AbsoluteUri;
        var request = new CreateInboundForwardRuleRequest
        {
            Name = "Copy billing mail to finance",
            Conditions =
            [
                new InboundForwardRuleCondition
                {
                    MatchType = ForwardRuleMatchType.Sender,
                    Operator = ForwardRuleOperator.EndsWith,
                    Value = "@billing.example.com"
                }
            ],
            Destinations = [new InboundForwardRuleDestination { Email = "finance@example.com" }]
        };
        const string ExpectedRequestBody =
            "{\"name\":\"Copy billing mail to finance\"," +
            "\"conditions\":[{\"match_type\":\"sender\",\"operator\":\"ends_with\",\"value\":\"@billing.example.com\"}]," +
            "\"destinations\":[{\"email\":\"finance@example.com\"}]}";

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Post, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithContent(ExpectedRequestBody)
            .Respond(HttpStatusCode.Created, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).ForwardRules().Create(request).ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Id.Should().Be(7);
        result.Conditions.Should().ContainSingle().Which.Operator.Should().Be(ForwardRuleOperator.EndsWith);
        result.Destinations.Should().ContainSingle().Which.Email.Should().Be("finance@example.com");
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenNameIsMissing()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var request = new CreateInboundForwardRuleRequest
        {
            Destinations = [new InboundForwardRuleDestination { Email = "finance@example.com" }]
        };

        using var mockHttp = new MockHttpMessageHandler();

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var act = () => client.Inbound().Inbox(inboxId).ForwardRules().Create(request);

        await act.Should().ThrowAsync<RequestValidationException>().ConfigureAwait(false);
    }

    [Test]
    public async Task Update_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var ruleId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ForwardRulesUri(inboxId).Append(ruleId).AbsoluteUri;
        var request = new UpdateInboundForwardRuleRequest
        {
            Destinations =
            [
                new InboundForwardRuleDestination { Email = "finance@example.com" },
                new InboundForwardRuleDestination { Email = "accounting@example.com" }
            ]
        };
        const string ExpectedRequestBody =
            "{\"destinations\":[{\"email\":\"finance@example.com\"},{\"email\":\"accounting@example.com\"}]}";

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Patch, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithContent(ExpectedRequestBody)
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).ForwardRule(ruleId).Update(request).ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Destinations.Should().HaveCount(2);
        result.Conditions.Should().ContainSingle();
    }

    [Test]
    public async Task Update_ClearConditions_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var ruleId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ForwardRulesUri(inboxId).Append(ruleId).AbsoluteUri;
        var request = new UpdateInboundForwardRuleRequest
        {
            Name = "Renamed rule",
            Conditions = []
        };
        const string ExpectedRequestBody = "{\"name\":\"Renamed rule\",\"conditions\":[]}";

        using var responseContent = await Feature.LoadFileToStringContent();
        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Patch, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithContent(ExpectedRequestBody)
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        var result = await client.Inbound().Inbox(inboxId).ForwardRule(ruleId).Update(request).ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
        result.Name.Should().Be("Renamed rule");
        result.Conditions.Should().BeEmpty();
    }

    [Test]
    public async Task Delete_Success()
    {
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var ruleId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = ForwardRulesUri(inboxId).Append(ruleId).AbsoluteUri;

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

        await client.Inbound().Inbox(inboxId).ForwardRule(ruleId).Delete().ConfigureAwait(false);

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
