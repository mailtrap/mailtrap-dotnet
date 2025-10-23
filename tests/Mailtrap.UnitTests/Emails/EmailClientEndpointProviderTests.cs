namespace Mailtrap.UnitTests.Emails;


[TestFixture]
internal sealed class EmailClientEndpointProviderTests
{
    private EmailClientEndpointProvider _emailClientEndpointProvider;


    [SetUp]
    public void Setup()
    {
        _emailClientEndpointProvider = new EmailClientEndpointProvider();
    }


    [Test]
    public void GetRequestUri_ShouldReturnSendDefaultUrl_WhenIsNotBulkAndInboxIdIsNull()
    {
        // Arrange
        var isBulk = false;
        long? inboxId = null;
        var expectedUrl = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(false, isBulk, inboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnBulkDefaultUrl_WhenIsBulkAndInboxIdIsNull()
    {
        // Arrange
        var isBulk = true;
        long? inboxId = null;
        var expectedUrl = EndpointsTestConstants.BulkDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(false, isBulk, inboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnTestDefaultUrl_WhenInboxIdIsNotNull([Values] bool isBulk)
    {
        // Arrange
        long inboxId = 12345;
        var expectedUrl = EndpointsTestConstants.TestDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .Append(inboxId);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(false, isBulk, inboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }


    [Test]
    public void GetRequestUri_ShouldReturnBatchDefaultUrl_WhenIsNotBulkAndInboxIdIsNull()
    {
        // Arrange
        var isBulk = false;
        long? inboxId = null;
        var expectedUrl = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(true, isBulk, inboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnBatchBulkDefaultUrl_WhenIsBulkAndInboxIdIsNull()
    {
        // Arrange
        var isBulk = true;
        long? inboxId = null;
        var expectedUrl = EndpointsTestConstants.BulkDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(true, isBulk, inboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnBatchTestDefaultUrl_WhenInboxIdIsNotNull([Values] bool isBulk)
    {
        // Arrange
        long inboxId = 12345;
        var expectedUrl = EndpointsTestConstants.TestDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .Append(inboxId);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(true, isBulk, inboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }
}
