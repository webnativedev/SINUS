// <copyright file="SimpleBrowserTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.MsTest.Extensions;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.SystemUnderTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class SimpleBrowserTests : TestBase
{
    private const string SimpleViewTitle = "SINUS TestSystem";
    private readonly (string?, string?) simpleView = ("SimpleView", "/simpleView");

    [TestMethod]
    public void Given_AWebsite_When_CreatingScreenshot_Then_NoExceptionShouldOccur()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("making a screenshot", (browser, data) => browser.TakeScreenshot())
            .ThenNoError()).Should().BeSuccessful();

    [TestMethod]
    public void Given_AWebsiteOnRandomEndpoint_When_CreatingScreenshot_Then_NoExceptionShouldOccur()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When((browser, data) => browser.TakeScreenshot())
            .Then()).Should().BeSuccessful();

    [TestMethod]
    public void Given_AWebsite_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.Should().ActualBe(SimpleViewTitle)));

    [TestMethod]
    public void Given_AWebsiteOnRandomEndpoint_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.Should().ActualBe(SimpleViewTitle)));

    [TestMethod]
    public void Given_ABlankWebsiteNotHeadless_When_StoringTheTitle_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .GivenABrowserAt(
                ("empty page", "about:blank"),
                new BrowserFactoryOptions()
                {
                    Headless = false,
                    IgnoreSslErrors = false,
                })
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => Assert.IsNotNull(data.ReadActual<string>())));

    [TestMethod]
    public void Given_ABlankWebsite_When_StoringTheTitle_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .GivenABrowserAt(("empty page", "about:blank"))
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => Assert.IsNotNull(data.ReadActual<string>())));

    [TestMethod]
    public void Given_ABlankWebsiteAsUri_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test(r => r
            .GivenABrowserAt("empty page", new Uri("about:blank"))
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => Assert.IsNotNull(data.ReadActual<string>())));

    [TestMethod]
    public void Given_AWebsite_When_CallingPrintUsageStatistic_Then_ItShouldReturnOneLeakingBecauseDisposeCalledAfterwards()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should print usage statistics", (browser, data) => { })
            .Debug((browser, data) => this.PrintUsageStatistic(this.TestName)));

    [TestMethod]
    public void Given_AWebsiteOnRandomEndpoint_When_CallingPrintUsageStatistic_Then_ItShouldReturnOneLeakingBecauseDisposeCalledAfterwards()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should print usage statistics", (browser, data) => { })
            .Debug((browser, data) => this.PrintUsageStatistic(this.TestName)));

    [TestMethod]
    public void Given_ABrowser_When_StoreData_Then_NoThrow()
    {
        this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("Calling Browser", (browser, data) =>
            {
                data.Store(browser.PageSource);
                data.Store(browser.Title);
                data.Store(browser.Url);
                data.Store("element active: " + (browser.FindActiveElement()?.Text ?? "<none>"));
            })
            .Then("no exception should be thrown")
            .DebugPrint());
    }

    [TestMethod]
    public void Given_ABrowserOnRandomEndpoint_When_StoreData_Then_NoThrow()
    {
        this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("Calling Browser", (browser, data) =>
            {
                data.Store(browser.PageSource);
                data.Store(browser.Title);
                data.Store(browser.Url);
                data.Store("element active: " + (browser.FindActiveElement()?.Text ?? "<none>"));
            })
            .Then("no exception should be thrown")
            .DebugPrint());
    }
}
