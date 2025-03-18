﻿using ValidationResult = Mailtrap.Core.Validation.ValidationResult;


namespace Mailtrap.UnitTests.Core.Validation;


[TestFixture]
internal sealed class ValidationResultTests
{
    private const string ParamName = "request";


    [Test]
    public void EnsureValidity_ShouldThrowArgumentException_WhenResultIsInvalid()
    {
        var result = new ValidationResult(["Error A", "Error B"]);

        var act = () => result.EnsureValidity(ParamName);

        act.Should().Throw<ArgumentException>().WithParameterName(ParamName);
    }

    [Test]
    public void EnsureValidity_ShouldNotThrowException_WhenResultInvalid()
    {
        var result = new ValidationResult();

        var act = () => result.EnsureValidity(ParamName);

        act.Should().NotThrow();
    }
}
