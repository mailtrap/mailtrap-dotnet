namespace Mailtrap.UnitTests.ContactExports;


[TestFixture]
internal sealed class ContactExportResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.AccountsSegment)
        .Append(TestContext.CurrentContext.Random.NextLong())
        .Append(UrlSegmentsTestConstants.ContactsSegment)
        .Append(UrlSegmentsTestConstants.ExportsSegment)
        .Append(TestContext.CurrentContext.Random.NextLong());

    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new ContactExportResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new ContactExportResource(_commandFactoryMock, null!);

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

    private ContactExportResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
