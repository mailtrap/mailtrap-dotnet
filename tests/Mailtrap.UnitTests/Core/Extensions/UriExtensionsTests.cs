﻿// -----------------------------------------------------------------------
// <copyright file="UriExtensionsTests.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.UnitTests.Core.Extensions;


[TestFixture]
internal sealed class UriExtensionsTests
{
    private string _absoluteUrl { get; } = "https://example.com";
    private string _relativeUrl { get; } = "api/send";
    private Uri _absoluteUri { get; } = new Uri("https://example.com");
    private Uri _relativeUri { get; } = new Uri("api", UriKind.Relative);



    #region ToRelativeUri

    [Test]
    public void ToRelativeUri_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => UriExtensions.ToRelativeUri(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ToRelativeUri_ShouldThrowArgumentNullException_WhenUriIsEmpty()
    {
        var act = string.Empty.ToRelativeUri;

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ToRelativeUri_ShouldThrowArgumentException_WhenUriIsAbsolute()
    {
        var act = _absoluteUrl.ToRelativeUri;

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ToRelativeUri_ShouldReturnUri_WhenUriIsRelative()
    {
        var uri = _relativeUrl.ToRelativeUri();

        uri.IsAbsoluteUri.Should().BeFalse();
        uri.ToString().Should().Be(_relativeUrl);
    }

    #endregion


    #region ToAbsoluteUri

    [Test]
    public void ToAbsoluteUri_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => UriExtensions.ToAbsoluteUri(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ToAbsoluteUri_ShouldThrowArgumentNullException_WhenUriIsEmpty()
    {
        var act = string.Empty.ToAbsoluteUri;

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ToAbsoluteUri_ShouldThrowArgumentException_WhenUriIsRelative()
    {
        var act = _relativeUrl.ToAbsoluteUri;

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ToAbsoluteUri_ShouldReturnUri_WhenUriIsAbsolute()
    {
        var uri = _absoluteUrl.ToAbsoluteUri();

        uri.IsAbsoluteUri.Should().BeTrue();
        uri.ToString().Should().Be(_absoluteUrl + "/");
    }

    #endregion



    #region ToUri

    [Test]
    public void ToUri_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => UriExtensions.ToUri(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ToUri_ShouldThrowArgumentNullException_WhenUriIsEmpty()
    {
        var act = string.Empty.ToUri;

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ToUri_ShouldThrowArgumentException_WhenUriIsInvalid()
    {
        var act = "https:///file://1.html".ToUri;

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ToUri_ShouldNotThrowException_WhenUriIsRelative()
    {
        var act = _relativeUrl.ToUri;

        act.Should().NotThrow();
    }

    [Test]
    public void ToUri_ShouldNotThrowException_WhenUriIsAbsolute()
    {
        var act = _absoluteUrl.ToUri;

        act.Should().NotThrow();
    }

    [Test]
    public void ToUri_ShouldReturnUri_WhenUriIsRelative()
    {
        var uri = _relativeUrl.ToUri();

        uri.IsAbsoluteUri.Should().BeFalse();
        uri.ToString().Should().Be(_relativeUrl);
    }

    [Test]
    public void ToUri_ShouldReturnUri_WhenUriIsAbsolute()
    {
        var uri = _absoluteUrl.ToUri();

        uri.IsAbsoluteUri.Should().BeTrue();
        uri.ToString().Should().Be(_absoluteUrl + "/");
    }

    #endregion


    #region Append

    [Test]
    public void Append_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => UriExtensions.Append(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Append_ShouldThrowArgumentNullException_WhenSegmentsIsNull()
    {
        var act = () => _absoluteUri.Append(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Append_ShouldReturnSameUri_WhenSegmentsAreEmpty()
    {
        var result = _absoluteUri.Append();

        result.Should().Be(_absoluteUri);
    }

    [Test]
    public void Append_ShouldAppendSegmentsToUri()
    {
        var segment1 = "api";
        var segment2 = "send";

        var result = _absoluteUri.Append(segment1, segment2);

        result.AbsoluteUri.Should().Be(_absoluteUri.AbsoluteUri + segment1 + "/" + segment2);
    }

    [Test]
    public void Append_ShouldAppendSegmentsProperly_WhenTrailingSlashIsMissingInTheOriginalUrl()
    {
        var segment1 = "api";
        var segment2 = "send";

        var result = _absoluteUri
            .Append(segment1) // trailing slash is missing here
            .Append(segment2);

        result.AbsoluteUri.Should().Be(_absoluteUri.AbsoluteUri + segment1 + "/" + segment2);
    }

    #endregion


    #region AppendQueryParameter

    [Test]
    public void AppendQueryParameter_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => UriExtensions.AppendQueryParameter(null!, "key", "value");

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AppendQueryParameter_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        var act = () => _absoluteUri.AppendQueryParameter(null!, "value");

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AppendQueryParameter_ShouldThrowArgumentNullException_WhenKeyIsEmpty()
    {
        var act = () => _absoluteUri.AppendQueryParameter(string.Empty, "value");

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AppendQueryParameter_ShouldThrowArgumentNullException_WhenValueIsNull()
    {
        var act = () => _absoluteUri.AppendQueryParameter("key", null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AppendQueryParameter_ShouldThrowArgumentNullException_WhenValueIsEmpty()
    {
        var act = () => _absoluteUri.AppendQueryParameter("key", string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AppendQueryParameter_ShouldAppendQueryParamToUri()
    {
        var key = "api";
        var value = "send";

        var result = _absoluteUri.AppendQueryParameter(key, value);

        result.AbsoluteUri.Should().Be($"{_absoluteUri.AbsoluteUri}?{key}={value}");
    }

    [Test]
    public void AppendQueryParameter_ShouldAppendQueryParamCorrectly_WhenCalledMultipleTimes()
    {
        var key1 = "api";
        var value1 = "send";

        var key2 = "bulk";
        var value2 = "true";

        var result = _absoluteUri
            .AppendQueryParameter(key1, value1)
            .AppendQueryParameter(key2, value2);

        result.AbsoluteUri.Should().Be($"{_absoluteUri.AbsoluteUri}?{key1}={value1}&{key2}={value2}");
    }

    #endregion



    #region EnsureAbsoluteUri

    [Test]
    public void EnsureAbsoluteUri_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        var act = () => UriExtensions.EnsureAbsoluteUri(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void EnsureAbsoluteUri_ShouldThrowArgumentException_WhenUriIsNotAbsoluteUri()
    {
        var act = _relativeUri.EnsureAbsoluteUri;

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void EnsureAbsoluteUri_ShouldNotThrowException_WhenUriIsAbsoluteUri()
    {
        var act = _absoluteUri.EnsureAbsoluteUri;

        act.Should().NotThrow();
    }

    #endregion
}
