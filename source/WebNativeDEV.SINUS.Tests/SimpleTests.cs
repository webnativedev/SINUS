// <copyright file="SimpleTests.cs" company="WebNativeDEV">
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
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.SystemUnderTest;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Mock;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class SimpleTests : TestBase
{
    [TestMethod]
    public void Given_TheMockTimeProvider_When_GettingTheCurrentSeconds_Then_ValueEquals59_AsTripleAPattern()
    {
        // Arrange
        ITimeProvider provider = new MockTimeProvider();

        // Act
        var actual = provider.GetCurrentSeconds();

        // Assert
        Assert.AreEqual(expected: 59, actual);
    }

    [TestMethod]
    public void Given_TheMockTimeProvider_When_GettingTheCurrentSeconds_Then_ValueEquals59_AsGherkinPreparation()
    {
        this.Test()
            .Given("The Mock Time Provider")
            .When("Ask for the current seconds")
            .Then("Check for the mocked value 59")
            .Dispose();
    }

    [TestMethod]
    public void Given_TheMockTimeProvider_When_GettingTheCurrentSeconds_Then_ValueEquals59_AsGherkin()
    {
        this.Test()
            .Given(
                "The Mock Time Provider",
                (data) => data.StoreSut(new MockTimeProvider()))
            .When(
                "Ask for the current seconds",
                (data) => data.StoreActual(data.ReadSut<MockTimeProvider>().GetCurrentSeconds()))
            .Then(
                "Check for the mocked value 59",
                data => Assert.That.AreEqualToActual(data, 59))
            .Debug(data => data.Print())
            .Dispose();
    }

    [TestMethod]
    public void Given_TheMockTimeProviderViaHttp_When_GettingTheCurrentSeconds_Then_ValueEquals59_AsGherkin()
    {
        this.Test()
            .GivenASystem<Program>("The Mock Time Provider via http")
            .When(
                "Ask for the current seconds",
                (client, data) => data.StoreActual(client.GetStringAsync("/time/sec").GetAwaiter().GetResult()))
            .Then(
                "Check for the mocked value 59",
                data => Assert.That.AreEqualToActual(data, "59"))
            .DebugPrint()
            .Dispose();
    }
}
