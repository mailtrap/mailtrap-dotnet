
namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture]
internal sealed class BatchEmailRequestTests_Validator_Base
{
    private string _validEmail { get; } = "someone@domain.com";
    private string _invalidEmail { get; } = "someone";
    private string _templateId { get; } = "ID";


    #region Base From

    [Test]
    public void Validation_Base_Should_Fail_WhenSenderEmailIsInvalid()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().From(_invalidEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Base.From!.Email);
    }

    [Test]
    public void Validation_Base_Should_Pass_WhenSenderEmailIsValid()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().From(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.From);
        result.ShouldNotHaveValidationErrorFor(r => r.Base.From!.Email);
    }

    #endregion

    #region Base ReplyTo

    [Test]
    public void Validation_Base_Should_Pass_WhenReplyToIsNull()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().ReplyTo(null));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.ReplyTo);
        result.ShouldNotHaveValidationErrorFor(r => r.Base.ReplyTo!.Email);
    }

    [Test]
    public void Validation_Base_Should_Fail_WhenReplyToEmailIsInvalid()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().ReplyTo(_invalidEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Base.ReplyTo!.Email);
    }

    [Test]
    public void Validation_Base_Should_Pass_WhenReplyToEmailIsValid()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().ReplyTo(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.ReplyTo);
        result.ShouldNotHaveValidationErrorFor(r => r.Base.ReplyTo!.Email);
    }

    #endregion

    #region Base Category

    [Test]
    public void Validation_Base_Should_Fail_WhenCategoryExceedsAllowedLength()
    {
        var @base = EmailRequest.Create()
            .Category(TestContext.CurrentContext.Random.GetString(256));
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Base.Category);
    }

    [Test]
    public void Validation_Base_Should_Pass_WhenCategoryFitsAllowedLength()
    {
        var @base = EmailRequest.Create()
            .Category(TestContext.CurrentContext.Random.GetString(255));
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.Category);
    }

    #endregion

    #region Base Attachments

    [Test]
    public void Validation_Base_Should_Fail_WhenAtLeastOneAttachmentIsInvalid()
    {
        var @base = EmailRequest.Create()
                        .Attach("Any content", "file1.txt")
                        .Attach("Any content", "file2.txt", mimeType: string.Empty);
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Base)}.{nameof(EmailRequest.Attachments)}[1].{nameof(Attachment.MimeType)}");
    }

    [Test]
    public void Validation_Base_Should_Pass_WhenAllAttachmentsAreValid()
    {
        var @base = EmailRequest.Create()
            .Attach("Any content", "file1.txt")
            .Attach("Any content", "file2.txt");
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.Attachments);
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Base)}.{nameof(EmailRequest.Attachments)}[0].{nameof(Attachment.MimeType)}");
    }

    #endregion

    #region Base Template ID

    [Test]
    public void Validation_Base_Should_Fail_WhenTemplateIdIsSetAndSubjectProvided()
    {
        var @base = EmailRequest.Create()
                        .Template(_templateId)
                        .Subject("Subject");
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Base.Subject);
    }

    [Test]
    public void Validation_Base_Should_Fail_WhenTemplateIdIsSetAndTextProvided()
    {
        var @base = EmailRequest.Create()
                        .Template(_templateId)
                        .Text("Text");
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Base.TextBody);
    }

    [Test]
    public void Validation_Base_Should_Fail_WhenTemplateIdIsSetAndHtmlProvided()
    {
        var @base = EmailRequest.Create()
                        .Template(_templateId)
                        .Html("<h1>Header</h1>");
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Base.HtmlBody);
    }

    [Test]
    public void Validation_Base_Should_Pass_WhenTemplateIdIsSetAndNoForbiddenPropertiesAreSet()
    {
        var @base = EmailRequest.Create()
                        .From(_validEmail)
                        .Category("Category")
                        .Template(_templateId);
        var request = BatchEmailRequest.Create()
                        .Base(@base);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base);
    }

    #endregion

    #region Base Subject

    [Test]
    public void Validation_Base_Should_Pass_WhenSubjectIsNull()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.Subject);
    }

    [Test]
    public void Validation_Base_Should_Pass_WhenSubjectIsNotNull()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().Subject("Base Subject"));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.Subject);
    }

    #endregion

    #region Base Body

    [TestCase("Base Body", null)]
    [TestCase("Base Body", "<div>Base Body</div>")]
    [TestCase(null, "<div>Base Body</div>")]
    [TestCase(null, null)]
    public void Validation_Base_Should_Pass_WhenBaseAreValidUseBodyAndHtml(string? textBody, string? htmlBody)
    {
        var @base = EmailRequest.Create()
                        .Text(textBody)
                        .Html(htmlBody);
        var request = BatchEmailRequest.Create()
                        .Base(@base)
                        .Requests(CreateValidRequestList());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion


    #region Helpers


    private static List<SendEmailRequest> CreateValidRequestList()
    {
        return new List<SendEmailRequest>()
            { CreateValidRequest(), CreateValidRequest(), CreateValidRequest() };
    }

    private static SendEmailRequest CreateValidRequest()
    {
        return new()
        {
            To = [new EmailAddress("test@example.com")],
            Cc = [new EmailAddress("test@example.com")],
            Bcc = [new EmailAddress("test@example.com")],
            From = new EmailAddress("sender@example.com"),
            ReplyTo = new EmailAddress("replyto@example.com"),
            Subject = "Test Subject",
            TextBody = "Test Body",
            HtmlBody = "<p>Test Body</p>"
        };
    }

    #endregion
}
