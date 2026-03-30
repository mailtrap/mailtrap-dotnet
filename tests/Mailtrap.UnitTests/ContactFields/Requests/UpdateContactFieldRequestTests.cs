namespace Mailtrap.UnitTests.ContactFields.Requests;


[TestFixture]
internal sealed class UpdateContactFieldRequestTests
{
    [Test]
    public void CopyConstructor_ShouldUpdateFieldsCorrectly()
    {
        const string newName = "newValidName";
        const string newMergeTag = "newValidMergeTag";

        var originalRequest = new UpdateContactFieldRequest("validName", "validMergeTag");
        var copiedRequest = originalRequest with
        {
            Name = newName,
            MergeTag = newMergeTag
        };
        copiedRequest.Name.Should().Be(newName);
        copiedRequest.MergeTag.Should().Be(newMergeTag);
    }
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenNameAndMergeTagIsNull()
    {
        var act = () => new UpdateContactFieldRequest(null, null);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenNameAndMergeTagIsEmpty()
    {
        var act = () => new UpdateContactFieldRequest(string.Empty, string.Empty);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ShouldInitializeNameAndMergeTagFieldsCorrectly()
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(5);
        var mergeTag = TestContext.CurrentContext.Random.GetString(5);

        // Act
        var request = new UpdateContactFieldRequest(name, mergeTag);

        // Assert
        request.Name.Should().Be(name);
        request.MergeTag.Should().Be(mergeTag);
    }

    [Test]
    public void Constructor_ShouldInitializeFieldsCorrectly_WhenNameIsNull()
    {
        // Arrange
        var mergeTag = TestContext.CurrentContext.Random.GetString(5);

        // Act
        var request = new UpdateContactFieldRequest(null, mergeTag);

        // Assert
        request.Name.Should().Be(null);
        request.MergeTag.Should().Be(mergeTag);
    }

    [Test]
    public void Constructor_ShouldInitializeFieldsCorrectly_WhenNameIsEmpty()
    {
        // Arrange
        var mergeTag = TestContext.CurrentContext.Random.GetString(5);

        // Act
        var request = new UpdateContactFieldRequest(string.Empty, mergeTag);

        // Assert
        request.Name.Should().Be(string.Empty);
        request.MergeTag.Should().Be(mergeTag);
    }

    [Test]
    public void Constructor_ShouldInitializeFieldsCorrectly_WhenMergeTagIsNull()
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(5);

        // Act
        var request = new UpdateContactFieldRequest(name, null);

        // Assert
        request.Name.Should().Be(name);
        request.MergeTag.Should().Be(null);
    }

    [Test]
    public void Constructor_ShouldInitializeFieldsCorrectly_WhenMergeTagIsEmpty()
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(5);

        // Act
        var request = new UpdateContactFieldRequest(name, string.Empty);

        // Assert
        request.Name.Should().Be(name);
        request.MergeTag.Should().Be(string.Empty);
    }

    [Test]
    public void Validate_ShouldPass_WhenProvidedFieldsIsValid()
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(10);
        var mergeTag = TestContext.CurrentContext.Random.GetString(10);
        var request = new UpdateContactFieldRequest(name, mergeTag);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_ShouldPass_WhenProvidedNameIsNull()
    {
        // Arrange
        string? name = null;
        var mergeTag = TestContext.CurrentContext.Random.GetString(10);
        var request = new UpdateContactFieldRequest(name, mergeTag);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_ShouldPass_WhenProvidedNameIsEmpty()
    {
        // Arrange
        var name = string.Empty;
        var mergeTag = TestContext.CurrentContext.Random.GetString(10);
        var request = new UpdateContactFieldRequest(name, mergeTag);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_ShouldPass_WhenProvidedMergeTagIsNull()
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(10);
        var request = new UpdateContactFieldRequest(name, null);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_ShouldPass_WhenProvidedMergeTagIsEmpty()
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(10);
        var mergeTag = string.Empty;
        var request = new UpdateContactFieldRequest(name, mergeTag);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_ShouldFail_WhenProvidedNameLengthIsInvalid([Values(81)] int length)
    {
        // Arrange
        var name = TestContext.CurrentContext.Random.GetString(length);
        var request = new UpdateContactFieldRequest(name, "validMergeTag");

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public void Validate_ShouldFail_WhenProvidedMergeTagLengthIsInvalid([Values(81)] int length)
    {
        // Arrange
        var mergeTag = TestContext.CurrentContext.Random.GetString(length);
        var request = new UpdateContactFieldRequest("validName", mergeTag);

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
