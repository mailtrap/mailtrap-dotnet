namespace Mailtrap.UnitTests.EmailCampaigns;


[TestFixture]
internal sealed class EmailCampaignCollectionResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();

    // Email campaigns are token-scoped: the resource URI is the bare "/api/email_campaigns",
    // NOT "/api/accounts/{account_id}/email_campaigns".
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.EmailCampaignsSegment);


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new EmailCampaignCollectionResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new EmailCampaignCollectionResource(_commandFactoryMock, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ResourceUri_ShouldBeInitializedProperly()
    {
        // Arrange
        var client = CreateResource();

        // Assert
        client.ResourceUri.Should().Be(_resourceUri);
    }

    [Test]
    public void ResourceUri_ShouldNotBeAccountScoped()
    {
        // Arrange
        var client = CreateResource();

        // Assert
        client.ResourceUri.AbsoluteUri.Should().NotContain("/api/accounts/");
        client.ResourceUri.AbsoluteUri.Should().EndWith("/api/email_campaigns");
    }

    #endregion


    private EmailCampaignCollectionResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
