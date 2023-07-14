// <copyright file="GoogleBrowserTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.MsTest.Chrome;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

/// <summary>
/// These tests demonstrate the basic usage of the ui driven test base.
/// </summary>
[TestClass]
public sealed class GoogleBrowserTests : ChromeTestBase
{
    private (string, string) Google { get; } = ("Google", "https://www.google.at");

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogle_When_CheckTitle_Then_TitleIsNotNull()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
            .Then("Title should not be null", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()))
            .Dispose();

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogle_When_CheckTitle_Then_TitleIsNotNull_Debug_AllStoredValues()
     => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
            .Then("no exception should occur", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()))
            .Debug((browser, data) => data.Print())
            .Dispose();

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogleWithUsing_When_CheckTitle_Then_TitleIsNotNull()
    {
        using var runner = this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
            .Then("Title should not be null", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()));
    }

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogleWithUsing_When_CheckTitle_Then_TitleIsNotNull_Debug_AllStoredValues()
    {
        using var runner = this.Test()
                .GivenABrowserAt(this.Google)
                .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
                .Then("no exception should occur", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()))
                .Debug((browser, data) => data.Print());
    }

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogleWithUsingBlock_When_CheckTitle_Then_TitleIsNotNull_Debug_AllStoredValues()
    {
        using (this.Test()
                .GivenABrowserAt(this.Google)
                .When("Navigation to page finished", (browser, data) => data.StoreActual(browser.Title!))
                .Then("no exception should occur", (browser, data) => Assert.IsNotNull(data.ReadActual<string>()))
                .Debug((browser, data) => data.Print()))
        {
        }
    }

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogle_When_Nothing_Then_ResultInconclusive()
    => this.Test()
        .GivenABrowserAt(this.Google)
        .When("Navigation to page finished")
        .Then("Title should not be null", (browser, data) => Assert.IsNotNull(browser))
        .Dispose();

    [TestMethod]
    [TestCategory("external")]
    public void Given_ABrowserLoadingGoogle_When_TakeASnapshot_Then_NoExceptionShouldOccur()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Taking a snapshot", (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur")
            .Dispose();
}