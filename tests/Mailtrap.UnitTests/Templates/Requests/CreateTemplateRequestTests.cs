namespace Mailtrap.UnitTests.Templates.Requests;


[TestFixture]
internal sealed class CreateTemplateRequestTests
{
    [Test]
    public void Validate_ShouldDelegateToValidator()
    {
        new CreateTemplateRequest { Name = "Welcome", Category = "Onboarding", Subject = "Hello" }
            .Validate().IsValid.Should().BeTrue();

        new CreateTemplateRequest { Category = "Onboarding", Subject = "Hello" }
            .Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Serialize_ShouldProduceFlatBody()
    {
        // Arrange
        var request = new CreateTemplateRequest
        {
            Name = "Welcome",
            Category = "Onboarding",
            Subject = "Hello",
            BodyHtml = "<h1>Hi</h1>",
            BodyText = "Hi"
        };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().Be("{\"name\":\"Welcome\",\"category\":\"Onboarding\",\"subject\":\"Hello\",\"body_html\":\"\\u003Ch1\\u003EHi\\u003C/h1\\u003E\",\"body_text\":\"Hi\"}");
        json.Should().NotContain("email_template");
    }

    [Test]
    public void Serialize_ShouldOmitUnsetBodies()
    {
        // Arrange
        var request = new CreateTemplateRequest { Name = "Welcome", Category = "Onboarding", Subject = "Hello" };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().NotContain("body_html");
        json.Should().NotContain("body_text");
    }

    [Test]
    public void Deserialize_ShouldThrow_WhenRequiredFieldIsMissing()
    {
        // Act
        var act = () => JsonSerializer.Deserialize<CreateTemplateRequest>(
            "{\"name\":\"Welcome\",\"category\":\"Onboarding\"}", MailtrapJsonSerializerOptions.Default);

        // Assert
        act.Should().Throw<JsonException>();
    }
}
