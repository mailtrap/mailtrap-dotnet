namespace Mailtrap.UnitTests.EmailCampaigns;


[TestFixture]
internal sealed class EmailCampaignRequestValidatorTests
{
    private const long SendingDomainId = 4321;

    private static readonly CreateEmailCampaignRequestValidator s_createValidator = CreateEmailCampaignRequestValidator.Instance;
    private static readonly UpdateEmailCampaignRequestValidator s_updateValidator = UpdateEmailCampaignRequestValidator.Instance;
    private static readonly ScheduleEmailCampaignRequestValidator s_scheduleValidator = ScheduleEmailCampaignRequestValidator.Instance;


    #region Create

    [Test]
    public void Create_WithRequiredFields_ShouldPass()
    {
        var result = s_createValidator.TestValidate(ValidCreateRequest());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Create_WithEmptyName_ShouldFail()
    {
        var request = ValidCreateRequest() with { Name = string.Empty };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Test]
    public void Create_WithMissingDomainId_ShouldFail()
    {
        var request = ValidCreateRequest() with { DomainId = null };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.DomainId);
    }

    [Test]
    public void Create_WithNonPositiveDomainId_ShouldFail()
    {
        var request = ValidCreateRequest() with { DomainId = 0 };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.DomainId);
    }

    [Test]
    public void Create_WithEmptyFromLocalPart_ShouldFail()
    {
        var request = ValidCreateRequest() with { FromLocalPart = string.Empty };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.FromLocalPart);
    }

    [Test]
    public void Create_WithoutTemplateAttributes_ShouldFail()
    {
        var request = ValidCreateRequest() with { TemplateAttributes = null };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.TemplateAttributes);
    }

    [Test]
    public void Create_WithoutTemplateSubject_ShouldFail()
    {
        var request = ValidCreateRequest() with
        {
            TemplateAttributes = new EmailCampaignTemplateAttributes { BodyHtml = "<html></html>" }
        };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.TemplateAttributes!.Subject);
    }

    #endregion


    #region Update

    [Test]
    public void Update_Empty_ShouldPass()
    {
        var request = new UpdateEmailCampaignRequest();

        var result = s_updateValidator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Update_WithDomainId_ShouldPass()
    {
        var request = new UpdateEmailCampaignRequest { DomainId = SendingDomainId };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Update_WithZeroDomainId_ShouldFail()
    {
        var request = new UpdateEmailCampaignRequest { DomainId = 0 };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.DomainId);
    }

    [Test]
    public void Update_WithNegativeDomainId_ShouldFail()
    {
        var request = new UpdateEmailCampaignRequest { DomainId = -1 };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.DomainId);
    }

    #endregion


    #region Schedule

    [Test]
    public void Schedule_WithNearFutureDatetime_ShouldPass()
    {
        var request = new ScheduleEmailCampaignRequest(DateTimeOffset.UtcNow.AddDays(7));

        var result = s_scheduleValidator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Schedule_WithPastDatetime_ShouldFail()
    {
        var request = new ScheduleEmailCampaignRequest(DateTimeOffset.UtcNow.AddDays(-1));

        var result = s_scheduleValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Datetime);
    }

    [Test]
    public void Schedule_WithDatetimeMoreThanOneMonthAhead_ShouldFail()
    {
        var request = new ScheduleEmailCampaignRequest(DateTimeOffset.UtcNow.AddMonths(2));

        var result = s_scheduleValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Datetime);
    }

    #endregion


    private static CreateEmailCampaignRequest ValidCreateRequest() => new()
    {
        Name = "Spring Sale",
        DomainId = SendingDomainId,
        FromLocalPart = "news",
        TemplateAttributes = new EmailCampaignTemplateAttributes { Subject = "Spring is here" }
    };
}
