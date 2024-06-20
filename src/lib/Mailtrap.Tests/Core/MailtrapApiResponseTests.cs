﻿// -----------------------------------------------------------------------
// <copyright file="MailtrapApiResponseTests.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.Tests.Core;


[TestFixture]
internal sealed class MailtrapApiResponseTests
{
    [Test]
    public void Constructor_ShouldProperlyInitializeProperties_WhenSuccess()
    {
        var response = new MailtrapApiResponse<string>(true);

        response.IsSuccess.Should().BeTrue();
        response.ErrorData.Should().BeNull();
    }

    [Test]
    public void Constructor_ShouldProperlyInitializeProperties_WhenError()
    {
        var errorMessage = "Error";
        var response = new MailtrapApiResponse<string>(false, errorMessage);

        response.IsSuccess.Should().BeFalse();
        response.ErrorData.Should()
            .BeOfType<string>().And
            .Be(errorMessage);
    }
}
