namespace Mailtrap.UnitTests.ContactExports.Requests;


[TestFixture]
internal sealed class CreateContactExportRequestTests
{
    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenProvidedCollectionIsNull()
    {
        var act = () => new CreateContactExportRequest(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenProvidedCollectionIsEmpty()
    {
        var act = () => new CreateContactExportRequest(Array.Empty<ContactExportFilterBase>());

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_Should_InitializeFieldsCorrectly()
    {
        // Arrange
        var filters = new List<ContactExportFilterBase> { RandomContactExportFilter() };

        // Act
        var request = new CreateContactExportRequest(filters);

        // Assert
        request.Filters.Should().BeEquivalentTo(filters);
    }


    [Test]
    public void Validate_Should_Fail_WhenProvidedCollectionSizeIsInvalid([Values(0, 50001)] int size)
    {
        // Arrange
        var filters = new List<ContactExportFilterBase>(size);
        for (var i = 0; i < size; i++)
        {
            filters.Add(RandomContactExportFilter());
        }
        var request = size == 0 ? new CreateContactExportRequest() : new CreateContactExportRequest(filters);

        // Act
        var result = CreateContactExportRequestValidator.Instance.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(f => f.Filters);
    }

    [Test]
    public void Validate_Should_Fail_WhenProvidedCollectionContainsNull()
    {
        // Arrange
        var filters = new List<ContactExportFilterBase>()
        {
            RandomContactExportFilter(),
            null!,
            RandomContactExportFilter()
        };

        var request = new CreateContactExportRequest(filters);

        // Act
        var result = CreateContactExportRequestValidator.Instance.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(f => f.Filters);
    }

    [Test]
    public void Validate_Should_Fail_WhenProvidedCollectionExceedsMaximumSize([Values(0, 50001)] int size)
    {
        // Arrange
        var filters = Enumerable.Range(0, size).Select(_ => RandomContactExportFilter());

        var request = size == 0 ? new CreateContactExportRequest() : new CreateContactExportRequest(filters);

        // Act
        var result = CreateContactExportRequestValidator.Instance.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(f => f.Filters);
    }

    [Test]
    public void Validate_Should_Pass_WhenProvidedCollectionIsValid([Values(1, 200, 50000)] int size)
    {
        // Arrange
        var filters = Enumerable.Range(0, size).Select(_ => RandomContactExportFilter());

        var request = new CreateContactExportRequest(filters);

        // Act
        var result = CreateContactExportRequestValidator.Instance.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }


    private static ContactExportFilterBase RandomContactExportFilter()
    {
        if (TestContext.CurrentContext.Random.NextBool())
        {
            return new ContactExportListIdFilter(
                TestContext.CurrentContext.Random.Next(),
                TestContext.CurrentContext.Random.Next()
                );
        }
        else
        {
            var status = (TestContext.CurrentContext.Random.Next() % 2) switch
            {
                0 => ContactExportFilterSubscriptionStatus.Subscribed,
                1 => ContactExportFilterSubscriptionStatus.Unsubscribed,
                _ => throw new ArgumentOutOfRangeException()
            };
            return new ContactExportSubscriptionStatusFilter(status);
        }
    }
}
