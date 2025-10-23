namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_From
{
    private string SenderEmail { get; } = "sender@domain.com";
    private string SenderDisplayName { get; } = "Sender";
    private EmailAddress _sender { get; } = new("sender@domain.com", "Sender");


    #region From(sender)

    [Test]
    public void From_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.From<EmailRequest>(null!, _sender);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void From_Should_ThrowArgumentNullException_WhenSenderIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.From(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void From_Should_AssignSenderProperly()
    {
        var request = EmailRequest
            .Create()
            .From(_sender);

        request.From.Should().BeSameAs(_sender);
    }

    [Test]
    public void From_Should_OverrideSender_WhenCalledSeveralTimes()
    {
        var otherSender = new EmailAddress("sender2@domain.com", "Sender 2");

        var request = EmailRequest
            .Create()
            .From(_sender)
            .From(otherSender);

        request.From.Should().BeSameAs(otherSender);
    }

    #endregion


    #region From(email, displayName)

    [Test]
    public void From_Should_ThrowArgumentNullException_WhenRequestIsNull_WithString()
    {
        var act = () => EmailRequestBuilder.From<EmailRequest>(null!, SenderEmail);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void From_Should_ThrowArgumentNullException_WhenSenderEmailIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.From(null!, SenderDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void From_Should_ThrowArgumentNullException_WhenSenderEmailIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.From(string.Empty, SenderDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void From_Should_NotThrowException_WhenSenderDisplayNameIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.From(SenderEmail, null);

        act.Should().NotThrow();
    }

    [Test]
    public void From_Should_NotThrowException_WhenSenderDisplayNameIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.From(SenderEmail, string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void From_Should_InitializeSenderProperly_WhenOnlyEmailProvided()
    {
        var request = EmailRequest
            .Create()
            .From(SenderEmail);

        request.From.Should().NotBeNull();
        request.From.Email.Should().Be(SenderEmail);
        request.From.DisplayName.Should().BeNull();
    }

    [Test]
    public void From_Should_InitializeSenderProperly_WhenFullInfoProvided()
    {
        var request = EmailRequest
            .Create()
            .From(SenderEmail, SenderDisplayName);

        request.From.Should().NotBeNull();
        request.From.Email.Should().Be(SenderEmail);
        request.From.DisplayName.Should().Be(SenderDisplayName);
    }

    [Test]
    public void From_Should_OverrideSender_WhenCalledSeveralTimes_WithString()
    {
        var otherSenderEmail = "sender2@domain.com";

        var request = EmailRequest
            .Create()
            .From(_sender)
            .From(otherSenderEmail);

        request.From.Should().NotBeSameAs(_sender);
        request.From.Email.Should().Be(otherSenderEmail);
        request.From.DisplayName.Should().BeNull();
    }

    #endregion
}
