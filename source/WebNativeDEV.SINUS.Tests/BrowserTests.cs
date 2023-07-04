// <copyright file="BrowserTests.cs" company="WebNativeDEV">
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
public sealed class BrowserTests : ChromeTestBase
{
    private (string, string) Google { get; } = ("Google", "https://www.google.at");

    [TestMethod]
    public void Given_Browser_When_GotoAnyUrl_Then_NoExceptionOccursCallingDispose()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished")
            .Then("no exception should occur", (browser, data) => Assert.IsNotNull(browser))
            .Dispose();

    [TestMethod]
    public void Given_Browser_When_GotoAnyUrl_Then_NoExceptionOccursCallingDebugAndDispose()
     => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished")
            .Then("no exception should occur", (browser, data) => Assert.IsNotNull(browser))
            .Debug((browser, data) => { })
            .Dispose();

    [TestMethod]
    public void Given_Browser_When_GotoAnyUrl_Then_NoExceptionOccursDisposwByVarUsing()
    {
        using var runner = this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished")
            .Then("no exception should occur", (browser, data) => Assert.IsNotNull(browser));
    }

    [TestMethod]
    public void Given_Browser_When_GotoAnyUrl_Then_NoExceptionOccursDisposwByVarUsingWithDebug()
    {
        using var runner = this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished")
            .Then("no exception should occur", (browser, data) => Assert.IsNotNull(browser))
            .Debug((_, _) => { });
    }

    [TestMethod]
    public void Given_Browser_When_GotoAnyUrl_Then_NoExceptionOccursDisposeByUsingBlock()
    {
        using (this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished")
            .Then("no exception should occur", (browser, data) => Assert.IsNotNull(browser)))
        {
        }
    }

    [TestMethod]
    public void Given_ABrowser_When_GotoSomeWebsite_Then_TitleShouldBeSet()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Navigation to page finished")
            .Then("check tab name", (browser, data) => Assert.IsNotNull(browser.Title))
            .Dispose();

    [TestMethod]
    public void Given_ABrowserOpensGoogle_When_ReadingTheTitle_Then_TitleShouldBeSet()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Reading the title", (browser, data) => data["Title"] = browser.Title)
            .Then("Title should be set", (browser, data) => Assert.IsNotNull(data["Title"]))
            .Dispose();

    [TestMethod]
    public void Given_Browser_When_GotoGoogle_Then_TakeScreenshotShouldWork()
        => this.Test()
            .GivenABrowserAt(this.Google)
            .When("Taking a snapshot", (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur")
            .Dispose();
}