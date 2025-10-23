namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture]
internal sealed class EmailRequestTests
{
    [Test]
    public void Create_Should_ReturnNewInstance_WhenCalled()
    {
        var result = EmailRequest.Create();

        result.Should()
            .NotBeNull().And
            .BeOfType<EmailRequest>();
    }

    [Test]
    public void Should_SerializeAndDeserializeCorrectly()
    {
        var request = CreateValidRequest();

        var serialized = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.NotIndented);

        var deserialized = JsonSerializer.Deserialize<EmailRequest>(serialized, MailtrapJsonSerializerOptions.NotIndented);

        deserialized.Should().BeEquivalentTo(request);
    }

    [Test]
    public void Validate_Should_Fail_WhenRequestIsInvalid()
    {
        var request = EmailRequest.Create();

        var result = request.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .NotBeEmpty().And
            .Contain("'From' must not be empty.").And
            .Contain("'Subject' must not be empty.").And
            .Contain("'Text Body' must not be empty.").And
            .Contain("'Html Body' must not be empty.");
    }

    [Test]
    public void Validate_Should_Pass_WhenRequestIsValid()
    {
        var request = CreateValidRequest();

        var result = request.Validate();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Validate_Should_Fail_WhenBothTemplateIdAndSubjectAreSet()
    {
        var request = EmailRequest
            .Create()
            .From("from@example.com", "From Name")
            .Template("template-id")
            .Subject("ShouldNotBeSet");

        var result = request.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains(nameof(EmailRequest.Subject)));
    }


    private static EmailRequest CreateValidRequest()
    {
        return EmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .Subject("Invitation to Earth")
            .Text("Dear Bill, It will be a great pleasure to see you on our blue planet next weekend. Best regards, John.");
    }
}
