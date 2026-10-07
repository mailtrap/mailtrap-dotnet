namespace Mailtrap.UnitTests.Templates.Requests;


[TestFixture]
internal sealed class UpdateTemplateRequestTests
{
    [Test]
    public void Validate_ShouldDelegateToValidator()
    {
        new UpdateTemplateRequest().Validate().IsValid.Should().BeTrue();

        new UpdateTemplateRequest { Name = string.Empty }
            .Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Serialize_ShouldProduceFlatBody()
    {
        // Arrange
        var request = new UpdateTemplateRequest
        {
            Name = "Welcome v2",
            Category = "Onboarding",
            Subject = "Hello again",
            BodyHtml = "<p>Hi</p>",
            BodyText = "Hi"
        };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().StartWith("{\"name\":\"Welcome v2\",\"category\":\"Onboarding\",\"subject\":\"Hello again\"");
        json.Should().Contain("\"body_html\":");
        json.Should().Contain("\"body_text\":\"Hi\"");
        json.Should().NotContain("email_template");
    }

    [Test]
    public void Serialize_ShouldOmitUnsetFields()
    {
        // Arrange
        var request = new UpdateTemplateRequest { Subject = "Only the subject changes" };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().Be("{\"subject\":\"Only the subject changes\"}");
    }
}
