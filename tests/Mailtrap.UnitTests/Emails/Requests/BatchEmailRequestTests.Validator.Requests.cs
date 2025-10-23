namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture]
internal sealed class BatchEmailRequestTests_Validator_Requests
{
    private string _validEmail { get; } = "someone@domain.com";
    private string _invalidEmail { get; } = "someone";
    private string _templateId { get; } = "ID";

    #region Requests
    [Test]
    public void Validation_Should_Fail_WhenRequestsAreNull()
    {
        var request = new BatchEmailRequest()
        {
            Base = CreateValidBaseRequest(),
            Requests = null!
        };

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Requests);
    }

    [Test]
    public void Validation_Should_Fail_WhenRequestsAreEmpty()
    {
        var request = BatchEmailRequest.Create()
                        .Base(CreateValidBaseRequest())
                        .Requests(new List<SendEmailRequest>());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Requests);
    }

    [Test]
    public void Validation_Should_Fail_WhenRequestsCountIsGreaterThan500([Values(501)] int count)
    {
        var request = BatchEmailRequest.Create()
                        .Base(CreateValidBaseRequest())
                        .Requests(Enumerable.Repeat(SendEmailRequest.Create(), count).ToList());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Requests.Count);
    }

    [Test]
    public void Validation_Should_Pass_WhenRequestsCountIsLessOrEqualTo500([Values(1, 500)] int count)
    {
        var request = BatchEmailRequest.Create()
                        .Base(CreateValidBaseRequest())
                        .Requests(Enumerable.Repeat(SendEmailRequest.Create(), count).ToList());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Requests.Count);
    }

    [Test]
    public void Validation_Should_Pass_WhenRequestsAndBaseAreValid()
    {
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().From(_validEmail))
                        .Requests(CreateValidRequestList());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validation_Should_Pass_WhenNoBaseAndRequestsAreValid()
    {
        var request = BatchEmailRequest.Create()
            .Requests(CreateValidRequestList());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion


    #region Request items

    [Test]
    public void Validation_Requests_Should_Fail_WhenItemIsNull()
    {
        var request = BatchEmailRequest.Create();
        request.Requests.Add(null!);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0]");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenNoRecipientsPresent()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenOnlyToRecipientsPresent()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().To(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r);
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenOnlyCcRecipientsPresent()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().Cc(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r);
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenOnlyBccRecipientsPresent()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().Bcc(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r);
    }

    #endregion



    #region Requests From

    [Test]
    public void Validation_Requests_Should_Fail_WhenSenderEmailIsInvalid()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().From(_invalidEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.From)}.{nameof(EmailAddress.Email)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenSenderEmailIsValid()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().From(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.From)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.From)}.{nameof(EmailAddress.Email)}");
    }

    #endregion



    #region Requests ReplyTo

    [Test]
    public void Validation_Requests_Should_Pass_WhenReplyToIsNull()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().ReplyTo(null));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.ReplyTo)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.ReplyTo)}.{nameof(EmailAddress.Email)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenReplyToEmailIsInvalid()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().ReplyTo(_invalidEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.ReplyTo)}.{nameof(EmailAddress.Email)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenReplyToEmailIsValid()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().ReplyTo(_validEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.ReplyTo)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.ReplyTo)}.{nameof(EmailAddress.Email)}");
    }

    #endregion



    #region Requests To

    [Test]
    public void Validation_Requests_Should_Fail_WhenToLengthExceedsLimit([Values(1001)] int count)
    {
        var request = BatchEmailRequest.Create()
                        .Requests(r => r
                            .To(Enumerable.Repeat(new EmailAddress(_validEmail), count).ToArray()));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.To)}");

    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenToLengthWithinLimit([Values(1, 500, 1000)] int count)
    {
        var request = BatchEmailRequest.Create()
                        .Requests(r => r
                            .To(Enumerable.Repeat(new EmailAddress(_validEmail), count).ToArray()));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.To)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenAtLeastOneToEmailIsInvalid()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(r => r
                            .To(_validEmail)
                            .To(_invalidEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        // Unfortunately we can't use expressions for collection properties,
        // we can't use nameof with indexer either,
        // thus hardcoding the property name.
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.To)}[1].{nameof(EmailAddress.Email)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenAllToEmailsAreValid()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(r => r
                            .To(_validEmail)
                            .To("other@domain.com"));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.To)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.To)}[0].{nameof(EmailAddress.Email)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.To)}[1].{nameof(EmailAddress.Email)}");
    }

    #endregion



    #region Request Cc

    [Test]
    public void Validation_Requests_Should_Fail_WhenCcLengthExceedsLimit([Values(1001)] int count)
    {
        var internalRequest = SendEmailRequest.Create();
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        for (var i = 1; i <= count; i++)
        {
            internalRequest.Cc($"recipient{i}@domain.com");
        }

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Cc)}");

    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenCcLengthWithinLimit([Values(1, 500, 1000)] int count)
    {
        var internalRequest = SendEmailRequest.Create();
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        for (var i = 1; i <= count; i++)
        {
            internalRequest.Cc($"recipient{i}@domain.com");
        }

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Cc)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenAtLeastOneCcEmailIsInvalid()
    {
        var request = BatchEmailRequest.Create();
        request.Requests.Add(SendEmailRequest.Create().Cc(_validEmail).Cc(_invalidEmail));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Cc)}[1].{nameof(EmailAddress.Email)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenAllCcEmailsAreValid()
    {
        var request = BatchEmailRequest.Create();
        request.Requests.Add(SendEmailRequest.Create().Cc(_validEmail).Cc("other@domain.com"));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Cc)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Cc)}[0].{nameof(EmailAddress.Email)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Cc)}[1].{nameof(EmailAddress.Email)}");
    }

    #endregion



    #region Request Bcc

    [Test]
    public void Validation_Requests_Should_Fail_WhenBccLengthExceedsLimit([Values(1001)] int count)
    {
        var internalRequest = SendEmailRequest.Create();
        for (var i = 1; i <= count; i++)
        {
            internalRequest.Bcc($"recipient{i}@domain.com");
        }

        var request = BatchEmailRequest.Create().Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Bcc)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenBccLengthWithinLimit([Values(1, 500, 1000)] int count)
    {
        var internalRequest = SendEmailRequest.Create();
        for (var i = 1; i <= count; i++)
        {
            internalRequest.Bcc($"recipient{i}@domain.com");
        }

        var request = BatchEmailRequest.Create().Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Bcc)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenAtLeastOneBccEmailIsInvalid()
    {
        var internalRequest = SendEmailRequest.Create()
            .Bcc(_validEmail)
            .Bcc(_invalidEmail);
        var request = BatchEmailRequest.Create().Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Bcc)}[1].{nameof(EmailAddress.Email)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenAllBccEmailsAreValid()
    {
        var internalRequest = SendEmailRequest.Create()
            .Bcc(_validEmail)
            .Bcc("other@domain.com");
        var request = BatchEmailRequest.Create().Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Bcc)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Bcc)}[0].{nameof(EmailAddress.Email)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Bcc)}[1].{nameof(EmailAddress.Email)}");
    }

    #endregion

    #region Request To, Cc, Bcc total

    [Test]
    public void Validation_Requests_Should_Fail_WhenNoRecipients()
    {
        var request = BatchEmailRequest.Create()
            .Requests(r => r
                .From("from@example.com")
                .Subject("Test")
                .Text("Body"));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenRecipientsAreNull()
    {
        var request = BatchEmailRequest.Create()
            .Requests(r => r
                .From("from@example.com")
                .Subject("Test")
                .Text("Body"));

        request.Requests[0].To = null!;
        request.Requests[0].Cc = null!;
        request.Requests[0].Bcc = null!;

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
    }

    [TestCase(500, 400, 101)]
    [TestCase(1000, 1, 0)]
    [TestCase(0, 1000, 1)]
    [TestCase(0, 1, 1000)]
    public void Validation_Requests_Should_Fail_WhenTotalRecipientsExceedsLimit(int toCount, int ccCount, int bccCount)
    {
        var request = BatchEmailRequest.Create()
                        .Requests(r => r
                            .To(Enumerable.Repeat(new EmailAddress(_validEmail), toCount).ToArray())
                            .Cc(Enumerable.Repeat(new EmailAddress(_validEmail), ccCount).ToArray())
                            .Bcc(Enumerable.Repeat(new EmailAddress(_validEmail), bccCount).ToArray()));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
    }

    [TestCase(500, 400, 100)]
    public void Validation_Requests_Should_Pass_WhenTotalRecipientsWithinLimit(int toCount, int ccCount, int bccCount)
    {
        var request = BatchEmailRequest.Create()
                        .Requests(r => r
                            .To(Enumerable.Repeat(new EmailAddress(_validEmail), toCount).ToArray())
                            .Cc(Enumerable.Repeat(new EmailAddress(_validEmail), ccCount).ToArray())
                            .Bcc(Enumerable.Repeat(new EmailAddress(_validEmail), bccCount).ToArray()));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].Recipients");
    }

    #endregion


    #region Request Attachments

    [Test]
    public void Validation_Requests_Should_Fail_WhenAtLeastOneAttachmentIsInvalid()
    {
        var request = BatchEmailRequest.Create();
        var internalRequest = SendEmailRequest.Create()
            .Attach("Any content", "file1.txt")
            .Attach("Any content", "file2.txt", mimeType: string.Empty);
        request.Requests.Add(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Attachments)}[1].{nameof(Attachment.MimeType)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenAllAttachmentsAreValid()
    {
        var internalRequest = SendEmailRequest.Create()
            .Attach("Any content", "file1.txt")
            .Attach("Any content", "file2.txt");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Attachments)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Attachments)}[0].{nameof(Attachment.MimeType)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Attachments)}[1].{nameof(Attachment.MimeType)}");
    }

    #endregion



    #region Request Template ID

    [Test]
    public void Validation_Requests_Should_Fail_WhenTemplateIdIsSetAndSubjectProvided()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Template(_templateId)
                                .Subject("Subject");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenTemplateIdIsSetAndTextProvided()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Template(_templateId)
                                .Text("Content");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenTemplateIdIsSetAndHtmlProvided()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Template(_templateId)
                                .Html("<h1>Header</h1>");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenTemplateIdIsSetAndNoForbiddenPropertiesAreSet()
    {
        var internalRequest = SendEmailRequest.Create()
                                .From(_validEmail)
                                .To(_validEmail)
                                .Template(_templateId);
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenTemplateIdIsSetAndBaseTextHtmlSubjectProvided()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Template(_templateId);
        var @base = EmailRequest.Create()
                        .Subject("Subject")
                        .Text("Text")
                        .Html("<h1>Header</h1>");
        var request = BatchEmailRequest.Create()
                        .Base(@base)
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
    }

    #endregion



    #region Request Subject

    [TestCase(null)]
    [TestCase("")]
    public void Validation_Requests_Should_Fail_WhenSubjectIsNotValid(string? subject)
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create());
        request.Requests[0].Subject = subject;

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
    }

    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase(null, "")]
    [TestCase("", null)]
    public void Validation_Requests_Should_Fail_WhenSubjectIsNotValidAndBaseSubjectIsNotValid(string? subject, string? baseSubject)
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create())
                        .Base(EmailRequest.Create());

        request.Requests[0].Subject = subject;
        request.Base.Subject = baseSubject;

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenSubjectIsSetAndBaseTemplateIdIsSet()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().Subject("Subject"))
                        .Base(EmailRequest.Create().Template(_templateId));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
    }

    [TestCase(null)]
    [TestCase("")]
    public void Validation_Requests_Should_Pass_WhenSubjectIsNotValidButBaseSubjectIsSet(string? subject)
    {
        var internalRequest = SendEmailRequest.Create();
        internalRequest.Subject = subject;
        var request = BatchEmailRequest.Create()
                        .Base(EmailRequest.Create().Subject("Base Subject"))
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.Subject);
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenSubjectProvided()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Subject("Subject");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Subject)}");
    }

    #endregion



    #region Request Category

    [Test]
    public void Validation_Requests_Should_Fail_WhenCategoryExceedsAllowedLength()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Category(TestContext.CurrentContext.Random.GetString(256));
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Category)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenCategoryFitsAllowedLength()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Category(TestContext.CurrentContext.Random.GetString(255));
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Category)}");
    }

    [TestCase(null)]
    [TestCase("")]
    public void Validation_Requests_Should_Fail_WhenCategoryNotSetButBaseCategoryExceedsAllowedLength(string? category)
    {
        var @base = EmailRequest.Create()
                        .Category(TestContext.CurrentContext.Random.GetString(256));
        var request = BatchEmailRequest.Create()
                        .Base(@base)
                        .Requests(SendEmailRequest.Create());
        request.Requests[0].Category = category;

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Base.Category);
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Category)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenCategoryNotSetButBaseCategoryFitsAllowedLength()
    {
        var @base = EmailRequest.Create()
                        .Category(TestContext.CurrentContext.Random.GetString(255));
        var request = BatchEmailRequest.Create()
                        .Base(@base)
                        .Requests(SendEmailRequest.Create());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Base.Category);
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.Category)}");
    }

    #endregion



    #region Request Body

    [Test]
    public void Validation_Requests_Should_Fail_WhenBothHtmlAndTextBodyAreNull()
    {
        var internalRequest = SendEmailRequest.Create();
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenBothHtmlAndTextBodyAreEmpty()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Text(string.Empty)
                                .Html(string.Empty);
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenTextIsSetAndBaseTemplateIdIsSet()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().Text("Text"))
                        .Base(EmailRequest.Create().Template(_templateId));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenHtmlIsSetAndBaseTemplateIdIsSet()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create().Html("<h1>Header</h1>"))
                        .Base(EmailRequest.Create().Template(_templateId));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Fail_WhenTextAndHtmlIsSetAndBaseTemplateIdIsSet()
    {
        var request = BatchEmailRequest.Create()
                        .Requests(SendEmailRequest.Create()
                                    .Html("<h1>Header</h1>")
                                    .Text("Text"))
                        .Base(EmailRequest.Create().Template(_templateId));

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
        result.ShouldHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenBothHtmlAndTextBodyAreNullButBaseTextIsNotEmpty()
    {
        var @base = EmailRequest.Create()
                        .Text("Base Text");
        var request = BatchEmailRequest.Create()
                        .Base(@base)
                        .Requests(SendEmailRequest.Create());

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenBothHtmlAndTextBodyAreEmptyAndBaseHtmlIsNotEmpty()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Text(string.Empty)
                                .Html(string.Empty);
        var @base = EmailRequest.Create()
                        .Html("<div>Base Html</div>");
        var request = BatchEmailRequest.Create()
                        .Base(@base)
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenTextBodyIsNotEmpty()
    {
        var internalRequest = SendEmailRequest.Create()
                                .Text("Text");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenHtmlBodyIsNotEmpty()
    {
        var internalRequest = SendEmailRequest.Create()
            .Html("<h1>Html</h1>");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    [Test]
    public void Validation_Requests_Should_Pass_WhenBothHtmlAndTextBodyAreNotEmpty()
    {
        var internalRequest = SendEmailRequest.Create()
            .Text("Text")
            .Html("<h1>Html</h1>");
        var request = BatchEmailRequest.Create()
                        .Requests(internalRequest);

        var result = BatchEmailRequestValidator.Instance.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.TextBody)}");
        result.ShouldNotHaveValidationErrorFor($"{nameof(BatchEmailRequest.Requests)}[0].{nameof(SendEmailRequest.HtmlBody)}");
    }

    #endregion

    #region Helpers


    private static List<SendEmailRequest> CreateValidRequestList()
    {
        return new List<SendEmailRequest>()
            { CreateValidRequest(), CreateValidRequest(), CreateValidRequest() };
    }

    private static EmailRequest CreateValidBaseRequest()
    {
        return EmailRequest.Create();
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
