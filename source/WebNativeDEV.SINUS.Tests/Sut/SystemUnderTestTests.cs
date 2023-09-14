// <copyright file="SystemUnderTestTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Sut;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.Execution.Exceptions;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.SystemUnderTest;
using WebNativeDEV.SINUS.SystemUnderTest.Controllers;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

/// <summary>
/// These tests demonstrate the basic usage of the ui driven test base by a system reference.
/// </summary>
[TestClass]
public sealed partial class SystemUnderTestTests : TestBase
{
    private readonly (string?, string?) simpleView = ("SimpleView", "/simpleView");

    [TestMethod]
    public void Given_Sut_When_CallingView_Then_SeleniumBrowsableWithRunner()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("making a screenshot", (browser, data) => browser.TakeScreenshot())
            .Then("no exception should occur"));

    [TestMethod]
    public void Given_SutOnRandomEndpoint_When_CallingView_Then_SeleniumBrowsableWithRunner()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("making a screenshot", (browser, data) => browser.TakeScreenshot())
            .ThenNoError()).Should().BeSuccessful();

    [TestMethod]
    public void Given_Sut_When_CallingView_Then_TitleShouldBeRight()
        => this.Test(r => r
            .GivenASystemAndABrowserAtDefaultEndpoint<Program>(this.simpleView)
            .When("checking the title", (browser, data) => data.StoreActual(browser.Title ?? string.Empty))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (browser, data) => data.Should().ActualBe("SINUS TestSystem")));

    [TestMethod]
    [TechnicalRequirement("public available system")]
    public void Given_SutOnRandomEndpoint_When_CallingView_Then_TitleShouldBeRight()
        => this.Test(r => r
            .GivenASystemAndABrowserAtRandomEndpoint<Program>(this.simpleView)
            .When("checking the title", (browser, data) => data.StoreActual(browser.Title ?? string.Empty))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (browser, data) => data.Should().ActualBe("SINUS TestSystem")));

    [TestMethod]
    [TechnicalRequirement("in memory system")]
    [OutOfScope("reachable from outside via selenium")]
    public void Given_Sut_When_CallingCalcToSquareMyNumberWith2_Then_ResultShouldBe4()
        => this.Test(r => r
            .GivenASystem<Program>("Calculation REST-Service")
            .When(
                "calculate square",
                (client, data) => data.StoreActual(client.GetStringAsync("/calc/2").GetAwaiter().GetResult()))
            .Then(
                "Title should be 'SINUS TestSystem'",
                (data) => data.Should().ActualBe("4")));

    [TestMethod]
    public void Given_Sut_When_CallingCalcToSquareMyNumberWithMinus2_Then_ResultShouldBe4()
        => this.Test(r => r
            .GivenASystem<Program>()
            .When((client, data) => data.StoreActual(client.GetStringAsync("/calc/-2").GetAwaiter().GetResult()))
            .Then((data) => data.Should().ActualBe("4")));

    [TestMethod]
    public void Given_SutClass_When_CallingCalcToSquareMyNumberWith2_Then_ResultShouldBe4()
        => this.Test(r => r
            .Given(
                "instance of a calculation controller",
                data => data.StoreSut(new CalcController()))
            .When(
                "calling the calculation method",
                data => data.StoreActual(data.ReadSut<CalcController>().CalculateSquare(2)))
            .Then("check value does be 4", data => data.Should().ActualBe(4)));

    [TestMethod]
    public void Given_SutWithErrorInBootstrapping_When_Execution_Then_Error()
        => this.Test(r => r
            .GivenASystem<Program>(args: "start-with-exception")
            .When((client, data) => data.Actual = "should not run")
            .ThenShouldHaveFailedWith<ExecutionEngineRunException>());
}