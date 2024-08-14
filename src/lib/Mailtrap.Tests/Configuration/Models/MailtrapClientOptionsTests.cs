﻿// -----------------------------------------------------------------------
// <copyright file="MailtrapClientOptionsTests.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.Tests.Configuration.Models;


[TestFixture]
internal sealed class MailtrapClientOptionsTests
{
    [Test]
    public void Default_ShouldReturnValidDefaults()
    {
        var options = MailtrapClientOptions.Default;

        options.Serialization.Should().Be(MailtrapClientSerializationOptions.Default);
        options.Authentication.Should().Be(MailtrapClientAuthenticationOptions.Empty);
        options.SendEndpoint.Should().Be(MailtrapClientEndpointOptions.SendDefault);
        options.BulkEndpoint.Should().Be(MailtrapClientEndpointOptions.BulkDefault);
        options.TestEndpoint.Should().Be(MailtrapClientEndpointOptions.TestDefault);
    }

    [Test]
    public void Default_ShouldReturnNewObjectEveryTime_WhenCalled()
    {
        var options1 = MailtrapClientOptions.Default;
        var options2 = MailtrapClientOptions.Default;

        options1.Should().NotBeSameAs(options2);
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenApiTokenIsNull()
    {
        var act = () => new MailtrapClientOptions(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenApiTokenIsEmpty()
    {
        var act = () => new MailtrapClientOptions(string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldAssignApiTokenSettings()
    {
        var token = "token";

        var options = new MailtrapClientOptions(token);

        options.Authentication.ApiToken.Should().Be(token);
    }
}
