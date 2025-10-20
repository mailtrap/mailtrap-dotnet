namespace Mailtrap.UnitTests.ContactExports.Model;


[TestFixture]
internal sealed class ContactExportSubscriptionStatusFilterTests
{
    [Test]
    public void Constructor_Should_ThrowArgumentNullException_WhenProvidedCollectionIsNull()
    {
        // Arrange & Act
        var act = () => new ContactExportSubscriptionStatusFilter(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_Should_InitializeFieldsCorrectly()
    {
        // Arrange & Act
        var filter = new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Subscribed);

        // Assert
        filter.Value.Should().BeEquivalentTo(ContactExportFilterSubscriptionStatus.Subscribed);
        filter.Operator.Should().BeEquivalentTo(ContactExportFilterOperator.Equal);
    }

    [Test]
    public void CopyConstructor_Should_InitializeFieldsCorrectly()
    {
        // Arrange & Act
        var filter = new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Subscribed);
        var filterCopy = filter with
        {
            Operator = ContactExportFilterOperator.None
        };

        // Assert
        filter.Value.Should().BeEquivalentTo(ContactExportFilterSubscriptionStatus.Subscribed);
        filter.Operator.Should().Be(ContactExportFilterOperator.Equal);
        filterCopy.Value.Should().BeEquivalentTo(ContactExportFilterSubscriptionStatus.Subscribed);
        filterCopy.Operator.Should().Be(ContactExportFilterOperator.None);
    }

    [Test]
    public void Validator_Should_Fail_WhenValueIsNullOrEmpty()
    {
        // Arrange
        var filter = new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Unknown)
        {
            Value = null!
        };

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(nameof(ContactExportSubscriptionStatusFilter.Value));
        result.ShouldNotHaveValidationErrorFor(nameof(ContactExportSubscriptionStatusFilter.Operator));
    }

    [Test]
    public void Validator_Should_Fail_WhenOperatorIsNullOrEmpty()
    {
        // Arrange
        var filter = new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Subscribed)
        {
            Operator = null!
        };

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldNotHaveValidationErrorFor(nameof(ContactExportSubscriptionStatusFilter.Value));
        result.ShouldHaveValidationErrorFor(nameof(ContactExportSubscriptionStatusFilter.Operator));
    }

    [Test]
    public void Validator_Should_Fail_WhenOperatorIsInvalid()
    {
        // Arrange
        var filter = new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.None)
        {
            Operator = ContactExportFilterOperator.Unknown,
        };

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(nameof(ContactExportSubscriptionStatusFilter.Value));
        result.ShouldHaveValidationErrorFor(nameof(ContactExportSubscriptionStatusFilter.Operator));
    }

    [Test]
    public void Validator_Should_Pass_ForValidFilter()
    {
        // Arrange
        var filter = new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Subscribed);

        // Act
        var result = ContactExportFilterValidator.Instance.TestValidate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }
}
