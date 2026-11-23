namespace Mailtrap.UnitTests.SubscriptionTopics;


[TestFixture]
internal sealed class SubscriptionTopicCollectionResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();

    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.SubscriptionTopicsSegment);


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new SubscriptionTopicCollectionResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new SubscriptionTopicCollectionResource(_commandFactoryMock, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ResourceUri_ShouldBeInitializedProperly()
    {
        // Arrange
        var resource = CreateResource();

        // Assert
        resource.ResourceUri.Should().Be(_resourceUri);
    }

    #endregion


    private SubscriptionTopicCollectionResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
