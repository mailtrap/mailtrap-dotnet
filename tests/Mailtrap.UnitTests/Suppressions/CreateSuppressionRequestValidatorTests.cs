namespace Mailtrap.UnitTests.Suppressions;


[TestFixture]
internal sealed class CreateSuppressionRequestValidatorTests
{
    private const long SendingDomainId = 4321;

    private static readonly CreateSuppressionRequestValidator s_validator = CreateSuppressionRequestValidator.Instance;


    [Test]
    public void WithRequiredFields_ShouldPass()
    {
        var result = s_validator.TestValidate(ValidRequest());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WithEmptyEmail_ShouldFail()
    {
        var request = ValidRequest() with { Email = string.Empty };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Email);
    }

    [Test]
    public void WithNonPositiveDomainId_ShouldFail()
    {
        var request = ValidRequest() with { DomainId = 0 };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.DomainId);
    }

    [Test]
    public void WithUnknownSendingStream_ShouldFail()
    {
        var request = ValidRequest() with { SendingStream = SendingStream.Unknown };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.SendingStream);
    }

    [Test]
    public void WithoutType_ShouldPass()
    {
        var request = ValidRequest() with { Type = null };

        var result = s_validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }


    private static CreateSuppressionRequest ValidRequest() => new()
    {
        Email = "test@demomailtrap.co",
        DomainId = SendingDomainId,
        SendingStream = SendingStream.Transactional,
        Type = SuppressionType.ManualImport
    };
}
