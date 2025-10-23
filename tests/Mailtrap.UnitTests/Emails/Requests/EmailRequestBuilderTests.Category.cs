namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_Category
{
    private string _category { get; } = "Category";


    [Test]
    public void Category_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Category<EmailRequest>(null!, _category);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Category_Should_NotThrowException_WhenCategoryIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Category(null!);

        act.Should().NotThrow();
    }

    [Test]
    public void Category_Should_NotThrowException_WhenCategoryIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Category(string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void Category_Should_AssignCategoryProperly()
    {
        var request = EmailRequest
            .Create()
            .Category(_category);

        request.Category.Should().BeSameAs(_category);
    }

    [Test]
    public void Category_Should_OverrideCategory_WhenCalledSeveralTimes()
    {
        var otherCategory = "Updated Category";

        var request = EmailRequest
            .Create()
            .Category(_category)
            .Category(otherCategory);

        request.Category.Should().BeSameAs(otherCategory);
    }
}
