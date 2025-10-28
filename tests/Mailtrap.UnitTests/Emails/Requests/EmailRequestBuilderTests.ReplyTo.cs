namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_ReplyTo
{
    private string ReplyToEmail { get; } = "reply.to@domain.com";
    private string ReplyToDisplayName { get; } = "Reply To";
    private EmailAddress _replyTo { get; } = new("reply.to@domain.com", "Reply To");


    #region ReplyTo(sender)

    [Test]
    public void ReplyTo_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.ReplyTo<EmailRequest>(null!, _replyTo);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ReplyTo_Should_AssignReplyToProperly_WhenNull()
    {
        var request = EmailRequest
            .Create()
            .ReplyTo(null);

        request.ReplyTo.Should().BeNull();
    }

    [Test]
    public void ReplyTo_Should_AssignReplyToProperly()
    {
        var request = EmailRequest
            .Create()
            .ReplyTo(_replyTo);

        request.ReplyTo.Should().BeSameAs(_replyTo);
    }

    [Test]
    public void ReplyTo_Should_Override_WhenCalledSeveralTimes()
    {
        var otherReplyTo = new EmailAddress("replyTo2@domain.com", "Reply To 2");

        var request = EmailRequest
            .Create()
            .ReplyTo(_replyTo)
            .ReplyTo(otherReplyTo);

        request.ReplyTo.Should().BeSameAs(otherReplyTo);
    }

    #endregion


    #region ReplyTo(email, displayName)

    [Test]
    public void ReplyTo_Should_ThrowArgumentNullException_WhenRequestIsNull_WithString()
    {
        var act = () => EmailRequestBuilder.ReplyTo<EmailRequest>(null!, ReplyToEmail);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ReplyTo_Should_ThrowArgumentNullException_WhenReplyToEmailIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.ReplyTo(null!, ReplyToDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ReplyTo_Should_ThrowArgumentNullException_WhenReplyToEmailIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.ReplyTo(string.Empty, ReplyToDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ReplyTo_Should_NotThrowException_WhenReplyToDisplayNameIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.ReplyTo(ReplyToEmail, null);

        act.Should().NotThrow();
    }

    [Test]
    public void ReplyTo_Should_NotThrowException_WhenReplyToDisplayNameIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.ReplyTo(ReplyToEmail, string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void ReplyTo_Should_InitializeReplyToProperly_WhenOnlyEmailProvided()
    {
        var request = EmailRequest
            .Create()
            .ReplyTo(ReplyToEmail);

        request.ReplyTo.Should().NotBeNull();
        request.ReplyTo.Email.Should().Be(ReplyToEmail);
        request.ReplyTo.DisplayName.Should().BeNull();
    }

    [Test]
    public void ReplyTo_Should_InitializeReplyToProperly_WhenFullInfoProvided()
    {
        var request = EmailRequest
            .Create()
            .ReplyTo(ReplyToEmail, ReplyToDisplayName);

        request.ReplyTo.Should().NotBeNull();
        request.ReplyTo.Email.Should().Be(ReplyToEmail);
        request.ReplyTo.DisplayName.Should().Be(ReplyToDisplayName);
    }

    [Test]
    public void ReplyTo_Should_OverrideReplyTo_WhenCalledSeveralTimes_2()
    {
        var otherReplyToEmail = "replyTo2@domain.com";

        var request = EmailRequest
            .Create()
            .ReplyTo(_replyTo)
            .ReplyTo(otherReplyToEmail);

        request.ReplyTo.Should().NotBeNull();
        request.ReplyTo.Should().NotBeSameAs(_replyTo);
        request.ReplyTo.Email.Should().Be(otherReplyToEmail);
        request.ReplyTo.DisplayName.Should().BeNull();
    }

    #endregion
}
