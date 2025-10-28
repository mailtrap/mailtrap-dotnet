namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture]
internal sealed class BatchEmailRequestTests
{
    [Test]
    public void Create_ShouldReturnNewInstance_WhenCalled()
    {
        var result = BatchEmailRequest.Create();

        result.Should()
            .NotBeNull().And
            .BeOfType<BatchEmailRequest>();
    }

    [Test]
    public void Should_SerializeAndDeserializeCorrectly()
    {
        var request = CreateValidRequest();

        var serialized = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.NotIndented);

        var deserialized = JsonSerializer.Deserialize<BatchEmailRequest>(serialized, MailtrapJsonSerializerOptions.NotIndented);

        deserialized.Should().BeEquivalentTo(request);
    }

    [Test]
    public void Validate_Should_Fail_WhenRequestIsInvalid()
    {
        var request = new BatchEmailRequest();

        var result = request.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("'Requests' must not be empty.");
    }

    [Test]
    public void Validate_Should_Fail_WhenPayloadRequestIsInvalid()
    {
        // Arrange
        var request = BatchEmailRequest.Create();
        request.Requests.Add(SendEmailRequest.Create());

        // Act
        var result = request.Validate();

        // Assert
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

    private static BatchEmailRequest CreateValidRequest()
    {
        var baseRequest = EmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .Subject("Hello World")
            .Text("This is a simple text email body.")
            .Html("<h1>This is a simple HTML email body.</h1>");

        var request = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill, It will be a great pleasure to see you on our blue planet next weekend. Best regards, John.");

        return new BatchEmailRequest
        {
            Base = baseRequest,
            Requests = [request]
        };
    }
}
