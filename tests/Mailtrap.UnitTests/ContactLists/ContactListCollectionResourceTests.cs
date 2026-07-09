namespace Mailtrap.UnitTests.ContactLists;


[TestFixture]
internal sealed class ContactListCollectionResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.AccountsSegment)
        .Append(TestContext.CurrentContext.Random.NextLong())
        .Append(UrlSegmentsTestConstants.ContactsSegment)
        .Append(UrlSegmentsTestConstants.ListsSegment);


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new ContactListCollectionResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new ContactListCollectionResource(_commandFactoryMock, null!);

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


    #region GetAll

    [Test]
    public async Task GetAll_ShouldNotAppendQueryParameters_WhenFilterIsNull()
    {
        // Arrange
        var commandFactoryMock = new Mock<IRestResourceCommandFactory>();
        var commandMock = new Mock<IRestResourceCommand<IList<ContactList>>>();
        commandMock
            .Setup(c => c.Execute(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        commandFactoryMock
            .Setup(f => f.CreateGet<IList<ContactList>>(It.IsAny<Uri>()))
            .Returns(commandMock.Object);

        var resource = new ContactListCollectionResource(commandFactoryMock.Object, _resourceUri);

        // Act
        await resource.GetAll();

        // Assert
        commandFactoryMock.Verify(f => f.CreateGet<IList<ContactList>>(_resourceUri), Times.Once);
    }

    [Test]
    public async Task GetAll_ShouldAppendSearchQueryParameter_WhenFilterSearchIsProvided()
    {
        // Arrange
        var commandFactoryMock = new Mock<IRestResourceCommandFactory>();
        var commandMock = new Mock<IRestResourceCommand<IList<ContactList>>>();
        commandMock
            .Setup(c => c.Execute(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        Uri? capturedUri = null;
        commandFactoryMock
            .Setup(f => f.CreateGet<IList<ContactList>>(It.IsAny<Uri>()))
            .Callback<Uri, string[]>((uri, _) => capturedUri = uri)
            .Returns(commandMock.Object);

        var resource = new ContactListCollectionResource(commandFactoryMock.Object, _resourceUri);
        var filter = new ContactListListFilter { Search = "news" };

        // Act
        await resource.GetAll(filter);

        // Assert
        capturedUri.Should().NotBeNull();
        capturedUri!.Query.Should().Be("?search=news");
    }

    #endregion

    private ContactListCollectionResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
