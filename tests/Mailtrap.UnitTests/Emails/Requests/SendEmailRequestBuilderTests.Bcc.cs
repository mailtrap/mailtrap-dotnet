namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(SendEmailRequestBuilder))]
internal sealed class SendEmailRequestBuilderTests_Bcc
{
    private string RecipientEmail { get; } = "recipient@domain.com";
    private string RecipientDisplayName { get; } = "Recipient";
    private EmailAddress _recipient1 { get; } = new("recipient1@domain.com", "Recipient 1");
    private EmailAddress _recipient2 { get; } = new("recipient2@domain.com");



    #region Bcc

    [Test]
    public void Bcc_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => SendEmailRequestBuilder.Bcc(null!, _recipient1);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Bcc_Should_ThrowArgumentNullException_WhenParamsIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Bcc(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Bcc_Should_NotThrowException_WhenParamsIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Bcc([]);

        act.Should().NotThrow();
    }

    [Test]
    public void Bcc_Should_AddRecipientsToCollection()
    {
        Bcc_CreateAndValidate(_recipient1, _recipient2);
    }

    [Test]
    public void Bcc_Should_AddRecipientsToCollection_WhenCalledMultipleTimes()
    {
        var recipient3 = new EmailAddress("recipient3@domain.com");
        var recipient4 = new EmailAddress("recipient4@domain.com", "Recipient 4");

        var request = Bcc_CreateAndValidate(_recipient1, _recipient2);

        request.Bcc(recipient3, recipient4);

        request.Bcc.Should()
            .HaveCount(4).And
            .ContainInOrder(_recipient1, _recipient2, recipient3, recipient4);
    }

    [Test]
    public void Bcc_Should_NotAddRecipientsToCollection_WhenParamsIsEmpty()
    {
        var request = Bcc_CreateAndValidate(_recipient1, _recipient2);

        request.Bcc([]);

        request.Bcc.Should()
            .HaveCount(2).And
            .ContainInOrder(_recipient1, _recipient2);
    }


    private static SendEmailRequest Bcc_CreateAndValidate(params EmailAddress[] recipients)
    {
        var request = SendEmailRequest
            .Create()
            .Bcc(recipients);

        request.Bcc.Should()
            .HaveCount(2).And
            .ContainInOrder(recipients);

        return request;
    }

    #endregion



    #region Bcc(email, displayName)
    [Test]
    public void Bcc_Should_ThrowArgumentNullException_WhenRequestIsNull_2()
    {
        var request = SendEmailRequest.Create();

        var act = () => SendEmailRequestBuilder.Bcc(null!, RecipientEmail);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Bcc_Should_ThrowArgumentNullException_WhenRecipientEmailIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Bcc(null!, RecipientDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Bcc_Should_ThrowArgumentNullException_WhenRecipientEmailIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Bcc(string.Empty, RecipientDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Bcc_Should_NotThrowException_WhenRecipientDisplayNameIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Bcc(RecipientEmail, null);

        act.Should().NotThrow();
    }

    [TestCase]
    public void Bcc_Should_NotThrowException_WhenRecipientDisplayNameIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Bcc(RecipientEmail, string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void Bcc_Should_AddRecipientToCollection_WhenOnlyEmailProvided()
    {
        var request = SendEmailRequest
            .Create()
            .Bcc(RecipientEmail);

        request.Bcc.Should().ContainSingle();

        var added = request.Bcc.First();
        added.Email.Should().Be(RecipientEmail);
        added.DisplayName.Should().BeNull();
    }

    [Test]
    public void Bcc_Should_AddRecipientToCollection_WhenFullInfoProvided()
    {
        var request = SendEmailRequest
            .Create()
            .Bcc(RecipientEmail, RecipientDisplayName);

        request.Bcc.Should().ContainSingle();

        var added = request.Bcc.First();
        added.Email.Should().Be(RecipientEmail);
        added.DisplayName.Should().Be(RecipientDisplayName);
    }

    #endregion
}
