namespace Mailtrap.UnitTests.Emails.Responses;


[TestFixture]
internal sealed class SendEmailResponseTests
{
    [Test]
    public void CreateSuccess_Should_InitializeFieldsCorrectly_WhenNoMessageIdsPresent()
    {
        var response = SendEmailResponse.CreateSuccess();

        response.Success.Should().BeTrue();
        response.MessageIds.Should().BeEmpty();
    }

    [Test]
    public void CreateSuccess_Should_InitializeFieldsCorrectly_WhenMessageIdsProvided()
    {
        string[] messageIds = ["id1", "id2"];

        var response = SendEmailResponse.CreateSuccess(messageIds);

        response.Success.Should().BeTrue();
        response.MessageIds.Should().BeEquivalentTo(messageIds);
    }

    [Test]
    public void ShouldDeserializeResponse_WhenSuccess()
    {
        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var responseText =
            "{" +
                "\"success\":true," +
                "\"message_ids\":[" +
                    messageId.AddDoubleQuote() +
                "]" +
            "}";

        var response = JsonSerializer.Deserialize<SendEmailResponse>(responseText, MailtrapJsonSerializerOptions.NotIndented);

        response.Should().NotBeNull();
        response!.Success.Should().BeTrue();
        response!.MessageIds.Should()
            .NotBeNull().And
            .HaveCount(1);
        response!.MessageIds!.Single().Should().Be(messageId);
    }

    [Test]
    public void Should_SerializeAndDeserializeCorrectly()
    {
        string[] messageIds = ["id1", "id2"];
        var response = SendEmailResponse.CreateSuccess(messageIds);
        var options = MailtrapJsonSerializerOptions.NotIndented;

        var json = JsonSerializer.Serialize(response, options);
        var deserialized = JsonSerializer.Deserialize<SendEmailResponse>(json, options);

        deserialized.Should().NotBeNull();
        deserialized!.Success.Should().BeTrue();
        deserialized.MessageIds.Should().BeEquivalentTo(messageIds);
    }
}
