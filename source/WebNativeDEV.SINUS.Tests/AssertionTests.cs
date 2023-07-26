// <copyright file="AssertionTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.FluentAssertions;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class AssertionTests : TestBase
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void Given_Assertions_When_DataIsUsed_Then_ItShouldNotBeNullInWhen()
        => this.Test()
            .Given("Assertions extended")
            .When("Running assert with null", data => data.Should().NotBeNull())
            .Then("exception should be thrown")
            .Dispose();

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_Assertions_When_CheckActual_Then_CheckForNullThrowsAnError()
        => this.Test()
            .Given("Assertions extended and actual value available", data => data.Actual = "test")
            .When("Running assert with null", data => data.Should().ActualBeNull())
            .Then("exception should be thrown")
            .Dispose();

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_Assertions_When_CheckActualAfterMethodStore_Then_CheckForNullThrowsAnError()
        => this.Test()
            .Given("Assertions extended and actual value available", data => data.StoreActual("test"))
            .When("Running assert with null", data => data.Should().ActualBeNull())
            .Then("exception should be thrown")
            .Dispose();
}
