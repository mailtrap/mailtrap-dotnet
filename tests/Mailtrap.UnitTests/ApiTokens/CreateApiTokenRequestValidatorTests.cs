namespace Mailtrap.UnitTests.ApiTokens;


[TestFixture]
internal sealed class CreateApiTokenRequestValidatorTests
{
    private static readonly CreateApiTokenRequestValidator s_validator = CreateApiTokenRequestValidator.Instance;


    [Test]
    public void Validate_WithValidName_ShouldPass()
    {
        var request = new CreateApiTokenRequest { Name = "My API Token" };

        var result = s_validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WithEmptyName_ShouldFail()
    {
        var request = new CreateApiTokenRequest { Name = string.Empty };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Test]
    public void Validate_WithNameLongerThan255_ShouldFail()
    {
        var request = new CreateApiTokenRequest { Name = new string('a', 256) };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Test]
    public void Validate_WithValidResource_ShouldPass()
    {
        var request = new CreateApiTokenRequest { Name = "Token" };
        request.Resources.Add(new ApiTokenAccessRequest(ResourceType.Account, 3229, AccessLevel.Admin));

        var result = s_validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
