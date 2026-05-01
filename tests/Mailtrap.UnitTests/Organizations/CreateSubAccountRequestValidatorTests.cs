namespace Mailtrap.UnitTests.Organizations;


[TestFixture]
internal sealed class CreateSubAccountRequestValidatorTests
{
    private static readonly CreateSubAccountRequestValidator s_validator = CreateSubAccountRequestValidator.Instance;


    [Test]
    public void Validate_WithValidName_ShouldPass()
    {
        var request = new CreateSubAccountRequest { Account = new SubAccountAttributes { Name = "Team A" } };

        var result = s_validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WithEmptyName_ShouldFail()
    {
        var request = new CreateSubAccountRequest { Account = new SubAccountAttributes { Name = string.Empty } };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Account.Name);
    }

    [Test]
    public void Validate_WithNameLongerThan255_ShouldFail()
    {
        var request = new CreateSubAccountRequest { Account = new SubAccountAttributes { Name = new string('a', 256) } };

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Account.Name);
    }
}
