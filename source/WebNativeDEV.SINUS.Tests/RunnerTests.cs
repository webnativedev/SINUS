// <copyright file="RunnerTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class RunnerTests : TestBase
{
    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInGiven_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test()
            .Given("An setup step with an error", data => throw new Exception("Setup failed"))
            .When("An excecution occurs", data => data.Print())
            .Then("The error should be visible")
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInWhen_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test()
            .Given("An setup step with an error")
            .When("An excecution occurs", data => throw new Exception("Execution failed"))
            .Then("The error should be visible")
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInThen_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test()
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.Print())
            .Then("The error should be visible", data => throw new Exception("Verification failed"))
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInThen2x_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test()
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.Print())
            .Then(
                "The error should be visible",
                data => throw new Exception("Verification failed"),
                data => throw new Exception("Verification failed"))
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_ARunnerWithAnErrorInThen2x_When_ExceptionIsThrownAndAssertion_Then_ThisErrorShouldBeVisible()
    {
        this.Test()
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.Print())
            .Then(
                "The error should be visible",
                data => throw new Exception("Verification failed"),
                data => Assert.Fail("Error happens here"))
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunner_When_NotExecutingAnyCode_Then_TheTestShouldBeInconclusive()
    {
        this.Test()
            .Given("A setup without code")
            .When("An excecution without code")
            .Then("A validation without code")
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunnerWithGiven_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeInconclusive()
    {
        this.Test()
            .Given("A setup without code", data => data["test"] = 11)
            .When("An excecution without code")
            .Then("A validation without code")
            .DebugPrint()
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunnerWithGivenAndThen_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeInconclusive()
    {
        this.Test()
            .Given("A setup without code", data => data["test"] = 11)
            .When("An excecution without code")
            .Then("A validation without code", data => data["test2"] = 22)
            .DebugPrint()
            .Dispose();
    }

    [TestMethod]
    [ExpectedException(typeof(AssertInconclusiveException))]
    public void Given_ARunnerWithThen_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeInconclusive()
    {
        this.Test()
            .Given("A setup without code")
            .When("An excecution without code")
            .Then("A validation without code", data => data["test2"] = 22)
            .DebugPrint()
            .Dispose();
    }
}
