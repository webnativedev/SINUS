// <copyright file="SimpleBrowserTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest.Assertions;
using WebNativeDEV.SINUS.MsTest.Chrome;
using WebNativeDEV.SINUS.SystemUnderTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class SimpleBrowserTests : ChromeTestBase
{
    private const string SimpleViewTitle = "SINUS TestSystem";
    private readonly (string?, string?) simpleView = ("SimpleView", "/simpleView");

    [TestMethod]
    [DoNotParallelize]
    public void Given_AWebsite_When_CreatingScreenshot_Then_NoExceptionShouldOccur()
        => this.Test()
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("making a screenshot", (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur")
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_AWebsite_When_StoringTheTitle_Then_ItShouldBeCorrect()
        => this.Test()
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => Assert.That.AreEqualToActual(data, SimpleViewTitle))
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_AWebsite_When_CallingPrintUsageStatistic_Then_ItShouldReturnOneLeakingBecauseDisposeCalledAfterwards()
        => this.Test()
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("storing the title", (browser, data) => data.StoreActual(browser?.Title))
            .Then("it should equal to the real title", (browser, data) => PrintBrowserUsageStatistic())
            .Dispose();

    [TestMethod]
    public void Given_ABrowser_When_StoreData_Then_NoThrow()
    {
        this.Test()
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("Calling Browser", (browser, data) =>
            {
                data.Store(browser.PageSource);
                data.Store(browser.Title);
                data.Store(browser.Url);
                data.Store("element active: " + (browser.FindActiveElement()?.Text ?? "<none>"));
            })
            .Then("no exception should be thrown")
            .DebugPrint()
            .Dispose();
    }
}
