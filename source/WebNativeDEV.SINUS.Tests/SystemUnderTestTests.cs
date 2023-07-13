// <copyright file="SystemUnderTestTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.MsTest.Assertions;
using WebNativeDEV.SINUS.MsTest.Chrome;
using WebNativeDEV.SINUS.SystemUnderTest;
using WebNativeDEV.SINUS.SystemUnderTest.Controllers;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

/// <summary>
/// These tests demonstrate the basic usage of the ui driven test base by a system reference.
/// </summary>
[TestClass]
public sealed partial class SystemUnderTestTests : ChromeTestBase
{
    private readonly (string?, string?) simpleView = ("SimpleView", "/simpleView");

    [TestMethod]
    public void Given_ASystemUnderTest_When_StepsAreTaken_Then_ACheckVerifiesTheIdea()
        => this.Test()
            .Given("a SUT")
            .When("steps are taken")
            .Then("verification is done")
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_Sut_When_CallingView_Then_SeleniumBrowsable_WithRunner()
        => this.Test()
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("making a screenshot", (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur")
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_Sut_When_CallingView_Then_TitleShouldBeRight()
        => this.Test()
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("checking the title", (browser, data) => data.StoreActual(browser.Title ?? string.Empty))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (browser, data) => Assert.That.AreEqualToActual(data, "SINUS TestSystem"))
            .Dispose();

    [TestMethod]
    [DoNotParallelize]
    public void Given_Sut_When_CallingCalcToSquareMyNumberWith2_Then_ResultShouldBe4()
        => this.Test()
            .GivenASystem<Program>("Calculation REST-Service")
            .When(
                "checking the title",
                (client, data) => data.StoreActual(client.GetStringAsync("/calc/2").GetAwaiter().GetResult()))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (data) => Assert.That.AreEqualToActual(data, "4"))
            .Dispose();

    [TestMethod]
    public void Given_SutClass_When_CallingCalcToSquareMyNumberWith2_Then_ResultShouldBe4()
        => this.Test()
            .Given(
                "instance of a calculation controller",
                data => data.StoreSut(new CalcController()))
            .When(
                "calling the calculation method",
                data => data.StoreActual(data.ReadSut<CalcController>().CalculateSquare(2)))
            .Then("check value does be 4", data => Assert.That.AreEqualToActual(data, 4))
            .Dispose();
}