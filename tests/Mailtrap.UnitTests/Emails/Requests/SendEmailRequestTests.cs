namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture]
internal sealed class SendEmailRequestTests
{
    [Test]
    public void Create_Should_ReturnNewInstance_WhenCalled()
    {
        var result = SendEmailRequest.Create();

        result.Should()
            .NotBeNull().And
            .BeOfType<SendEmailRequest>();
    }

    [Test]
    public void Should_SerializeAndDeserializeCorrectly()
    {
        var request = CreateValidRequest();

        var serialized = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.NotIndented);

        var deserialized = JsonSerializer.Deserialize<SendEmailRequest>(serialized, MailtrapJsonSerializerOptions.NotIndented);

        deserialized.Should().BeEquivalentTo(request);
    }

    [Test]
    public void Should_SerializeTemplateVariablesCorrectly()
    {
        var request = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("bill.hero@galaxy.com")
            .Template("ID")
            .TemplateVariables(new { Var1 = "First Name", Var2 = "Last Name" });

        var serialized = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.NotIndented);

        using var doc = JsonDocument.Parse(serialized);
        var root = doc.RootElement;

        root.GetProperty("from").GetProperty("email").GetString().Should().Be("john.doe@demomailtrap.com");
        root.GetProperty("to")[0].GetProperty("email").GetString().Should().Be("bill.hero@galaxy.com");
        root.GetProperty("cc").GetArrayLength().Should().Be(0);
        root.GetProperty("bcc").GetArrayLength().Should().Be(0);
        root.GetProperty("attachments").GetArrayLength().Should().Be(0);
        root.GetProperty("headers").GetRawText().Should().Be("{}");
        root.GetProperty("custom_variables").GetRawText().Should().Be("{}");
        root.GetProperty("template_uuid").GetString().Should().Be("ID");
        root.GetProperty("template_variables").GetProperty("var1").GetString().Should().Be("First Name");
        root.GetProperty("template_variables").GetProperty("var2").GetString().Should().Be("Last Name");

        // Here is how the full JSON looks like:
        // "{" +
        //     "\"from\":{\"email\":\"john.doe@demomailtrap.com\",\"name\":\"John Doe\"}," +
        //     "\"to\":[{\"email\":\"bill.hero@galaxy.com\"}]," +
        //     "\"cc\":[]," +
        //     "\"bcc\":[]," +
        //     "\"attachments\":[]," +
        //     "\"headers\":{}," +
        //     "\"custom_variables\":{}," +
        //     "\"template_uuid\":\"ID\"," +
        //     "\"template_variables\":{\"var1\":\"First Name\",\"var2\":\"Last Name\"}" +
        // "}");
    }

    [Test]
    public void Validate_Should_Fail_WhenRequestIsInvalid()
    {
        var request = SendEmailRequest.Create();

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
    public void Validate_Should_Fail_WhenNoRecipients()
    {
        var req = SendEmailRequest.Create();
        req.From = new EmailAddress("from@example.com");
        req.Subject = "Test";
        req.TextBody = "Body";

        var result = req.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("recipient"));
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
    public void Validate_Should_Pass_WhenValidRecipients()
    {
        var req = SendEmailRequest.Create();
        req.From = new EmailAddress("from@example.com");
        req.Subject = "Test";
        req.TextBody = "Body";
        req.To.Add(new EmailAddress("to@example.com"));

        var result = req.Validate();

        result.IsValid.Should().BeTrue();
    }

    [TestCase(1001, 0, 0, "To")]
    [TestCase(0, 1001, 0, "Cc")]
    [TestCase(0, 0, 1001, "Bcc")]
    public void Validate_Should_Fail_WhenRecipientsIsNotValid(int toCount, int ccCount, int bccCount, string invalidRecipientType)
    {
        var request = SendEmailRequest.Create();
        request.From = new EmailAddress("from@example.com");
        request.Subject = "Test";
        request.TextBody = "Body";

        request.To = Enumerable.Repeat(new EmailAddress("to@example.com"), toCount).ToList();
        request.Cc = Enumerable.Repeat(new EmailAddress("cc@example.com"), ccCount).ToList();
        request.Bcc = Enumerable.Repeat(new EmailAddress("bcc@example.com"), bccCount).ToList();

        var result = request.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains(invalidRecipientType));
    }

    private static SendEmailRequest CreateValidRequest()
    {
        return SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("bill.hero@galaxy.com")
            .Subject("Invitation to Earth")
            .Text("Dear Bill, It will be a great pleasure to see you on our blue planet next weekend. Best regards, John.");
    }
}
