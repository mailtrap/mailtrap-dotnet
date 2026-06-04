namespace Mailtrap.UnitTests.EmailCampaigns.Requests;


[TestFixture]
internal sealed class CreateEmailCampaignRequestTests
{
    [Test]
    public void Validate_ShouldDelegateToValidator()
    {
        // Valid request passes through the request's own Validate() entry point.
        new CreateEmailCampaignRequest { Name = "Spring Sale", MailsendDomainId = 123 }
            .Validate().IsValid.Should().BeTrue();

        // Missing name fails.
        new CreateEmailCampaignRequest { MailsendDomainId = 123 }
            .Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void ToDto_ShouldWrapBodyUnderEmailCampaignEnvelope()
    {
        // Arrange
        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            MailsendDomainId = 123,
            FromDisplayName = "Acme Marketing",
            FromLocalPart = "news",
            TemplateAttributes = new EmailCampaignTemplateAttributes { Subject = "Spring is here" }
        };

        // Act
        var json = JsonSerializer.Serialize(request.ToDto(), MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().StartWith("{\"email_campaign\":{");
        json.Should().Contain("\"name\":\"Spring Sale\"");
        json.Should().Contain("\"mailsend_domain_id\":123");
        json.Should().Contain("\"template_attributes\":{\"subject\":\"Spring is here\"}");

        // Create payload must NOT carry update-only fields.
        json.Should().NotContain("delivery_mode");
        json.Should().NotContain("scheduled_for");
    }
}
