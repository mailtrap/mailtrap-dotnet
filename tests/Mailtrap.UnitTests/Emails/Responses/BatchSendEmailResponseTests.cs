namespace Mailtrap.UnitTests.Emails.Responses;


[TestFixture]
internal sealed class BatchSendEmailResponseTests
{
    [Test]
    public void CreateSuccess_Should_InitializeFieldsCorrectly_WhenNoDataProvided()
    {
        var response = BatchSendEmailResponse.CreateSuccess();

        response.Success.Should().BeTrue();
        response.MessageIds.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
    }

    [Test]
    public void CreateSuccess_Should_SetSuccessAndMessageIds()
    {
        var messageIds = new[] { "id1", "id2" };
        var response = BatchSendEmailResponse.CreateSuccess(messageIds);

        response.Success.Should().BeTrue();
        response.MessageIds.Should().BeEquivalentTo(messageIds);
        response.Errors.Should().BeEmpty();
    }

    [Test]
    public void CreateFailure_Should_InitializeFieldsCorrectly_WhenNoDataProvided()
    {
        var response = BatchSendEmailResponse.CreateFailure();

        response.Success.Should().BeFalse();
        response.MessageIds.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
    }

    [Test]
    public void CreateFailure_Should_InitializeFieldsCorrectly_WhenErrorsProvided()
    {
        string[] errors = ["error 1", "error 2"];

        var response = BatchSendEmailResponse.CreateFailure(errors);

        response.Success.Should().BeFalse();
        response.MessageIds.Should().BeEmpty();
        response.Errors.Should().BeEquivalentTo(errors);
    }

    [Test]
    public void CreateFailure_Should_InitializeFieldsCorrectly_WhenMessageIdsAndErrorsProvided()
    {
        string[] messageIds = ["id1", "id2"];
        string[] errors = ["error 1", "error 2"];

        var response = BatchSendEmailResponse.CreateFailure(messageIds, errors);

        response.Success.Should().BeFalse();
        response.MessageIds.Should().BeEquivalentTo(messageIds);
        response.Errors.Should().BeEquivalentTo(errors);
    }

    [Test]
    public void Should_DeserializeResponse_WhenErrors()
    {
        var responseText =
            "{" +
                "\"success\":false," +
                "\"errors\":[" +
                    "\"error 1\"," +
                    "\"error 2\"," +
                    "\"error 3\"" +
                "]" +
            "}";

        var response = JsonSerializer.Deserialize<BatchSendEmailResponse>(responseText, MailtrapJsonSerializerOptions.NotIndented);

        response.Should().NotBeNull();
        response!.Success.Should().BeFalse();
        response!.MessageIds.Should().BeEmpty();
        response!.Errors.Should()
            .NotBeNull().And
            .HaveCount(3).And
            .ContainInOrder("error 1", "error 2", "error 3");
    }

    [Test]
    public void Should_DeserializeResponse_WhenMessageIds()
    {
        // Arrange
        var responseText =
            "{" +
                "\"success\":false," +
                "\"message_ids\":[" +
                    "\"id1\"," +
                    "\"id2\"" +
                "]" +
            "}";

        // Act
        var response = JsonSerializer.Deserialize<BatchSendEmailResponse>(responseText, MailtrapJsonSerializerOptions.NotIndented);

        // Assert
        response.Should().NotBeNull();
        response!.Success.Should().BeFalse();
        response!.Errors.Should().BeEmpty();
        response!.MessageIds.Should()
            .NotBeNull().And
            .HaveCount(2).And
            .ContainInOrder("id1", "id2");
    }

    [Test]
    public void Should_DeserializeResponse_WhenMessageIdsAndErrors()
    {
        var responseText =
            "{" +
                "\"success\":false," +
                "\"message_ids\":[" +
                    "\"id1\"," +
                    "\"id2\"" +
                "]," +
                "\"errors\":[" +
                    "\"error 1\"," +
                    "\"error 2\"," +
                    "\"error 3\"" +
                "]" +
            "}";

        var response = JsonSerializer.Deserialize<BatchSendEmailResponse>(responseText, MailtrapJsonSerializerOptions.NotIndented);

        response.Should().NotBeNull();
        response!.Success.Should().BeFalse();
        response!.MessageIds.Should()
            .NotBeNull().And
            .HaveCount(2).And
            .ContainInOrder("id1", "id2");
        response!.Errors.Should()
            .NotBeNull().And
            .HaveCount(3).And
            .ContainInOrder("error 1", "error 2", "error 3");
    }

    [Test]
    public void Should_SerializeAndDeserializeCorrectly()
    {
        var options = MailtrapJsonSerializerOptions.NotIndented;
        var messageIds = new[] { "id1", "id2" };
        var errors = new[] { "err1", "err2" };
        var response = BatchSendEmailResponse.CreateFailure(messageIds, errors);

        var json = JsonSerializer.Serialize(response, options);
        var deserialized = JsonSerializer.Deserialize<BatchSendEmailResponse>(json, options);

        deserialized.Should().NotBeNull();
        deserialized!.Success.Should().BeFalse();
        deserialized.MessageIds.Should().BeEquivalentTo(messageIds);
        deserialized.Errors.Should().BeEquivalentTo(errors);
    }
}
