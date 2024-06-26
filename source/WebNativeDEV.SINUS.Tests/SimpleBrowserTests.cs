﻿// <copyright file="SimpleBrowserTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.Core.Utils;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.SystemUnderTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class SimpleBrowserTests : TestBase
{
    private const string SimpleViewTitle = "SINUS TestSystem";
    private readonly (string?, string?) simpleView = ("SimpleView", "/simpleView");
    private readonly BrowserFactoryOptions optionEdge = new (true, true, SupportedWebDriver.Edge);

    [TestMethod]
    public void Given_AWebsite_When_CreatingScreenshot_Then_NoExceptionShouldOccur()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("making a screenshot", (browser, data) => browser.TakeScreenshot())
            .ThenNoError()).Should().BeSuccessful();

    [TestMethod]
    public void Given_AWebsiteInEdge_When_CreatingScreenshot_Then_NoExceptionShouldOccur()
    => this.Test(r => r
        .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView, this.optionEdge)
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
    public void Given_AWebsiteInEdge_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView, this.optionEdge)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.Should().ActualBe(SimpleViewTitle)));

    [TestMethod]
    public void Given_AWebsiteOnRandomEndpoint_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.Should().ActualBe(SimpleViewTitle)));

    [TestMethod]
    public void Given_ABlankWebsiteInChromeNotHeadless_When_StoringTheTitle_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .GivenABrowserAt(
                ("empty page", "about:blank"),
                new BrowserFactoryOptions()
                {
                    Headless = false,
                    IgnoreSslErrors = false,
                    WebDriver = SupportedWebDriver.Chrome,
                })
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.ReadActual<string>().Should().NotBeNull()));

    [TestMethod]
    public void Given_ABlankWebsiteInEdgeNotHeadless_When_StoringTheTitle_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .GivenABrowserAt(
                ("empty page", "about:blank"),
                new BrowserFactoryOptions()
                {
                    Headless = false,
                    IgnoreSslErrors = false,
                    WebDriver = SupportedWebDriver.Edge,
                })
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.ReadActual<string>().Should().NotBeNull()));

    [TestMethod]
    public void Given_ABlankWebsite_When_StoringTheTitle_Then_ItShouldNotBeNull()
        => this.Test(r => r
            .GivenABrowserAt(("empty page", "about:blank"))
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.ReadActual<string>().Should().NotBeNull()));

    [TestMethod]
    public void Given_ABlankWebsiteAsUri_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test(r => r
            .GivenABrowserAt("empty page", new Uri("about:blank"))
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => data.ReadActual<string>().Should().NotBeNull()));

    [TestMethod]
    public void Given_AWebsite_When_CallingPrintUsageStatistic_Then_ItShouldReturnOneLeakingBecauseDisposeCalledAfterwards()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should print usage statistics", (browser, data) => { })
            .Debug((browser, data) => SinusUtils.PrintUsageStatistic(data.TestName)));

    [TestMethod]
    public void Given_AWebsite_When_CallingPrintUsageStatisticAfterwards_Then_ItShouldNotReturnOneLeakingBecauseDisposeCalledAfterwards()
    {
        string? testName = null;

        this.Test(r => r
                .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
                .When((browser, data) => data.StoreActual(browser?.Title))
                .ThenNoError()
                .Debug(data => testName = data.TestName));

        SinusUtils.PrintUsageStatistic(
            testName ?? throw new InvalidDataException("testname not stored in debug phase"));

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.CheckAttribute(
            testName,
            data => data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserCreated) &&
                    data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserDisposed) &&
                    data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafCreated) &&
                    data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafDisposed))
            .Should().BeTrue();
    }

    [TestMethod]
    public void Given_AWebsiteOnRandomEndpoint_When_CallingPrintUsageStatistic_Then_ItShouldReturnOneLeakingBecauseDisposeCalledAfterwards()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should print usage statistics", (browser, data) => { })
            .Debug((browser, data) => SinusUtils.PrintUsageStatistic(data.TestName)));

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
                data.Store(browser.HumanReadablePageName);
                data.Store(browser.GetBaseObject());
                data.Store("element active: " + (browser.FindActiveElement()?.Text ?? "<none>"));
            })
            .Then("no exception should be thrown")
            .DebugPrint(RunStorePrintOrder.KeySorted));
    }

    [TestMethod]
    public void Given_ABrowserOnRandomEndpoint_When_CallingDifferentBrowserActions_Then_NoThrow()
    {
        this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("Calling Browser", (browser, data) =>
            {
                data.Store("text", browser.FindElement("titleText").Text);
                data.Store("countTitles", browser.FindElements("//@titleText").Count());
                browser.NavigateTo("about:blank");
            })
            .ThenNoError()
            .DebugPrint()).Should().BeSuccessful();
    }

    [TestMethod]
    public void Given_ATestSystem_When_StoringTheTitle_Then_ItShouldBeTheRightValue()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>("SimpleView", "/simpleView")
            .When((browser, data) => data.StoreActual(browser.Title))
            .Then((browser, data) => data.Should().ActualBe("SINUS TestSystem"))
            .DebugPrint());
}
