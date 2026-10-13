namespace Mailtrap.UnitTests.Marketing;


[TestFixture]
internal sealed class MarketingStatsResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.MarketingSegment,
            UrlSegmentsTestConstants.StatsSegment);


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new MarketingStatsResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new MarketingStatsResource(_commandFactoryMock, null!);

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

    #endregion



    #region GetStats

    [Test]
    public async Task GetStats_ShouldThrowArgumentNullException_WhenFilterIsNull()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.GetStats(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestCase("", "2026-01-31")]
    [TestCase("2026-01-01", "")]
    public async Task GetStats_ShouldThrowArgumentNullException_WhenDateIsEmpty(string startDate, string endDate)
    {
        // Arrange
        var client = CreateResource();
        var filter = new MarketingStatsFilter { StartDate = startDate, EndDate = endDate };

        // Act
        var act = () => client.GetStats(filter);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion



    private MarketingStatsResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
