namespace Mailtrap.UnitTests.EmailCampaigns.Requests;


[TestFixture]
internal sealed class UpdateEmailCampaignRequestTests
{
    [Test]
    public void Validate_ShouldDelegateToValidator()
    {
        // Empty update is valid (all fields optional).
        new UpdateEmailCampaignRequest().Validate().IsValid.Should().BeTrue();

        // Scheduled without scheduled_for fails.
        new UpdateEmailCampaignRequest { DeliveryMode = DeliveryMode.Scheduled }
            .Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void ToDto_ShouldWrapBodyUnderEmailCampaignEnvelope_AndSerializeScheduledForAsIso8601()
    {
        // Arrange
        var request = new UpdateEmailCampaignRequest
        {
            Name = "Spring Sale (updated)",
            DeliveryMode = DeliveryMode.Scheduled,
            ScheduledFor = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero),
            DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
            TemplateAttributes = new EmailCampaignTemplateAttributes { Id = 789, Subject = "New subject" }
        };

        // Act
        var json = JsonSerializer.Serialize(request.ToDto(), MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().StartWith("{\"email_campaign\":{");
        json.Should().Contain("\"delivery_mode\":\"scheduled\"");

        // scheduled_for must be an ISO-8601 string, not .NET ticks or a Unix epoch number.
        json.Should().Contain("\"scheduled_for\":\"2026-06-01T09:00:00");
        json.Should().NotContain("\"scheduled_for\":637");

        json.Should().Contain("\"delivery_options\":{\"emails_per_hour\":1000}");
        json.Should().Contain("\"template_attributes\":{\"id\":789,\"subject\":\"New subject\"}");
    }

    [Test]
    public void ToDto_ShouldOmitUnsetFields()
    {
        // Arrange - PATCH semantics: only set fields are serialized.
        var request = new UpdateEmailCampaignRequest
        {
            Name = "Only the name changes"
        };

        // Act
        var json = JsonSerializer.Serialize(request.ToDto(), MailtrapJsonSerializerOptions.Default);

        // Assert
        json.Should().Contain("\"name\":\"Only the name changes\"");
        json.Should().NotContain("mailsend_domain_id");
        json.Should().NotContain("delivery_mode");
        json.Should().NotContain("scheduled_for");
        json.Should().NotContain("reply_to");
    }
}
