namespace Mailtrap.UnitTests.ContactExports.Model;


[TestFixture]
internal sealed class ContactExportListIdFilterTests
{
    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenProvidedCollectionIsNull()
    {
        var act = () => new ContactExportListIdFilter(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenProvidedCollectionIsEmpty()
    {
        var act = () => new ContactExportListIdFilter(Array.Empty<int>());

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_Should_InitializeFieldsCorrectlyFromArray()
    {
        var values = new[] { 1, 2, 3 };

        // Arrange & Act
        var filter = new ContactExportListIdFilter(values);


        // Assert
        filter.Value.Should().BeEquivalentTo(values);
    }

    [Test]
    public void Constructor_Should_InitializeFieldsCorrectlyFromEnumerable()
    {
        IEnumerable<int> values = new List<int> { 1, 2, 3 };

        // Arrange & Act
        var filter = new ContactExportListIdFilter(values);

        // Assert
        filter.Value.Should().BeEquivalentTo(values);
    }

    [Test]
    public void CopyConstructor_Should_InitializeFieldsCorrectly()
    {
        IEnumerable<int> values = new List<int> { 1, 2, 3 };

        // Arrange & Act
        var filter = new ContactExportListIdFilter(values);
        var filterCopy = filter with
        {
            Operator = ContactExportFilterOperator.None
        };

        // Assert
        filter.Value.Should().BeEquivalentTo(values);
        filterCopy.Value.Should().BeEquivalentTo(values);
        filterCopy.Operator.Should().Be(ContactExportFilterOperator.None);
    }

    [Test]
    public void Validator_Should_Fail_WhenValueIsEmpty()
    {
        // Arrange
        var invalidFilter = new ContactExportListIdFilter(1);
        invalidFilter.Value.Clear();

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(invalidFilter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(nameof(ContactExportListIdFilter.Value));
        result.ShouldNotHaveValidationErrorFor(nameof(ContactExportListIdFilter.Operator));
    }

    [Test]
    public void Validator_Should_Fail_WhenOperatorIsNullOrEmpty()
    {
        // Arrange
        var filter = new ContactExportListIdFilter(1, 2)
        {
            Operator = null!
        };

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldNotHaveValidationErrorFor(nameof(ContactExportListIdFilter.Value));
        result.ShouldHaveValidationErrorFor(nameof(ContactExportListIdFilter.Operator));
    }

    [Test]
    public void Validator_Should_Fail_WhenFilterIsInvalid()
    {
        // Arrange
        var filter = new ContactExportListIdFilter(1, 2)
        {
            Operator = ContactExportFilterOperator.Unknown
        };
        filter.Value.Clear();

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(nameof(ContactExportListIdFilter.Value));
        result.ShouldHaveValidationErrorFor(nameof(ContactExportListIdFilter.Operator));
    }

    [Test]
    public void Validator_Should_Pass_ForValidFilter()
    {
        // Arrange
        var filter = new ContactExportListIdFilter(1);

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }


}
