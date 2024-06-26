﻿// <copyright file="RunnerTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.FluentAPI;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class RunnerTests : TestBase
{
    [TestMethod]
    public void Given_ARunnerWithAnErrorInGiven_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test(r => r
            .Given("An setup step with an error", data => throw new Exception("Setup failed"))
            .When("An excecution occurs", data => data.PrintStore())
            .ThenShouldHaveFailedWith<Exception>());
    }

    [TestMethod]
    public void Given_ARunnerWithAnErrorInWhen_When_ExceptionIsThrown_Then_ThisErrorShouldBeVisible()
    {
        this.Test(r => r
            .Given("An setup step without an error")
            .When("An excecution occurs", data => throw new Exception("Execution failed"))
            .ThenShouldHaveFailedWith<Exception>());
    }

    [TestMethod]
    public void Given_ARunnerWithAnErrorInWhen_When_ExceptionIsThrown_Then_ThisErrorShouldBeExpected()
    {
        this.Test(r => r
            .Given("An setup step without an error")
            .When("An excecution occurs", data => throw new Exception("Execution failed"))
            .ThenNoError()
            .ExpectFail());
    }

    [TestMethod]
    public void Given_ARunnerWithAnErrorInThen_When_ExceptionIsThrown_Then_ThisErrorShouldBeExpected()
    {
        this.Test(r => r
            .Given("An setup step without an error")
            .When("An excecution occurs", data => data.PrintStore())
            .Then("The error should be visible", data => throw new Exception("Verification failed"))
            .ExpectFail());
    }

    [TestMethod]
    public void Given_ARunnerWithAnErrorInThen2x_When_ExceptionIsThrown_Then_ThisErrorShouldBeExpected()
    {
        this.Test(r => r
            .Given("An setup step without an error")
            .When("An excecution occurs", data => data.PrintStore())
            .Then(
                "The error should be visible",
                data => throw new Exception("Verification failed"),
                data => throw new Exception("Verification failed"))
            .ExpectFail());
    }

    [TestMethod]
    public void Given_ARunnerWithAnErrorInThen2x_When_ExceptionIsThrownAndAssertion_Then_ThisErrorShouldBeExpected()
    {
        this.Test(r => r
            .Given("An setup step with an error")
            .When("An excecution occurs", data => data.PrintStore())
            .Then(
                "The error should be visible",
                data => throw new Exception("Verification failed"))
            .ExpectFail());
    }

    [TestMethod]
    [BusinessRequirement("set inconclusive if no code in when-block")]
    public void Given_ARunner_When_NotExecutingAnyCode_Then_TheTestShouldBeExpectedAsInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code")
            .When("An excecution without code")
            .Then("A validation without code")
            .ExpectInconclusive());
    }

    [TestMethod]
    public void Given_ARunnerWithGiven_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeExpectedAsInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code", data => data["test"] = 11)
            .When("An excecution without code")
            .Then("A validation without code")
            .DebugPrint()
            .ExpectInconclusive());
    }

    [TestMethod]
    public void Given_ARunnerWithGivenAndThen_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeExpectedAsInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code", data => data["test"] = 11)
            .When("An excecution without code")
            .Then("A validation without code", data => data["test2"] = data["test"])
            .DebugPrint()
            .ExpectInconclusive());
    }

    [TestMethod]
    public void Given_ARunnerWithThen_When_NotExecutingAnyCodeInWhen_Then_TheTestShouldBeExpectedAsInconclusive()
    {
        this.Test(r => r
            .Given("A setup without code")
            .When("An excecution without code")
            .Then("A validation without code", data => data["test2"] = 22)
            .DebugPrint()
            .ExpectInconclusive());
    }

    [TestMethod]
    public void Given_ARunnerWithRunStore_When_StoringDataInDifferentWays_Then_AllShouldBeAvailable()
    {
        this.Test(r => r
            .Given("A RunStore")
            .When("saving some data", data =>
            {
                data.Store(item: 1.555);
                data.Store(key: "key2", item: 2);
                data.StoreActual(item: "3");
                data.StoreSut(systemUnderTest: "sut");
                data.StoreActual<string>(sut => sut + "3");
                data["key5"] = 5;
            })
            .Then(
                "All data could be read",
                data => data.Read<double>().Should().Be(1.555d),
                data => data.Read<int>("key2").Should().Be(2),
                data => data.ReadSut<string>().Should().Be("sut"),
                data => data.ReadActual<string>().Should().Be("sut3"),
                data => data["key5"].Should().Be(5))
            .DebugPrint());
    }

    [TestMethod]
    public void Given_ARunnerWithRunStore_When_StoringActualWithNoMethod_Then_NoError()
    {
        this.Test(r => r
            .Given("A RunStore")
            .When(
                "saving some data with null as calculation function",
                data => data.StoreActual<string>(null!))
            .ThenNoError("no error")
            .DebugPrint()).Outcome.Should().Be(TestOutcome.Success);
    }

    [TestMethod]
    public void Given_ARunnerWithRunStore_When_StoringActualAsStringAndReadAsInt_Then_Throw()
    {
        this.Test(r => r
            .Given("A RunStore")
            .When("saving some data and read with another type", data =>
            {
                data.StoreActual("3");
                var check = data.ReadActual<int>();
                check.Should().NotBe(3);
            })
            .Then("exception occurs")
            .DebugPrint()
            .ExpectFail());
    }

    [TestMethod]
    public void Given_ARunnerWithRunStore_When_StoringMultipleStrings_Then_CannotReadByType()
    {
        this.Test(r => r
            .Given("A RunStore")
            .When("saving some strings", data =>
            {
                data.Store(item: "1");
                data.Store(item: "2");

                var check = data.Read<string>();
                check.Should().NotBe("1");
                check.Should().NotBe("2");
            })
            .Then("exception occurs")
            .DebugPrint()
            .ExpectFail());
    }

    [TestMethod]
    public void Given_SimpleFlow_When_ExceptionOccurs_Then_NoErrorFunctionShouldExpectFail()
        => this.Test(r => r
            .Given()
            .When(data => throw new InvalidDataException())
            .ThenNoError()
            .ExpectFail());

    [TestMethod]
    public void Given_SimpleDocumentedFlow_When_ExceptionOccurs_Then_NoErrorFunctionShouldExpectFail()
        => this.Test(r => r
            .Given("a simple flow")
            .When("exception occured", data => throw new InvalidDataException())
            .ThenNoError("no error")
            .ExpectFail());

    [TestMethod]
    public void Given_SimpleFlow_When_Working_Then_ShouldFailFunctionShouldExpectFail()
        => this.Test(r => r
            .Given()
            .When(data => { })
            .ThenShouldHaveFailed()
            .ExpectFail());

    [TestMethod]
    public void Given_SimpleFlow_When_Working_Then_ShouldFailWithFunctionShouldExpectFail()
        => this.Test(r => r
            .Given()
            .When(data => { })
            .ThenShouldHaveFailedWith<Exception>()
            .ExpectFail());

    [TestMethod]
    public void Given_SimpleDocumentedFlow_When_Working_Then_ShouldFailFunctionShouldExpectFail()
    => this.Test(r => r
        .Given("a simple flow")
        .When("exception occured", data => { })
        .ThenShouldHaveFailed("no error")
        .ExpectFail());
}
