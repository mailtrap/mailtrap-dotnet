namespace Mailtrap.UnitTests.Inbound;


[TestFixture]
internal sealed class InboundForwardRuleResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.InboundSegment,
            UrlSegmentsTestConstants.InboxesSegment)
        .Append(TestContext.CurrentContext.Random.NextLong())
        .Append(UrlSegmentsTestConstants.ForwardRulesSegment)
        .Append(TestContext.CurrentContext.Random.NextLong());


    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        var act = () => new InboundForwardRuleResource(null!, _resourceUri);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => new InboundForwardRuleResource(_commandFactoryMock, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ResourceUri_ShouldBeInitializedProperly()
    {
        var client = CreateResource();

        client.ResourceUri.Should().Be(_resourceUri);
    }


    private InboundForwardRuleResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
