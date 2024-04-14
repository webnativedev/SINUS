// <copyright file="SimpleTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.MsTest;
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
    public void Given_TheMockTimeProvider_When_GettingTheCurrentSeconds_Then_ValueEquals59AsTripleAPattern()
    {
        // Init
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.Register(this);

        // Arrange
        var provider = new MockTimeProvider();

        // Act
        var actual = provider.GetCurrentSeconds();

        // Assert
        actual.Should().Be(59);
    }

    [TestMethod]
    public void Given_TheMockTimeProvider_When_GettingTheCurrentSeconds_Then_ValueEquals59AsGherkinPreparation()
    {
        this.Test(r => r
            .Given("The Mock Time Provider")
            .When("Ask for the current seconds")
            .Then("Check for the mocked value 59")
            .ExpectInconclusive());
    }

    [TestMethod]
    public void Given_TheMockTimeProvider_When_GettingTheCurrentSeconds_Then_ValueEquals59AsGherkin()
    {
        this.Test(r => r
            .Given(
                "The Mock Time Provider",
                (data) => data.StoreSut(new MockTimeProvider()))
            .When(
                "Ask for the current seconds",
                (data) => data.StoreActual(data.ReadSut<MockTimeProvider>().GetCurrentSeconds()))
            .Then("Check for the mocked value 59", data => data.Should().ActualBe(59))
            .Debug(data => data.PrintStore()));
    }

    [TestMethod]
    public void Given_TheMockTimeProviderViaHttp_When_GettingTheCurrentSeconds_Then_ValueEquals59AsGherkin()
    {
        this.Test(r => r
            .GivenASystem<Program>("The Mock Time Provider via http")
            .When(
                "Ask for the current seconds",
                (client, data) => data.StoreActual(client.GetStringAsync("/time/sec").GetAwaiter().GetResult()))
            .Then(
                "Check for the mocked value 59",
                data => data.Should().ActualBe("59"))
            .DebugPrint());
    }
}
