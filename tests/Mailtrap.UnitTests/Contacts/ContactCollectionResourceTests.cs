namespace Mailtrap.UnitTests.Contacts;


[TestFixture]
internal sealed class ContactCollectionResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.AccountsSegment)
        .Append(TestContext.CurrentContext.Random.NextLong())
        .Append(UrlSegmentsTestConstants.ContactsSegment);


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new ContactCollectionResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new ContactCollectionResource(_commandFactoryMock, null!);

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

    #region Exports

    [Test]
    public void Exports_ShouldReturnContactExportCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Exports();

        // Assert
        ResourceValidator.Validate<IContactExportCollectionResource, ContactExportCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ExportsSegment));
    }

    [Test]
    public void Export_ShouldReturnContactExportResource()
    {
        // Arrange
        var client = CreateResource();
        var exportId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Export(exportId);

        // Assert
        ResourceValidator.Validate<IContactExportResource, ContactExportResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ExportsSegment).Append(exportId));
    }

    [Test]
    public void Export_ShouldThrowArgumentOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long exportId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Export(exportId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Imports

    [Test]
    public void Imports_ShouldReturnContactImportCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Imports();

        // Assert
        ResourceValidator.Validate<IContactImportCollectionResource, ContactImportCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ImportsSegment));
    }

    [Test]
    public void Import_ShouldReturnContactImportResource()
    {
        // Arrange
        var client = CreateResource();
        var importId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Import(importId);

        // Assert
        ResourceValidator.Validate<IContactImportResource, ContactImportResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ImportsSegment).Append(importId));
    }

    [Test]
    public void Import_ShouldThrowArgumentOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long importId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Import(importId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Lists

    [Test]
    public void Lists_ShouldReturnContactListCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Lists();

        // Assert
        ResourceValidator.Validate<IContactListCollectionResource, ContactListCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ListsSegment));
    }

    [Test]
    public void List_ShouldReturnContactListResource()
    {
        // Arrange
        var client = CreateResource();
        var listId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.List(listId);

        // Assert
        ResourceValidator.Validate<IContactListResource, ContactListResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ListsSegment).Append(listId));
    }

    [Test]
    public void List_ShouldThrowArgumentOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long listId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.List(listId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Fields

    [Test]
    public void Fields_ShouldReturnContactFieldCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Fields();

        // Assert
        ResourceValidator.Validate<IContactFieldCollectionResource, ContactFieldCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.FieldsSegment));
    }

    [Test]
    public void Field_ShouldReturnContactFieldResource()
    {
        // Arrange
        var client = CreateResource();
        var fieldId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Field(fieldId);

        // Assert
        ResourceValidator.Validate<IContactFieldResource, ContactFieldResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.FieldsSegment).Append(fieldId));
    }

    [Test]
    public void Field_ShouldThrowArgumentOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long fieldId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Field(fieldId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Events

    [Test]
    public void Events_ShouldReturnContactEventCollectionResource()
    {
        // Arrange
        var client = CreateResource();
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();

        // Act
        var result = client.Events(contactId);

        // Assert
        ResourceValidator.Validate<IContactEventCollectionResource, ContactEventCollectionResource>(
            result, client.ResourceUri.Append(contactId).Append(UrlSegmentsTestConstants.EventsSegment));
    }

    #endregion

    private ContactCollectionResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
