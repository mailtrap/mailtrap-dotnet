namespace Mailtrap.UnitTests.EmailCampaigns;


[TestFixture]
internal sealed class EmailCampaignRequestValidatorTests
{
    private static readonly CreateEmailCampaignRequestValidator s_createValidator = CreateEmailCampaignRequestValidator.Instance;
    private static readonly UpdateEmailCampaignRequestValidator s_updateValidator = UpdateEmailCampaignRequestValidator.Instance;


    #region Create

    [Test]
    public void Create_WithNameAndDomain_ShouldPass()
    {
        var request = new CreateEmailCampaignRequest { Name = "Spring Sale", MailsendDomainId = 123 };

        var result = s_createValidator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Create_WithEmptyName_ShouldFail()
    {
        var request = new CreateEmailCampaignRequest { Name = string.Empty, MailsendDomainId = 123 };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Test]
    public void Create_WithNonPositiveDomainId_ShouldFail()
    {
        var request = new CreateEmailCampaignRequest { Name = "Spring Sale", MailsendDomainId = 0 };

        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.MailsendDomainId);
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
    public void Update_ScheduledWithoutScheduledFor_ShouldFail()
    {
        var request = new UpdateEmailCampaignRequest { DeliveryMode = DeliveryMode.Scheduled };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.ScheduledFor);
    }

    [Test]
    public void Update_ScheduledWithScheduledFor_ShouldPass()
    {
        var request = new UpdateEmailCampaignRequest
        {
            DeliveryMode = DeliveryMode.Scheduled,
            ScheduledFor = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero)
        };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Update_WithNonPositiveDomainId_ShouldFail()
    {
        var request = new UpdateEmailCampaignRequest { MailsendDomainId = 0 };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.MailsendDomainId);
    }

    [Test]
    public void Update_ImmediateDeliveryMode_DoesNotRequireScheduledFor()
    {
        var request = new UpdateEmailCampaignRequest { DeliveryMode = DeliveryMode.Immediate };

        var result = s_updateValidator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}
