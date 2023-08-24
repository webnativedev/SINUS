// <copyright file="GoogleBrowserTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

/// <summary>
/// These tests demonstrate the basic usage of the ui driven test base.
/// </summary>
[TestClass]
[BusinessRequirements(
    "InternetAccessViaBrowser",
    "001:Read Title",
    "002:Take Screenshot")]
public sealed class GoogleBrowserTests : TestBase
{
    private (string, string) Google { get; } = ("Google", "https://www.google.at");

    [TestMethod]
    [TestCategory("external")]
    [BusinessRequirement("001:Read Title")]
    public void Given_ABrowserLoadingGoogle_When_CheckTitle_Then_TitleIsNotNull()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When(
                "Navigation to page finished",
                (browser, data) => data.Actual = browser.Title)
            .Then(
                "Title should not be null",
                (browser, data) => data.Actual.Should().NotBeNull())
            .Dispose();

    [TestMethod]
    [TestCategory("external")]
    [BusinessRequirement("001:Read Title")]
    public void Given_ABrowserLoadingGoogle_When_CheckTitle_Then_TitleIsNotNull_Debug_AllStoredValues()
     => this.Test()
            .GivenABrowserAt(this.Google)
            .When(
                "Navigation to page finished",
                (browser, data) => data.StoreActual(browser.Title!))
            .Then(
                "no exception should occur",
                (browser, data) => data.ReadActual<string>().Should().NotBeNullOrWhiteSpace())
            .Debug((browser, data) => data.PrintStore())
            .Dispose();

    [TestMethod]
    [TestCategory("external")]
    [BusinessRequirement("001:Read Title")]
    public void Given_ABrowserLoadingGoogleWithUsing_When_CheckTitle_Then_TitleIsNotNull()
    {
        using var runner = this.Test()
            .GivenABrowserAt(this.Google)
            .When(
                "Navigation to page finished",
                (browser, data) => data.StoreActual(browser.Title!))
            .Then(
                "Title should not be null",
                (browser, data) => Assert.IsNotNull(data.ReadActual<string>()));
    }

    [TestMethod]
    [TestCategory("external")]
    [BusinessRequirement("001:Read Title")]
    public void Given_ABrowserLoadingGoogleWithUsing_When_CheckTitle_Then_TitleIsNotNull_Debug_AllStoredValues()
    {
        using var runner = this.Test()
                .GivenABrowserAt(this.Google)
                .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
                .Then("no exception should occur", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()))
                .Debug((browser, data) => data.PrintStore());
    }

    [TestMethod]
    [TestCategory("external")]
    [BusinessRequirement("001:Read Title")]
    public void Given_ABrowserLoadingGoogleWithUsingBlock_When_CheckTitle_Then_TitleIsNotNull_Debug_AllStoredValues()
    {
        #pragma warning disable IDE0063 // Use simple using-Statement.

        using (var runner = this.Test()
                .GivenABrowserAt(this.Google)
                .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
                .Then("no exception should occur", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()))
                .Debug((browser, data) => data.PrintStore()))
        {
            runner.Should().NotBeNull();
        }

        #pragma warning restore IDE0063 // Use simple using-Statement.
    }

    [TestMethod]
    [TestCategory("external")]
    [ExpectedException(typeof(AssertInconclusiveException))]
    [TechnicalApproval("Empty When-Block leads to inconclusive result.")]
    [TechnicalApproval("Browser injected correctly in Then block.")]
    public void Given_ABrowserLoadingGoogle_When_Nothing_Then_ResultInconclusive()
    => this.Test()
        .GivenABrowserAt(this.Google)
        .When("Navigation to page finished")
        .Then("Title should not be null", (browser, data) => browser.Should().NotBeNull())
        .Dispose();

    [TestMethod]
    [TestCategory("external")]
    [BusinessRequirement("002:Take Screenshot")]
    public void Given_ABrowserLoadingGoogle_When_TakeASnapshot_Then_NoExceptionShouldOccur()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Taking a snapshot", (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur")
            .Dispose();
}