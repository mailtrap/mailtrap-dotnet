namespace Mailtrap.UnitTests.Inbound;


[TestFixture]
internal sealed class InboundInboxContentResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.InboundSegment,
            UrlSegmentsTestConstants.InboxesSegment)
        .Append(TestContext.CurrentContext.Random.NextLong());


    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        var act = () => new InboundInboxContentResource(null!, _resourceUri);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => new InboundInboxContentResource(_commandFactoryMock, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ResourceUri_ShouldBeInitializedProperly()
    {
        var client = CreateResource();

        client.ResourceUri.Should().Be(_resourceUri);
    }

    [Test]
    public void ForwardRules_ShouldReturnResourceWithForwardRulesUri()
    {
        var result = CreateResource().ForwardRules();

        result.ResourceUri.Should().Be(_resourceUri.Append(UrlSegmentsTestConstants.ForwardRulesSegment));
    }

    [Test]
    public void ForwardRule_ShouldReturnResourceWithForwardRuleUri()
    {
        var ruleId = TestContext.CurrentContext.Random.NextLong();

        var result = CreateResource().ForwardRule(ruleId);

        result.ResourceUri.Should().Be(_resourceUri.Append(UrlSegmentsTestConstants.ForwardRulesSegment).Append(ruleId));
    }

    [Test]
    public void ForwardRule_ShouldThrowArgumentOutOfRangeException_WhenIdIsNotPositive([Values(0, -1)] long ruleId)
    {
        var act = () => CreateResource().ForwardRule(ruleId);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }


    private InboundInboxContentResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
