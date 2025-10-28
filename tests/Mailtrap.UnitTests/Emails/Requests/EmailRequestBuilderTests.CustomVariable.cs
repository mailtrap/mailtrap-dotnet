using Variable = System.Collections.Generic.KeyValuePair<string, string>;


namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_CustomVariable
{

    private string VariableKey { get; } = "key";
    private string VariableValue { get; } = "value";
    private Variable _variable1 { get; } = new("Key-1", "Value 1");
    private Variable _variable2 { get; } = new("Key-2", "Value 2");



    #region CustomVariable

    [Test]
    public void CustomVariable_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.CustomVariable<EmailRequest>(null!, _variable1);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void CustomVariable_Should_ThrowArgumentNullException_WhenParamsIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.CustomVariable(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void CustomVariable_Should_NotThrowException_WhenParamsIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.CustomVariable([]);

        act.Should().NotThrow();
    }

    [Test]
    public void CustomVariable_Should_AddVariablesToCollection()
    {
        CustomVariable_CreateAndValidate(_variable1, _variable2);
    }

    [Test]
    public void CustomVariable_Should_AddVariablesToCollection_WhenCalledMultipleTimes()
    {
        var variable3 = new Variable("key-3", "Value 3");
        var variable4 = new Variable("key-4", "Value 4");

        var request = CustomVariable_CreateAndValidate(_variable1, _variable2);

        request.CustomVariable(variable3, variable4);

        request.CustomVariables.Should()
            .HaveCount(4).And
            .ContainInOrder(_variable1, _variable2, variable3, variable4);
    }

    [Test]
    public void CustomVariable_Should_OverrideVariables_WhenCalledMultipleTimesWithTheSameKeys()
    {
        var variable3 = new Variable("key-3", "Value 3");

        var request = CustomVariable_CreateAndValidate(_variable1, _variable2);

        request.CustomVariable(_variable1, variable3);

        request.CustomVariables.Should()
            .HaveCount(3).And
            .ContainInOrder(_variable1, _variable2, variable3);
    }

    [Test]
    public void CustomVariable_Should_NotAddVariablesToCollection_WhenParamsIsEmpty()
    {
        var request = CustomVariable_CreateAndValidate(_variable1, _variable2);

        request.CustomVariable([]);

        request.CustomVariables.Should()
            .HaveCount(2).And
            .ContainInOrder(_variable1, _variable2);
    }


    private static EmailRequest CustomVariable_CreateAndValidate(params Variable[] headers)
    {
        var request = EmailRequest
            .Create()
            .CustomVariable(headers);

        request.CustomVariables.Should()
            .HaveCount(headers.Length).And
            .ContainInOrder(headers);

        return request;
    }

    #endregion



    #region CustomVariable(key, value)

    [Test]
    public void CustomVariable_Should_ThrowArgumentNullException_WhenRequestIsNull_WithStrings()
    {
        var request = EmailRequest.Create();

        var act = () => EmailRequestBuilder.CustomVariable<EmailRequest>(null!, VariableKey, VariableValue);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void CustomVariable_Should_ThrowArgumentNullException_WhenVariableKeyIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.CustomVariable(null!, VariableValue);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void CustomVariable_Should_ThrowArgumentNullException_WhenVariableKeyIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.CustomVariable(string.Empty, VariableValue);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void CustomVariable_Should_NotThrowException_WhenVariableValueIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.CustomVariable(VariableKey, null!);

        act.Should().NotThrow();
    }

    [Test]
    public void CustomVariable_Should_NotThrowException_WhenVariableValueIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.CustomVariable(VariableKey, string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void CustomVariable_Should_AddVariableToCollection_2()
    {
        CustomVariable_CreateAndValidate(VariableKey, VariableValue);
    }

    [Test]
    public void CustomVariable_Should_AddVariablesToCollection_WhenCalledMultipleTimes_2()
    {
        var key = "key-2";
        var value = "Value 2";

        var request = CustomVariable_CreateAndValidate(VariableKey, VariableValue);

        request.CustomVariable(key, value);

        request.CustomVariables.Should().HaveCount(2);

        request.CustomVariables.Should().ContainKeys(VariableKey, key);

        request.CustomVariables[VariableKey].Should().Be(VariableValue);

        request.CustomVariables[key].Should().Be(value);
    }

    [Test]
    public void CustomVariable_Should_OverrideVariable_WhenCalledMultipleTimesWithTheSameKey_2()
    {
        var otherValue = "Other Value";

        var request = CustomVariable_CreateAndValidate(VariableKey, VariableValue);

        request.CustomVariable(VariableKey, otherValue);

        request.CustomVariables.Should().ContainSingle();

        request.CustomVariables.Should().ContainKey(VariableKey);

        request.CustomVariables[VariableKey].Should().Be(otherValue);
    }


    private static EmailRequest CustomVariable_CreateAndValidate(string key, string value)
    {
        var request = EmailRequest
            .Create()
            .CustomVariable(key, value);

        request.CustomVariables.Should().ContainSingle();

        request.CustomVariables.Should().ContainKey(key);

        request.CustomVariables[key].Should().Be(value);

        return request;
    }

    #endregion
}
