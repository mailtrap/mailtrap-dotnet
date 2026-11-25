using Mailtrap.Webhooks.Models;
using Mailtrap.Webhooks.Requests;


namespace Mailtrap.UnitTests.Webhooks;


[TestFixture]
internal sealed class WebhookCampaignsFieldsTests
{
    [Test]
    public void Deserialize_CampaignsWebhook_MapsTypeAndSubscriptionPreferencesUpdate()
    {
        var json = """
        {
          "id": 8,
          "url": "https://example.com/campaigns",
          "active": true,
          "webhook_type": "campaigns",
          "payload_format": "json",
          "sending_stream": null,
          "domain_id": 4321,
          "event_types": ["delivery", "subscription_preferences_update"]
        }
        """;

        var webhook = JsonSerializer.Deserialize<Webhook>(json, MailtrapJsonSerializerOptions.Default);

        webhook.Should().NotBeNull();
        webhook!.WebhookType.Should().Be(WebhookType.Campaigns);
        webhook.EventTypes.Should().Equal(WebhookEventType.Delivery, WebhookEventType.SubscriptionPreferencesUpdate);
    }

    [Test]
    public void Serialize_CreateCampaignsWebhookRequest_WritesTypeAndEvent()
    {
        var request = new CreateWebhookRequest
        {
            Url = new Uri("https://example.com/campaigns"),
            WebhookType = WebhookType.Campaigns
        };
        request.EventTypes.Add(WebhookEventType.SubscriptionPreferencesUpdate);

        var json = JsonSerializer.Serialize(request, MailtrapJsonSerializerOptions.Default);

        using var doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("webhook_type").GetString().Should().Be("campaigns");
        doc.RootElement.GetProperty("event_types")[0].GetString().Should().Be("subscription_preferences_update");
        doc.RootElement.TryGetProperty("sending_stream", out _).Should().BeFalse();
    }
}
