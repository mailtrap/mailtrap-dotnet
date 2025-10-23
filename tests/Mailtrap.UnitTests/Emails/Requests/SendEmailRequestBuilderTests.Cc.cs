namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(SendEmailRequestBuilder))]
internal sealed class SendEmailRequestBuilderTests_Cc
{
    private string RecipientEmail { get; } = "recipient@domain.com";
    private string RecipientDisplayName { get; } = "Recipient";
    private EmailAddress _recipient1 { get; } = new("recipient1@domain.com", "Recipient 1");
    private EmailAddress _recipient2 { get; } = new("recipient2@domain.com");



    #region Cc

    [Test]
    public void Cc_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => SendEmailRequestBuilder.Cc(null!, _recipient1);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Cc_Should_ThrowArgumentNullException_WhenParamsIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Cc(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Cc_Should_NotThrowException_WhenParamsIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Cc([]);

        act.Should().NotThrow();
    }

    [Test]
    public void Cc_Should_AddRecipientsToCollection()
    {
        Cc_CreateAndValidate(_recipient1, _recipient2);
    }

    [Test]
    public void Cc_Should_AddRecipientsToCollection_WhenCalledMultipleTimes()
    {
        var recipient3 = new EmailAddress("recipient3@domain.com");
        var recipient4 = new EmailAddress("recipient4@domain.com", "Recipient 4");

        var request = Cc_CreateAndValidate(_recipient1, _recipient2);

        request.Cc(recipient3, recipient4);

        request.Cc.Should()
            .HaveCount(4).And
            .ContainInOrder(_recipient1, _recipient2, recipient3, recipient4);
    }

    [Test]
    public void Cc_Should_NotAddRecipientsToCollection_WhenParamsIsEmpty()
    {
        var request = Cc_CreateAndValidate(_recipient1, _recipient2);

        request.Cc([]);

        request.Cc.Should()
            .HaveCount(2).And
            .ContainInOrder(_recipient1, _recipient2);
    }


    private static SendEmailRequest Cc_CreateAndValidate(params EmailAddress[] recipients)
    {
        var request = SendEmailRequest
            .Create()
            .Cc(recipients);

        request.Cc.Should()
            .HaveCount(2).And
            .ContainInOrder(recipients);

        return request;
    }

    #endregion



    #region Cc(email, displayName)

    [Test]
    public void Cc_Should_ThrowArgumentNullException_WhenRequestIsNull_2()
    {
        var request = SendEmailRequest.Create();

        var act = () => SendEmailRequestBuilder.Cc(null!, RecipientEmail);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Cc_Should_ThrowArgumentNullException_WhenRecipientEmailIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Cc(null!, RecipientDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Cc_Should_ThrowArgumentNullException_WhenRecipientEmailIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Cc(string.Empty, RecipientDisplayName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Cc_Should_NotThrowException_WhenRecipientDisplayNameIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Cc(RecipientEmail, null);

        act.Should().NotThrow();
    }

    [TestCase]
    public void Cc_Should_NotThrowException_WhenRecipientDisplayNameIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Cc(RecipientEmail, string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void Cc_Should_AddRecipientToCollection_WhenOnlyEmailProvided()
    {
        var request = SendEmailRequest
            .Create()
            .Cc(RecipientEmail);

        request.Cc.Should().ContainSingle();

        var added = request.Cc.First();
        added.Email.Should().Be(RecipientEmail);
        added.DisplayName.Should().BeNull();
    }

    [Test]
    public void Cc_Should_AddRecipientToCollection_WhenFullInfoProvided()
    {
        var request = SendEmailRequest
            .Create()
            .Cc(RecipientEmail, RecipientDisplayName);

        request.Cc.Should().ContainSingle();

        var added = request.Cc.First();
        added.Email.Should().Be(RecipientEmail);
        added.DisplayName.Should().Be(RecipientDisplayName);
    }

    #endregion
}
