namespace Mailtrap.UnitTests.Emails.Extensions;

[TestFixture]
internal sealed class BatchEmailRequestExtensionsTests
{
    [Test]
    public void GetMergedRequests_Should_ReturnNull_WhenRequestsIsNull()
    {
        // Arrange
        var batch = new BatchEmailRequest
        {
            Base = EmailRequest.Create()
                        .From("base@example.com"),
            Requests = null!
        };

        // Act
        var result = batch.GetMergedRequests();

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void GetMergedRequests_Should_ReturnEmpty_WhenRequestsEmpty()
    {
        // Arrange
        var batch = new BatchEmailRequest
        {
            Base = EmailRequest.Create()
                        .From("base@example.com"),
            Requests = new List<SendEmailRequest>()
        };

        // Act
        var result = batch.GetMergedRequests();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetMergedRequests_Should_UseBaseValues_WhenRequestFieldsAreNull()
    {
        // Arrange
        var baseRequest = new EmailRequest
        {
            From = new EmailAddress("base@example.com"),
            Subject = "Base Subject",
            TextBody = "Base Text",
            HtmlBody = "Base Html"
        };

        var internalRequest = new SendEmailRequest
        {
            From = null,
            Subject = null,
            TextBody = null,
            HtmlBody = null
        };

        var batch = new BatchEmailRequest
        {
            Base = baseRequest,
            Requests = new[] { internalRequest }
        };

        // Act
        var result = batch.GetMergedRequests()!.Single();

        // Assert
        result.From!.Email.Should().Be("base@example.com");
        result.Subject.Should().Be("Base Subject");
        result.TextBody.Should().Be("Base Text");
        result.HtmlBody.Should().Be("Base Html");
    }

    [Test]
    public void GetMergedRequests_Should_PreserveRequestValues_WhenTheyAreNotNull()
    {
        // Arrange
        var baseRequest = EmailRequest.Create()
                            .From("base@example.com")
                            .Subject("Base Subject");


        var internalRequest = SendEmailRequest.Create()
                            .From("custom@example.com")
                            .Subject("Custom Subject");

        var batch = new BatchEmailRequest
        {
            Base = baseRequest,
            Requests = new[] { internalRequest }
        };

        // Act
        var result = batch.GetMergedRequests()!.Single();

        // Assert
        result.From!.Email.Should().Be("custom@example.com");
        result.Subject.Should().Be("Custom Subject");
    }

    [Test]
    public void GetMergedRequests_Should_Work_WhenBaseIsNull()
    {
        // Arrange
        var internalRequest = SendEmailRequest.Create()
                                .From("no-base@example.com");
        var batch = new BatchEmailRequest
        {
            Base = null!,
            Requests = new[] { internalRequest }
        };

        // Act
        var result = batch.GetMergedRequests()!.Single();

        // Assert
        result.Should().BeSameAs(internalRequest);
    }

    [Test]
    public void GetMergedRequests_Should_Merge_Collections_And_Dictionaries()
    {
        // Arrange
        var baseRequest = EmailRequest.Create()
            .Attach(new Attachment("content", "base.pdf"))
            .Header("X-Base", "1")
            .CustomVariable("baseVar", "42")
            .TemplateVariables(new Dictionary<string, object>
            {
                ["one"] = true,
                ["two"] = 2
            });

        var internalRequest = new SendEmailRequest
        {
            Attachments = null!,
            Headers = null!,
            CustomVariables = null!,
            TemplateVariables = null!
        };

        var batch = new BatchEmailRequest
        {
            Base = baseRequest,
            Requests = new[] { internalRequest }
        };

        // Act
        var result = batch.GetMergedRequests()!.Single();

        // Assert
        result.Attachments.Should().ContainSingle().Which.FileName.Should().Be("base.pdf");
        result.Headers.Should().ContainKey("X-Base").WhoseValue.Should().Be("1");
        result.CustomVariables.Should().ContainKey("baseVar").WhoseValue.Should().Be("42");
        result.TemplateVariables.Should().BeAssignableTo<IDictionary<string, object>>();
        var templateVariables = (IDictionary<string, object>)result.TemplateVariables;
        templateVariables.Should().ContainKey("one").WhoseValue.Should().Be(true);
        templateVariables.Should().ContainKey("two").WhoseValue.Should().Be(2);
    }
}
