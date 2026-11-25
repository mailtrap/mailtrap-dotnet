namespace Mailtrap.UnitTests.EmailLogs;


[TestFixture]
internal sealed class EmailLogMessageEventsTests
{
    [Test]
    public void Deserialize_TopicUnsubscribe_MapsDetails()
    {
        var json = """
        {
          "event_type": "topic_unsubscribe",
          "created_at": "2026-05-01T09:00:00Z",
          "details": { "web_ip_address": "198.51.100.50", "subscription_topic_id": 12 }
        }
        """;

        var result = JsonSerializer.Deserialize<EmailLogMessageEvent>(json, MailtrapJsonSerializerOptions.Default);

        var topicEvent = result.Should().BeOfType<EmailLogMessageEventTopicUnsubscribe>().Subject;
        topicEvent.EventType.Should().Be("topic_unsubscribe");
        topicEvent.CreatedAt.Should().Be(new DateTimeOffset(2026, 5, 1, 9, 0, 0, TimeSpan.Zero));
        topicEvent.Details.Should().NotBeNull();
        topicEvent.Details!.WebIpAddress.Should().Be("198.51.100.50");
        topicEvent.Details.SubscriptionTopicId.Should().Be(12);
    }

    [Test]
    public void Deserialize_TopicResubscribe_MapsDetails()
    {
        var json = """
        {
          "event_type": "topic_resubscribe",
          "created_at": "2026-05-02T09:00:00Z",
          "details": { "web_ip_address": null, "subscription_topic_id": 15 }
        }
        """;

        var result = JsonSerializer.Deserialize<EmailLogMessageEvent>(json, MailtrapJsonSerializerOptions.Default);

        var topicEvent = result.Should().BeOfType<EmailLogMessageEventTopicResubscribe>().Subject;
        topicEvent.EventType.Should().Be("topic_resubscribe");
        topicEvent.Details!.WebIpAddress.Should().BeNull();
        topicEvent.Details.SubscriptionTopicId.Should().Be(15);
    }

    [Test]
    public void Deserialize_UnknownEventType_KeepsTypeAndDetails()
    {
        var json = """
        {
          "event_type": "future_event",
          "created_at": "2026-05-03T09:00:00Z",
          "details": { "anything": [1, 2], "nested": { "value": "x" } }
        }
        """;

        var result = JsonSerializer.Deserialize<EmailLogMessageEvent>(json, MailtrapJsonSerializerOptions.Default);

        var unknown = result.Should().BeOfType<EmailLogMessageEventUnknown>().Subject;
        unknown.EventType.Should().Be("future_event");
        unknown.CreatedAt.Should().Be(new DateTimeOffset(2026, 5, 3, 9, 0, 0, TimeSpan.Zero));
        unknown.Details.Should().NotBeNull();
        unknown.Details!.Value.GetProperty("nested").GetProperty("value").GetString().Should().Be("x");
    }

    [Test]
    public void Deserialize_UnknownEventTypeWithoutDetails_HasNullDetails()
    {
        var json = """{ "event_type": "future_event", "details": null }""";

        var result = JsonSerializer.Deserialize<EmailLogMessageEvent>(json, MailtrapJsonSerializerOptions.Default);

        var unknown = result.Should().BeOfType<EmailLogMessageEventUnknown>().Subject;
        unknown.Details.Should().BeNull();
        unknown.CreatedAt.Should().BeNull();
    }

    [Test]
    public void RoundTrip_UnknownEventDirectly_KeepsEventType()
    {
        var json = """{ "event_type": "future_event", "details": { "value": "x" } }""";

        var unknown = JsonSerializer.Deserialize<EmailLogMessageEventUnknown>(json, MailtrapJsonSerializerOptions.Default);

        unknown!.EventType.Should().Be("future_event");
        unknown.Details!.Value.GetProperty("value").GetString().Should().Be("x");

        var written = JsonSerializer.Serialize(unknown, MailtrapJsonSerializerOptions.NotIndented);

        written.Should().StartWith("""{"event_type":"future_event",""");
    }

    [Test]
    public void Deserialize_MessageWithUnknownEvent_DoesNotFail()
    {
        var json = """
        {
          "message_id": "abc",
          "status": "delivered",
          "events": [
            { "event_type": "delivery", "created_at": "2026-05-01T09:00:00Z", "details": { "sending_ip": "192.0.2.1" } },
            { "event_type": "future_event", "created_at": "2026-05-01T10:00:00Z", "details": { "foo": "bar" } },
            { "event_type": "topic_unsubscribe", "created_at": "2026-05-01T11:00:00Z", "details": { "subscription_topic_id": 12 } }
          ]
        }
        """;

        var message = JsonSerializer.Deserialize<EmailLogMessage>(json, MailtrapJsonSerializerOptions.Default);

        message!.Events.Should().HaveCount(3);
        message.Events![0].Should().BeOfType<EmailLogMessageEventDelivery>();
        message.Events[1].Should().BeOfType<EmailLogMessageEventUnknown>()
            .Which.EventType.Should().Be("future_event");
        message.Events[2].Should().BeOfType<EmailLogMessageEventTopicUnsubscribe>();
    }

    [TestCase("delivery", typeof(EmailLogMessageEventDelivery))]
    [TestCase("open", typeof(EmailLogMessageEventOpen))]
    [TestCase("click", typeof(EmailLogMessageEventClick))]
    [TestCase("soft_bounce", typeof(EmailLogMessageEventSoftBounce))]
    [TestCase("bounce", typeof(EmailLogMessageEventBounce))]
    [TestCase("spam", typeof(EmailLogMessageEventSpam))]
    [TestCase("unsubscribe", typeof(EmailLogMessageEventUnsubscribe))]
    [TestCase("suspension", typeof(EmailLogMessageEventSuspension))]
    [TestCase("reject", typeof(EmailLogMessageEventReject))]
    [TestCase("topic_unsubscribe", typeof(EmailLogMessageEventTopicUnsubscribe))]
    [TestCase("topic_resubscribe", typeof(EmailLogMessageEventTopicResubscribe))]
    [TestCase("future_event", typeof(EmailLogMessageEventUnknown))]
    public void RoundTrip_EveryEventType_KeepsTypeFirstAndDetails(string eventType, Type expectedType)
    {
        // Each event type here has a details object; an empty one is enough to check it survives.
        var json = $$$"""{"event_type":"{{{eventType}}}","created_at":"2026-05-01T09:00:00+00:00","details":{}}""";

        var parsed = JsonSerializer.Deserialize<EmailLogMessageEvent>(json, MailtrapJsonSerializerOptions.Default);
        parsed.Should().BeOfType(expectedType);
        parsed!.EventType.Should().Be(eventType);

        // Writing must not recurse into this converter, and must put event_type first.
        var written = JsonSerializer.Serialize(parsed, MailtrapJsonSerializerOptions.NotIndented);
        written.Should().StartWith($"{{\"event_type\":\"{eventType}\",");
        written.Should().NotContain("eventType");
        written.Should().Contain("\"details\":{");

        var reparsed = JsonSerializer.Deserialize<EmailLogMessageEvent>(written, MailtrapJsonSerializerOptions.Default);
        reparsed.Should().BeOfType(expectedType);
        reparsed!.EventType.Should().Be(eventType);
        reparsed.CreatedAt.Should().Be(parsed.CreatedAt);
    }

    [Test]
    public void Serialize_DerivedEventDirectly_WritesEventTypeFirst()
    {
        // A type-level converter isn't inherited, so the derived type serializes on its own, discriminator included.
        var delivery = new EmailLogMessageEventDelivery { Details = new EventDetailsDelivery { SendingIp = "192.0.2.1" } };

        var written = JsonSerializer.Serialize(delivery, MailtrapJsonSerializerOptions.NotIndented);

        written.Should().StartWith("{\"event_type\":\"delivery\",");
        written.Should().Contain("\"sending_ip\":\"192.0.2.1\"");
    }
}
