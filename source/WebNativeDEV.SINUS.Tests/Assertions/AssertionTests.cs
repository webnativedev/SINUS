// <copyright file="AssertionTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Assertions;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
[BusinessRequirements(
    "Assertion",
    "B001:Check actual of RunStore for null value",
    "B002:Check actual of RunStore for exact value")]
[TechnicalRequirements(
    "Assertion",
    "T001:FluentAssertions work")]
public class AssertionTests : TestBase
{
    [TestMethod]
    [BusinessRequirement("B001:Check actual of RunStore for null value")]
    public void Given_FluentAssertions_When_DataIsUsed_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .Given("FluentAssertions")
            .When("Data is used", data => data["test"] = 1)
            .Then("it should not be null", data => data.Should().NotBeNull()));

    [TestMethod]
    public void Given_FluentAssertions_When_DataIsNotUsed_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .Given()
            .When(data => { })
            .Then(data => data.Should().NotBeNull()));

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActual_Then_CheckForNullThrowsAnError()
        => this.Test(r => r
            .Given("Assertions extended and actual value available")
            .When("Running assert with null", data => data.Actual = "test")
            .Then("exception should be thrown", data => data.Should().ActualBeNull())
            .ExpectFail());

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActualAfterMethodStore_Then_CheckForNullThrowsNoError()
        => this.Test(r => r
            .Given("Assertions extended and actual value available", data => data.StoreActual("test"))
            .When("Running assert with null", data => data.Should().ActualBeNotNull().And.ActualBe("test"))
            .ThenNoError());

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActual_Then_CheckForNotNullThrowsAnError()
        => this.Test(r => r
            .Given("Assertions extended and actual value not available")
            .When("Running assert with not null", data => data.InitActual())
            .Then("exception should be thrown", data => data.Should().ActualBeNotNull())
            .ExpectFail());

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActualAfterMethodStore_Then_CheckForNotNullThrowsNoError()
        => this.Test(r => r
            .Given("Assertions extended and actual value available", data => data.StoreActual(null))
            .When("Running assert with not null", data => data.Should().ActualBeNull())
            .ThenNoError("no error expected"));

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActualAfterMethodStore_Then_CheckForNotNullWithActualBe()
        => this.Test(r => r
            .Given(data => data.StoreActual(null))
            .When(data => data.Should().ActualBe<string>(null!))
            .ThenNoError());

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActualAfterMethodStoreWithNonNull_Then_CheckForNullWithActualBe()
        => this.Test(r => r
            .Given(data => data.StoreActual("test"))
            .When(data => data.Should().ActualBe<string>(null!))
            .ThenShouldHaveFailed());

    [TestMethod]
    public void Given_FluentAssertions_When_CheckActualAfterMethodStoreWithNull_Then_CheckForNotNullWithActualBe()
        => this.Test(r => r
            .Given(data => data.StoreActual(null))
            .When(data => data.Should().ActualBe<string>("test"))
            .ThenShouldHaveFailed());

    [TestMethod]
    public void Given_FluentAssertions_When_PrintStore_Then_NoError()
        => this.Test(r => r
            .Given("a simple setup")
            .When(data => data.PrintStore())
            .ThenNoError()).Should().BeSuccessful();

    [TestMethod]
    public void Given_FluentAssertions_When_PrintStoreUnsorted_Then_NoError()
        => this.Test(r => r
            .Given("a simple setup")
            .When(data => data.PrintStore(RunStorePrintOrder.Unsorted))
            .ThenNoError()).Should().BeSuccessful();
}
