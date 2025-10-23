namespace Mailtrap.UnitTests.Emails.Responses;


[TestFixture]
internal sealed class BatchEmailResponseTests
{
    [Test]
    public void CreateSuccess_Should_InitializeFieldsCorrectly_WhenNoMessageIdsPresent()
    {
        var response = BatchEmailResponse.CreateSuccess();

        response.Success.Should().BeTrue();
        response.Responses.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
    }

    [Test]
    public void CreateSuccess_Should_InitializeFieldsCorrectly_WhenBatchResponsesProvided()
    {
        BatchSendEmailResponse[] responses =
            [
                BatchSendEmailResponse.CreateSuccess("id1", "id2"),
                BatchSendEmailResponse.CreateSuccess("id3"),
                BatchSendEmailResponse.CreateFailure("error 1"),
                BatchSendEmailResponse.CreateFailure("error 2", "error 3")
            ];

        var response = BatchEmailResponse.CreateSuccess(responses);

        response.Success.Should().BeTrue();
        response.Responses.Should().BeEquivalentTo(responses);
        response.Errors.Should().BeEmpty();
    }

    [Test]
    public void CreateFailure_Should_InitializeFieldsCorrectly_WhenNoErrorsProvided()
    {
        var response = BatchEmailResponse.CreateFailure();

        response.Success.Should().BeFalse();
        response.Responses.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
    }

    [Test]
    public void CreateFailure_Should_InitializeFieldsCorrectly_WhenErrorsProvided()
    {
        var response = BatchEmailResponse.CreateFailure("error 1", "error 2", "error 3");

        response.Success.Should().BeFalse();
        response.Responses.Should().BeEmpty();
        response.Errors.Should().NotBeEmpty().And.ContainInOrder("error 1", "error 2", "error 3");
    }

    [Test]
    public void Should_DeserializeResponse_WhenSuccess()
    {
        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var responseText =
            "{" +
                "\"success\":true," +
                "\"responses\":[" +
                    "{" +
                        "\"success\":true," +
                        "\"message_ids\":[" +
                            messageId.AddDoubleQuote() +
                        "]" +
                    "}" +
                "]," +
                 "\"errors\":[" +
                    "\"error 1\"," +
                    "\"error 2\"," +
                    "\"error 3\"" +
                "]" +
            "}";

        var response = JsonSerializer.Deserialize<BatchEmailResponse>(responseText, MailtrapJsonSerializerOptions.NotIndented);

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Errors.Should()
            .NotBeNull().And
            .HaveCount(3).And
            .ContainInOrder("error 1", "error 2", "error 3");
        response.Responses.Should()
            .NotBeNull().And
            .ContainSingle();

        var messageResponse = response.Responses.Single();

        messageResponse.Success.Should().BeTrue();
        messageResponse.Errors.Should().BeEmpty();
        messageResponse.MessageIds.Should()
            .NotBeNull().And
            .HaveCount(1).And
            .BeEquivalentTo(new[] { messageId });
    }

    [Test]
    public void Should_DeserializeResponse_WhenFailure()
    {
        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var responseText =
            "{" +
                "\"success\":false," +
                "\"responses\":[" +
                    "{" +
                        "\"success\":false," +
                        "\"message_ids\":[" +
                            messageId.AddDoubleQuote() +
                        "]," +
                        "\"errors\":[" +
                            "\"error 1\"," +
                            "\"error 2\"" +
                        "]" +
                    "}" +
                "]," +
                 "\"errors\":[" +
                    "\"error 1\"," +
                    "\"error 2\"," +
                    "\"error 3\"" +
                "]" +
            "}";

        var response = JsonSerializer.Deserialize<BatchEmailResponse>(responseText, MailtrapJsonSerializerOptions.NotIndented);

        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Errors.Should()
            .NotBeNull().And
            .HaveCount(3).And
            .ContainInOrder("error 1", "error 2", "error 3");
        response.Responses.Should()
            .NotBeNull().And
            .ContainSingle();

        var messageResponse = response.Responses.Single();

        messageResponse.Success.Should().BeFalse();
        messageResponse.Errors.Should()
            .NotBeNull().And
            .HaveCount(2).And
            .ContainInOrder("error 1", "error 2");
        messageResponse.MessageIds.Should()
            .NotBeNull().And
            .ContainSingle();

        messageResponse.MessageIds.Single().Should().Be(messageId);
    }

    [Test]
    public void Should_SerializeAndDeserializeCorrectly()
    {
        var options = MailtrapJsonSerializerOptions.NotIndented;
        var messageIds = new[] { "id1", "id2" };
        var errors = new[] { "err1", "err2" };
        var resp1 = BatchSendEmailResponse.CreateSuccess(messageIds);
        var resp2 = BatchSendEmailResponse.CreateFailure(errors);

        var response = BatchEmailResponse.CreateSuccess(resp1, resp2);

        var json = JsonSerializer.Serialize(response, options);
        var deserialized = JsonSerializer.Deserialize<BatchEmailResponse>(json, options);

        deserialized.Should().NotBeNull();
        deserialized!.Success.Should().BeTrue();
        deserialized.Errors.Should().BeEmpty();
        deserialized.Responses.Should().HaveCount(2);
        deserialized.Responses[0].MessageIds.Should().BeEquivalentTo(messageIds);
        deserialized.Responses[1].Errors.Should().BeEquivalentTo(errors);
    }
}
