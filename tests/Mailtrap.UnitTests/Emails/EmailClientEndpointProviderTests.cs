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
    public void GetRequestUri_ShouldReturnSendDefaultUrl_WhenIsNotBulkAndSandboxIdIsNull()
    {
        // Arrange
        var isBulk = false;
        long? sandboxId = null;
        var expectedUrl = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(false, isBulk, sandboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnBulkDefaultUrl_WhenIsBulkAndSandboxIdIsNull()
    {
        // Arrange
        var isBulk = true;
        long? sandboxId = null;
        var expectedUrl = EndpointsTestConstants.BulkDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(false, isBulk, sandboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnSandboxDefaultUrl_WhenSandboxIdIsNotNull([Values] bool isBulk)
    {
        // Arrange
        long sandboxId = 12345;
        var expectedUrl = EndpointsTestConstants.SandboxDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .Append(sandboxId);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(false, isBulk, sandboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }


    [Test]
    public void GetRequestUri_ShouldReturnBatchDefaultUrl_WhenIsNotBulkAndSandboxIdIsNull()
    {
        // Arrange
        var isBulk = false;
        long? sandboxId = null;
        var expectedUrl = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(true, isBulk, sandboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnBatchBulkDefaultUrl_WhenIsBulkAndSandboxIdIsNull()
    {
        // Arrange
        var isBulk = true;
        long? sandboxId = null;
        var expectedUrl = EndpointsTestConstants.BulkDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(true, isBulk, sandboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Test]
    public void GetRequestUri_ShouldReturnBatchSandboxDefaultUrl_WhenSandboxIdIsNotNull([Values] bool isBulk)
    {
        // Arrange
        long sandboxId = 12345;
        var expectedUrl = EndpointsTestConstants.SandboxDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .Append(sandboxId);

        // Act
        var result = _emailClientEndpointProvider.GetRequestUri(true, isBulk, sandboxId);

        // Assert
        result.Should().Be(expectedUrl);
    }
}
