namespace Mailtrap.UnitTests.Organizations;


[TestFixture]
internal sealed class OrganizationResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.OrganizationsSegment)
        .Append(TestContext.CurrentContext.Random.NextLong());


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new OrganizationResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new OrganizationResource(_commandFactoryMock, null!);

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


    #region Sub Accounts

    [Test]
    public void SubAccounts_ShouldReturnSubAccountCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.SubAccounts();

        // Assert
        ResourceValidator.Validate<IOrganizationSubAccountCollectionResource, OrganizationSubAccountCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.SubAccountsSegment));
    }

    [Test]
    public void SubAccount_ShouldReturnSubAccountResource()
    {
        // Arrange
        var client = CreateResource();
        var subAccountId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.SubAccount(subAccountId);

        // Assert
        ResourceValidator.Validate<IOrganizationSubAccountResource, OrganizationSubAccountResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.SubAccountsSegment).Append(subAccountId));
    }

    [Test]
    public void SubAccount_ShouldThrowOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long subAccountId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.SubAccount(subAccountId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion



    private OrganizationResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
