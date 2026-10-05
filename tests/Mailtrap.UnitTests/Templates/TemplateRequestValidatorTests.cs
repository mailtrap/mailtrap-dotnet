namespace Mailtrap.UnitTests.Templates;


[TestFixture]
internal sealed class TemplateRequestValidatorTests
{
    private const int MaxShort = 255;
    private const int MaxBody = 10_000_000;

    private static readonly CreateTemplateRequestValidator s_createValidator = CreateTemplateRequestValidator.Instance;
    private static readonly UpdateTemplateRequestValidator s_updateValidator = UpdateTemplateRequestValidator.Instance;


    #region Create

    [Test]
    public void Create_WithRequiredFields_ShouldPass()
    {
        s_createValidator.TestValidate(ValidCreateRequest()).ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Create_WithMaxLengths_ShouldPass()
    {
        var request = new CreateTemplateRequest
        {
            Name = new string('a', MaxShort),
            Category = new string('a', MaxShort),
            Subject = new string('a', MaxShort),
            BodyHtml = new string('a', MaxBody),
            BodyText = new string('a', MaxBody)
        };

        s_createValidator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Create_WithMissingRequiredFields_ShouldFail([Values] bool nullValue)
    {
        var value = nullValue ? null : string.Empty;

        var request = ValidCreateRequest() with { Name = value, Category = value, Subject = value };
        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
        result.ShouldHaveValidationErrorFor(r => r.Category);
        result.ShouldHaveValidationErrorFor(r => r.Subject);
    }

    [Test]
    public void Create_WithTooLongShortFields_ShouldFail()
    {
        var tooLong = new string('a', MaxShort + 1);

        var request = ValidCreateRequest() with { Name = tooLong, Category = tooLong, Subject = tooLong };
        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
        result.ShouldHaveValidationErrorFor(r => r.Category);
        result.ShouldHaveValidationErrorFor(r => r.Subject);
    }

    [Test]
    public void Create_WithTooLongBodies_ShouldFail()
    {
        var tooLong = new string('a', MaxBody + 1);

        var request = ValidCreateRequest() with { BodyHtml = tooLong, BodyText = tooLong };
        var result = s_createValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.BodyHtml);
        result.ShouldHaveValidationErrorFor(r => r.BodyText);
    }

    #endregion


    #region Update

    [Test]
    public void Update_WithNoFields_ShouldPass()
    {
        s_updateValidator.TestValidate(new UpdateTemplateRequest()).ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Update_WithEmptyShortFields_ShouldFail()
    {
        var request = new UpdateTemplateRequest { Name = string.Empty, Category = string.Empty, Subject = string.Empty };
        var result = s_updateValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
        result.ShouldHaveValidationErrorFor(r => r.Category);
        result.ShouldHaveValidationErrorFor(r => r.Subject);
    }

    [Test]
    public void Update_WithTooLongFields_ShouldFail()
    {
        var tooLong = new string('a', MaxShort + 1);
        var tooLongBody = new string('a', MaxBody + 1);

        var request = new UpdateTemplateRequest
        {
            Name = tooLong,
            Category = tooLong,
            Subject = tooLong,
            BodyHtml = tooLongBody,
            BodyText = tooLongBody
        };
        var result = s_updateValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
        result.ShouldHaveValidationErrorFor(r => r.Category);
        result.ShouldHaveValidationErrorFor(r => r.Subject);
        result.ShouldHaveValidationErrorFor(r => r.BodyHtml);
        result.ShouldHaveValidationErrorFor(r => r.BodyText);
    }

    [Test]
    public void Update_WithValidSubset_ShouldPass()
    {
        var request = new UpdateTemplateRequest { Name = "Renamed", BodyHtml = "<p>Hi</p>" };

        s_updateValidator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    #endregion


    private static CreateTemplateRequest ValidCreateRequest() => new()
    {
        Name = "Welcome",
        Category = "Onboarding",
        Subject = "Hello"
    };
}
