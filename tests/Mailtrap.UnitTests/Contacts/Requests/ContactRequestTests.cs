namespace Mailtrap.UnitTests.Contacts.Requests;


[TestFixture]
internal sealed class ContactRequestTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenEmailIsNull()
    {
        var act = () => new ContactRequest(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenEmailIsEmpty()
    {
        var act = () => new ContactRequest(string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var email = TestContext.CurrentContext.Random.GetString(5)
                    + "@"
                    + TestContext.CurrentContext.Random.GetString(5)
                    + ".com";

        // Act
        var request = new ContactRequest(email);

        // Assert
        request.Email.Should().Be(email);
    }

    [Test]
    public void Validate_ShouldFail_WhenProvidedEmailLengthIsInvalid([Values(1, 101)] int length)
    {
        // Arrange
        var email = TestContext.CurrentContext.Random.GetString(length);
        var request = new ContactRequest(email);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
