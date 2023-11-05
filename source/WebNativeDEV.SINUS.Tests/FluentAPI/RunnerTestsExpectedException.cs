// <copyright file="RunnerTestsExpectedException.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.FluentAPI;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
[Ignore("Legacy usage of expected exception, but still working")]
public class RunnerTestsExpectedException : TestBase
{
    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInWhen_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisibleIfNotCaught()
    {
        this.Test(r => r
            .Given("An setup step with an error")
            .When("An excecution occurs", data => throw new Exception("Execution failed"))
            .ThenNoError());
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInThen_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test(r => r
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.PrintStore())
            .Then("The error should be visible", data => throw new Exception("Verification failed")));
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInThen2x_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test(r => r
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.PrintStore())
            .Then(
                "The error should be visible",
                data => throw new Exception("Verification failed"),
                data => throw new Exception("Verification failed")));
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInThen2x_When_ExceptionIsThrownAndAssertion_Then_ThisErrorShouldBeVisible()
    {
        this.Test(r => r
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.PrintStore())
            .Then(
                "The error should be visible",
                data => throw new Exception("Verification failed"),
                data => Assert.Fail("Error happens here")));
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    [BusinessRequirement("set inconclusive if no code in when-block")]
    public void Given_ARunner_When_NotExecutingAnyCode_Then_TheTestShouldBeInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code")
            .When("An excecution without code")
            .Then("A validation without code"));
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunnerWithGiven_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code", data => data["test"] = 11)
            .When("An excecution without code")
            .Then("A validation without code")
            .DebugPrint());
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunnerWithGivenAndThen_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code", data => data["test"] = 11)
            .When("An excecution without code")
            .Then("A validation without code", data => data["test2"] = data["test"])
            .DebugPrint());
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunnerWithThen_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code")
            .When("An excecution without code")
            .Then("A validation without code", data => data["test2"] = 22)
            .DebugPrint());
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_SimpleFlow_When_ExceptionOccurs_Then_NoErrorFunctionShouldFail()
        => this.Test(r => r
            .Given()
            .When(data => throw new InvalidDataException())
            .ThenNoError());

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_SimpleFlow_When_Working_Then_ShouldFailFunctionShouldFail()
        => this.Test(r => r
            .Given()
            .When(data => { })
            .ThenShouldHaveFailed());

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_SimpleFlow_When_Working_Then_ShouldFailWithFunctionShouldFail()
        => this.Test(r => r
            .Given()
            .When(data => { })
            .ThenShouldHaveFailedWith<Exception>());

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_SimpleDocumentedFlow_When_Working_Then_ShouldFailFunctionShouldFail()
        => this.Test(r => r
            .Given("a simple flow")
            .When("exception occured", data => { })
            .ThenShouldHaveFailed("no error"));
}
