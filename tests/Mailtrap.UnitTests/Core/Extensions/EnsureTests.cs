namespace Mailtrap.UnitTests.Core.Extensions;


[TestFixture]
internal sealed class EnsureTests
{
    [Test]
    public void NotNull_ShouldThrowArgumentNullException_WhenParamValueIsNull()
    {
        object? paramValue = null;
        var message = "Message";

        var act = () => Ensure.NotNull(paramValue, nameof(paramValue), message);

        act.Should().Throw<ArgumentNullException>().WithMessage(message + "*");
    }

    [Test]
    public void NotNull_ShouldNotThrowException_WhenParamValueIsNotNull()
    {
        var paramValue = new object();

        var act = () => Ensure.NotNull(paramValue, nameof(paramValue));

        act.Should().NotThrow();
    }

    [Test]
    public void NotNullOrEmpty_ShouldThrowArgumentNullException_WhenParamValueIsNull()
    {
        string? paramValue = null;
        var message = "Message";

        var act = () => Ensure.NotNullOrEmpty(paramValue, nameof(paramValue), message);

        act.Should().Throw<ArgumentNullException>().WithMessage(message + "*");
    }

    [Test]
    public void NotNullOrEmpty_ShouldThrowArgumentNullException_WhenParamValueIsEmpty()
    {
        var paramValue = string.Empty;

        var act = () => Ensure.NotNullOrEmpty(paramValue, nameof(paramValue));

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void NotNullOrEmpty_ShouldNotThrowException_WhenParamValueIsNotNullOrEmpty()
    {
        var paramValue = "paramValue";

        var act = () => Ensure.NotNullOrEmpty(paramValue, nameof(paramValue));

        act.Should().NotThrow();
    }

    [Test]
    public void GreaterThanZero_ShouldThrowArgumentOutOfRangeException_WhenParamValueIsZero()
    {
        var paramValue = 0L;
        var message = "Message";

        var act = () => Ensure.GreaterThanZero(paramValue, nameof(paramValue), message);

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(message + "*");
    }

    [Test]
    public void GreaterThanZero_ShouldThrowArgumentOutOfRangeException_WhenParamValueIsLessThanZero()
    {
        var paramValue = -1L;

        var act = () => Ensure.GreaterThanZero(paramValue, nameof(paramValue));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GreaterThanZero_ShouldNotThrowException_WhenParamValueIsGreaterThanZero()
    {
        var paramValue = 1L;

        var act = () => Ensure.GreaterThanZero(paramValue, nameof(paramValue));

        act.Should().NotThrow();
    }
}
