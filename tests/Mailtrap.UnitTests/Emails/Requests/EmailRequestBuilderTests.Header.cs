using Header = System.Collections.Generic.KeyValuePair<string, string>;


namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_Header
{

    private string HeaderKey { get; } = "key";
    private string HeaderValue { get; } = "value";
    private Header _header1 { get; } = new("Key-1", "Value 1");
    private Header _header2 { get; } = new("Key-2", "Value 2");



    #region Header

    [Test]
    public void Header_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Header<EmailRequest>(null!, _header1);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Header_Should_ThrowArgumentNullException_WhenParamsIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Header(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Header_Should_NotThrowException_WhenParamsIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Header([]);

        act.Should().NotThrow();
    }

    [Test]
    public void Header_Should_AddHeadersToCollection()
    {
        Header_CreateAndValidate(_header1, _header2);
    }

    [Test]
    public void Header_Should_AddHeadersToCollection_WhenCalledMultipleTimes()
    {
        var header3 = new Header("key-3", "Value 3");
        var header4 = new Header("key-4", "Value 4");

        var request = Header_CreateAndValidate(_header1, _header2);

        request.Header(header3, header4);

        request.Headers.Should()
            .HaveCount(4).And
            .ContainInOrder(_header1, _header2, header3, header4);
    }

    [Test]
    public void Header_Should_OverrideHeaders_WhenCalledMultipleTimesWithTheSameKeys()
    {
        var header3 = new Header("key-3", "Value 3");

        var request = Header_CreateAndValidate(_header1, _header2);

        request.Header(_header1, header3);

        request.Headers.Should()
            .HaveCount(3).And
            .ContainInOrder(_header1, _header2, header3);
    }

    [Test]
    public void Header_Should_NotAddHeadersToCollection_WhenParamsIsEmpty()
    {
        var request = Header_CreateAndValidate(_header1, _header2);

        request.Header([]);

        request.Headers.Should()
            .HaveCount(2).And
            .ContainInOrder(_header1, _header2);
    }


    private static EmailRequest Header_CreateAndValidate(params Header[] headers)
    {
        var request = EmailRequest
            .Create()
            .Header(headers);

        request.Headers.Should()
            .HaveCount(headers.Length).And
            .ContainInOrder(headers);

        return request;
    }

    #endregion



    #region Header(key, value)

    [Test]
    public void Header_Should_ThrowArgumentNullException_WhenRequestIsNull_WithStrings()
    {
        var request = EmailRequest.Create();

        var act = () => EmailRequestBuilder.Header<EmailRequest>(null!, HeaderKey, HeaderValue);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Header_Should_ThrowArgumentNullException_WhenHeaderKeyIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Header(null!, HeaderValue);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Header_Should_ThrowArgumentNullException_WhenHeaderKeyIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Header(string.Empty, HeaderValue);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Header_Should_NotThrowException_WhenHeaderValueIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Header(HeaderKey, null!);

        act.Should().NotThrow();
    }

    [Test]
    public void Header_Should_NotThrowException_WhenHeaderValueIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Header(HeaderKey, string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void Header_Should_AddHeaderToCollection_2()
    {
        Header_CreateAndValidate(HeaderKey, HeaderValue);
    }

    [Test]
    public void Header_Should_AddHeadersToCollection_WhenCalledMultipleTimes_2()
    {
        var key = "key-2";
        var value = "Value 2";

        var request = Header_CreateAndValidate(HeaderKey, HeaderValue);

        request.Header(key, value);

        request.Headers.Should().HaveCount(2);

        request.Headers.Should().ContainKeys(HeaderKey, key);

        request.Headers[HeaderKey].Should().Be(HeaderValue);

        request.Headers[key].Should().Be(value);
    }

    [Test]
    public void Header_Should_OverrideHeader_WhenCalledMultipleTimesWithTheSameKey_2()
    {
        var otherValue = "Other Value";

        var request = Header_CreateAndValidate(HeaderKey, HeaderValue);

        request.Header(HeaderKey, otherValue);

        request.Headers.Should().ContainSingle();

        request.Headers.Should().ContainKey(HeaderKey);

        request.Headers[HeaderKey].Should().Be(otherValue);
    }


    private static EmailRequest Header_CreateAndValidate(string key, string value)
    {
        var request = EmailRequest
            .Create()
            .Header(key, value);

        request.Headers.Should().ContainSingle();

        request.Headers.Should().ContainKey(key);

        request.Headers[key].Should().Be(value);

        return request;
    }

    #endregion
}
