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

    [Test]
    public void Validate_ShouldPass_WhenSubscriptionTopicsAreValid()
    {
        // Arrange
        var request = new UpdateContactRequest("john.smith@example.com")
        {
            SubscriptionTopics =
            [
                new ContactSubscriptionTopic { SubscriptionTopicId = 12, DomainId = 4321, Status = ContactStatus.Unsubscribed },
                new ContactSubscriptionTopic { SubscriptionTopicId = 15, DomainId = 4321, Status = ContactStatus.Subscribed }
            ]
        };

        // Act
        var result = request.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCase(0, 4321)]
    [TestCase(-1, 4321)]
    [TestCase(12, 0)]
    [TestCase(12, -1)]
    public void Validate_ShouldFail_WhenTopicOrDomainIdIsNotPositive(long subscriptionTopicId, long domainId)
    {
        var request = RequestWithTopic(new ContactSubscriptionTopic
        {
            SubscriptionTopicId = subscriptionTopicId,
            DomainId = domainId,
            Status = ContactStatus.Subscribed
        });

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Validate_ShouldFail_WhenStatusIsNotSet()
    {
        var request = RequestWithTopic(new ContactSubscriptionTopic { SubscriptionTopicId = 12, DomainId = 4321 });

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Validate_ShouldFail_WhenSubscriptionTopicIsNull()
    {
        var request = RequestWithTopic(null!);

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Serialize_ShouldWriteSubscriptionTopicEntries()
    {
        // Arrange
        var request = RequestWithTopic(new ContactSubscriptionTopic
        {
            SubscriptionTopicId = 12,
            DomainId = 4321,
            Status = ContactStatus.Unsubscribed
        });

        // Act
        var json = JsonSerializer.Serialize(request.ToDto(), MailtrapJsonSerializerOptions.Default);

        // Assert - every entry carries all three keys.
        json.Should().Contain("\"subscription_topics\":[{\"subscription_topic_id\":12,\"domain_id\":4321,\"status\":\"unsubscribed\"}]");
    }

    [Test]
    public void Serialize_ShouldOmitSubscriptionTopics_WhenNotSet()
    {
        // Arrange
        var request = new UpdateContactRequest("john.smith@example.com");

        // Act
        var json = JsonSerializer.Serialize(request.ToDto(), MailtrapJsonSerializerOptions.Default);

        // Assert - leaving them out keeps the contact's topic preferences unchanged.
        json.Should().NotContain("subscription_topics");
    }


    private static UpdateContactRequest RequestWithTopic(ContactSubscriptionTopic topic)
        => new("john.smith@example.com") { SubscriptionTopics = [topic] };
}
