namespace Mailtrap.UnitTests.EmailCampaigns.Requests;


[TestFixture]
internal sealed class CreateEmailCampaignRequestTests
{
    private const long SendingDomainId = 4321;


    [Test]
    public void Validate_ShouldDelegateToValidator()
    {
        // Valid request passes through the request's own Validate() entry point.
        new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            DomainId = SendingDomainId,
            FromLocalPart = "news",
            TemplateAttributes = new EmailCampaignTemplateAttributes { Subject = "Spring is here" }
        }
        .Validate().IsValid.Should().BeTrue();

        // Missing name fails.
        new CreateEmailCampaignRequest
        {
            DomainId = SendingDomainId,
            FromLocalPart = "news",
            TemplateAttributes = new EmailCampaignTemplateAttributes { Subject = "Spring is here" }
        }
        .Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Serialize_ShouldProduceFlatBody()
    {
        // Arrange
        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            DomainId = SendingDomainId,
            FromDisplayName = "Acme Marketing",
            FromLocalPart = "news",
            TemplateAttributes = new EmailCampaignTemplateAttributes { Subject = "Spring is here" },
            DeliveryMode = DeliveryMode.Gradual,
            DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
            ContactListIds = [55, 56],
            ContactSegmentIds = [12]
        };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert - the body must be flat, without the legacy "email_campaign" envelope.
        json.Should().StartWith("{\"name\":\"Spring Sale\"");
        json.Should().NotContain("email_campaign");
        json.Should().Contain($"\"domain_id\":{SendingDomainId}");
        json.Should().Contain("\"template_attributes\":{\"subject\":\"Spring is here\"}");
        json.Should().Contain("\"delivery_mode\":\"gradual\"");
        json.Should().Contain("\"delivery_options\":{\"emails_per_hour\":1000}");
        json.Should().Contain("\"contact_list_ids\":[55,56]");
        json.Should().Contain("\"contact_segment_ids\":[12]");
    }

    [Test]
    public void Serialize_ShouldOmitUnsetOptionalFields()
    {
        // Arrange
        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            DomainId = SendingDomainId,
            FromLocalPart = "news",
            TemplateAttributes = new EmailCampaignTemplateAttributes { Subject = "Spring is here" }
        };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().NotContain("reply_to");
        json.Should().NotContain("delivery_mode");
        json.Should().NotContain("delivery_options");
        json.Should().NotContain("contact_list_ids");
        json.Should().NotContain("contact_segment_ids");
    }
}
