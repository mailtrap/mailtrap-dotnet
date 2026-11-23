namespace Mailtrap.UnitTests.EmailCampaigns.Requests;


[TestFixture]
internal sealed class UpdateEmailCampaignRequestTests
{
    [Test]
    public void Validate_ShouldDelegateToValidator()
    {
        // Empty update is valid (all fields optional).
        new UpdateEmailCampaignRequest().Validate().IsValid.Should().BeTrue();

        // A non-positive sending domain identifier fails.
        new UpdateEmailCampaignRequest { DomainId = 0 }
            .Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Serialize_ShouldProduceFlatBody()
    {
        // Arrange
        var request = new UpdateEmailCampaignRequest
        {
            Name = "Spring Sale (updated)",
            DeliveryMode = DeliveryMode.Gradual,
            DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
            TemplateAttributes = new EmailCampaignTemplateAttributes
            {
                Subject = "New subject",
                BodyHtml = "<html><body><a href=\"__unsubscribe_url__\">Unsubscribe</a></body></html>",
                MergeTags = ["first_name"]
            },
            ContactListIds = [55],
            ContactSegmentIds = []
        };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert - the body must be flat, without the legacy "email_campaign" envelope.
        json.Should().StartWith("{\"name\":\"Spring Sale (updated)\"");
        json.Should().NotContain("email_campaign");
        json.Should().Contain("\"delivery_mode\":\"gradual\"");
        json.Should().Contain("\"delivery_options\":{\"emails_per_hour\":1000}");
        json.Should().Contain("\"subject\":\"New subject\"");
        json.Should().Contain("\"body_html\":");
        json.Should().Contain("\"merge_tags\":[\"first_name\"]");

        // An empty list is serialized (clears the audience), unlike an unset (null) one.
        json.Should().Contain("\"contact_list_ids\":[55]");
        json.Should().Contain("\"contact_segment_ids\":[]");
    }

    [Test]
    public void Serialize_ShouldOmitUnsetFields()
    {
        // Arrange - PATCH semantics: only set fields are serialized.
        var request = new UpdateEmailCampaignRequest
        {
            Name = "Only the name changes"
        };

        // Act
        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().Contain("\"name\":\"Only the name changes\"");
        json.Should().NotContain("domain_id");
        json.Should().NotContain("delivery_mode");
        json.Should().NotContain("reply_to");
        json.Should().NotContain("template_attributes");
        json.Should().NotContain("contact_list_ids");
        json.Should().NotContain("contact_segment_ids");
    }
}
