namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture]
internal sealed class EmailRequestTests_Validator
{
    private string _validEmail { get; } = "someone@domain.com";
    private string _invalidEmail { get; } = "someone";
    private string _templateId { get; } = "ID";



    #region From

    [Test]
    public void Validation_Should_Fail_WhenSenderEmailIsInvalid()
    {
        var request = EmailRequest
            .Create()
            .From(_invalidEmail);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.From!.Email);
    }

    [Test]
    public void Validation_Should_Pass_WhenSenderEmailIsValid()
    {
        var request = EmailRequest
            .Create()
            .From(_validEmail);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.From);
        result.ShouldNotHaveValidationErrorFor(r => r.From!.Email);
    }

    #endregion



    #region ReplyTo

    [Test]
    public void Validation_Should_Pass_WhenReplyToIsNull()
    {
        var request = EmailRequest.Create();

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.ReplyTo);
        result.ShouldNotHaveValidationErrorFor(r => r.ReplyTo!.Email);
    }

    [Test]
    public void Validation_Should_Fail_WhenReplyToEmailIsInvalid()
    {
        var request = EmailRequest
            .Create()
            .ReplyTo(_invalidEmail);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.ReplyTo!.Email);
    }

    [Test]
    public void Validation_Should_Pass_WhenReplyToEmailIsValid()
    {
        var request = EmailRequest
            .Create()
            .ReplyTo(_validEmail);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.ReplyTo);
        result.ShouldNotHaveValidationErrorFor(r => r.ReplyTo!.Email);
    }

    #endregion



    #region Attachments

    [Test]
    public void Validation_Should_Fail_WhenAtLeastOneAttachmentIsInvalid()
    {
        var request = EmailRequest
            .Create()
            .Attach("Any content", "file1.txt")
            .Attach("Any content", "file2.txt", mimeType: string.Empty);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(EmailRequest.Attachments)}[1].{nameof(Attachment.MimeType)}");
    }

    [Test]
    public void Validation_Should_Pass_WhenAllAttachmentsAreValid()
    {
        var request = EmailRequest
            .Create()
            .Attach("Any content", "file1.txt")
            .Attach("Any content", "file2.txt");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Attachments);
        result.ShouldNotHaveValidationErrorFor($"{nameof(EmailRequest.Attachments)}[0].{nameof(Attachment.MimeType)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(EmailRequest.Attachments)}[1].{nameof(Attachment.MimeType)}");
    }

    #endregion



    #region Templated

    [Test]
    public void Validation_Should_Fail_WhenTemplateIdIsSetAndSubjectProvided()
    {
        var request = EmailRequest
            .Create()
            .Template(_templateId)
            .Subject("Subject");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Subject);
    }

    [Test]
    public void Validation_Should_Fail_WhenTemplateIdIsSetAndTextProvided()
    {
        var request = EmailRequest
            .Create()
            .Template(_templateId)
            .Text("Content");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.TextBody);
    }

    [Test]
    public void Validation_Should_Fail_WhenTemplateIdIsSetAndHtmlProvided()
    {
        var request = EmailRequest
            .Create()
            .Template(_templateId)
            .Html("<h1>Header</h1>");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.HtmlBody);
    }

    [Test]
    public void Validation_Should_Pass_WhenTemplateIdIsSetAndNoForbiddenPropertiesAreSet()
    {
        var request = EmailRequest
            .Create()
            .From(_validEmail)
            .Template(_templateId);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    #endregion



    #region Subject

    [Test]
    public void Validation_Should_Fail_WhenSubjectIsNull()
    {
        var request = EmailRequest.Create();

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Subject);
    }

    [Test]
    public void Validation_Should_Pass_WhenSubjectProvided()
    {
        var request = EmailRequest.Create()
            .Subject("Subject");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Subject);
    }

    #endregion



    #region Category

    [Test]
    public void Validation_Should_Fail_WhenCategoryExceedsAllowedLength()
    {
        var request = EmailRequest
            .Create()
            .Category(TestContext.CurrentContext.Random.GetString(256));

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Category);
    }

    [Test]
    public void Validation_Should_Pass_WhenCategoryFitsAllowedLength()
    {
        var request = EmailRequest
            .Create()
            .Category(TestContext.CurrentContext.Random.GetString(255));

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Category);
    }

    #endregion



    #region Body

    [Test]
    public void Validation_Should_Fail_WhenBothHtmlAndTextBodyAreNull()
    {
        var request = EmailRequest.Create();

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.TextBody);
        result.ShouldHaveValidationErrorFor(r => r.HtmlBody);
    }

    [Test]
    public void Validation_Should_Fail_WhenBothHtmlAndTextBodyAreEmpty()
    {
        var request = EmailRequest
            .Create()
            .Text(string.Empty)
            .Html(string.Empty);

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.TextBody);
        result.ShouldHaveValidationErrorFor(r => r.HtmlBody);
    }

    [Test]
    public void Validation_Should_Pass_WhenTextBodyIsNotEmpty()
    {
        var request = EmailRequest
            .Create()
            .Text("Text");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.TextBody);
        result.ShouldNotHaveValidationErrorFor(r => r.HtmlBody);
    }

    [Test]
    public void Validation_Should_Pass_WhenHtmlBodyIsNotEmpty()
    {
        var request = EmailRequest
            .Create()
            .Html("<h1>Html</h1>");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.TextBody);
        result.ShouldNotHaveValidationErrorFor(r => r.HtmlBody);
    }

    [Test]
    public void Validation_Should_Pass_WhenBothHtmlAndTextBodyAreNotEmpty()
    {
        var request = EmailRequest
            .Create()
            .Text("Text")
            .Html("<h1>Html</h1>");

        var result = EmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.TextBody);
        result.ShouldNotHaveValidationErrorFor(r => r.HtmlBody);
    }

    #endregion
}
