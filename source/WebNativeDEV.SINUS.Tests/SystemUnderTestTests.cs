// <copyright file="SystemUnderTestTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.MsTest.Chrome;
using WebNativeDEV.SINUS.SystemUnderTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

/// <summary>
/// These tests demonstrate the basic usage of the ui driven test base by a system reference.
/// </summary>
[TestClass]
public sealed partial class SystemUnderTestTests : ChromeTestBase
{
    [TestMethod]
    [DoNotParallelize]
    public void Given_SUT_When_CallingView_Then_SeleniumBrowsable_WithRunner()
        => this.UITest()
            .GivenASystemAndABrowserAt<Program>(
                humanReadablePageName: "SimpleView",
                endpoint: "https://localhost:10001",
                url: new Uri("https://localhost:10001/simpleView"))
            .When(
                "making a screenshot",
                (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur")
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_SUT_When_CallingView_Then_TitleShouldBeRight()
        => this.UITest()
            .GivenASystemAndABrowserAt<Program>(
                "SimpleView",
                endpoint: "https://localhost:10001",
                url: new Uri("https://localhost:10001/simpleView"))
            .When(
                "checking the title",
                (browser, data) => data.Add("Title", browser.Title))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (browser, data) => Assert.AreEqual("SINUS TestSystem", (string?)data["Title"]))
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_SUT_When_CallingCalcToSquareMyNumberWith2_Then_ResultShouldBe4()
        => this.Test()
            .GivenASystem<Program>("Calculation REST-Service")
            .When(
                "checking the title",
                (client, data) => data.Add("Result", client.GetStringAsync("/calc/2").GetAwaiter().GetResult()))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (data) => Assert.AreEqual("4", data["Result"] as string))
            .Dispose();
}
