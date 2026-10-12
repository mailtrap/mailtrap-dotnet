namespace Mailtrap.UnitTests.Inbound;


[TestFixture]
internal sealed class InboundForwardRuleRequestValidatorTests
{
    private static InboundForwardRuleCondition ValidCondition() => new()
    {
        MatchType = ForwardRuleMatchType.Sender,
        Operator = ForwardRuleOperator.EndsWith,
        Value = "@billing.example.com"
    };

    private static InboundForwardRuleDestination ValidDestination() => new() { Email = "finance@example.com" };


    [Test]
    public void Create_ShouldBeValid_WhenNameAndOptionalSetsAreProvided()
    {
        var request = new CreateInboundForwardRuleRequest
        {
            Name = "Copy billing mail to finance",
            Conditions = [ValidCondition()],
            Destinations = [ValidDestination()]
        };

        request.Validate().IsValid.Should().BeTrue();
    }

    [Test]
    public void Create_ShouldBeValid_WhenOnlyNameIsProvided()
    {
        var request = new CreateInboundForwardRuleRequest { Name = "Match everything" };

        request.Validate().IsValid.Should().BeTrue();
    }

    [Test]
    public void Create_ShouldBeInvalid_WhenNameIsMissing([Values(null, "")] string? name)
    {
        var request = new CreateInboundForwardRuleRequest { Name = name };

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Create_ShouldBeInvalid_WhenConditionHasNoOperator()
    {
        var request = new CreateInboundForwardRuleRequest
        {
            Name = "Rule",
            Conditions = [new InboundForwardRuleCondition { MatchType = ForwardRuleMatchType.Sender, Value = "x" }]
        };

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Create_ShouldBeInvalid_WhenConditionHasNoMatchType()
    {
        var request = new CreateInboundForwardRuleRequest
        {
            Name = "Rule",
            Conditions = [new InboundForwardRuleCondition { Operator = ForwardRuleOperator.Contains, Value = "x" }]
        };

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Update_ShouldBeValid_WhenEmpty()
    {
        var request = new UpdateInboundForwardRuleRequest();

        request.Validate().IsValid.Should().BeTrue();
    }

    [Test]
    public void Update_ShouldBeValid_WhenSetsAreCleared()
    {
        var request = new UpdateInboundForwardRuleRequest { Conditions = [], Destinations = [] };

        request.Validate().IsValid.Should().BeTrue();
    }

    [Test]
    public void Update_ShouldBeInvalid_WhenNameIsEmpty()
    {
        var request = new UpdateInboundForwardRuleRequest { Name = string.Empty };

        request.Validate().IsValid.Should().BeFalse();
    }

    [Test]
    public void Update_ShouldBeInvalid_WhenConditionHasNoOperator()
    {
        var request = new UpdateInboundForwardRuleRequest
        {
            Conditions = [new InboundForwardRuleCondition { MatchType = ForwardRuleMatchType.Header, HeaderKey = "X-Priority" }]
        };

        request.Validate().IsValid.Should().BeFalse();
    }
}
