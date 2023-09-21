// <copyright file="TimeProviderTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Sut;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.SystemUnderTest.Controllers;
using WebNativeDEV.SINUS.SystemUnderTest.Services;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class TimeProviderTests : TestBase
{
    [TestMethod]
    public void Given_TimeProvider_When_CheckingSeconds_Then_TheyShouldBeLessThan60()
        => this.Test(r => r
            .Given("a time provider", data => data.StoreSut(new TimeProvider()))
            .When("Reading the seconds", data => data.StoreActual<TimeProvider>(sut => sut.GetCurrentSeconds()))
            .Then("Check if less than 60", (data) => Assert.IsTrue(data.ReadActual<int>() is >= 0 and <= 60)));

    [TestMethod]
    public void Given_TimeProvider_When_CheckingToString_Then_TheyShouldReturnSomethingValid()
        => this.Test(r => r
            .Given("a time provider", data => data.StoreSut(new TimeProvider()))
            .When("Reading the seconds", data => data.StoreActual<TimeProvider>(sut => sut.ToString()))
            .Then("Check if string is long", (data) => Assert.IsTrue(data.ReadActual<string>().Length > 16))
            .DebugPrint());

    [TestMethod]
    public void Given_TimeController_When_DependencyTimeProviderIsNull_Then_Throw()
        => this.Test(r => r
            .Given("a time controller", data => data.StoreSut(new TimeController(null!, null!)))
            .When("sut can not be created", data => data["not-available"] = 1)
            .Then("Check if controller exists", (data) => Assert.IsNotNull(data.ReadSut<TimeController>()))
            .DebugPrint()
            .ExpectFail());

    [TestMethod]
    public void Given_TimeControllerWithMockedSetup_When_GetSeconds_Then_MockedResultShouldBePresent()
        => this.Test(r => r
            .Given("a time controller", data =>
            {
                var timeProviderMock = Substitute.For<ITimeProvider>();
                timeProviderMock.GetCurrentSeconds().Returns(1);
                ITimeProvider provider = timeProviderMock;

                var loggerMock = Substitute.For<ILogger<TimeController>>();
                ILogger<TimeController> logger = loggerMock;

                data.StoreSut(new TimeController(provider, logger));
            })
            .When("sut can be created", data => data.Actual = data.ReadSut<TimeController>().GetSeconds())
            .Then("Check if controller exists", (data) => data.ReadActual<int>().Should().Be(1))
            .DebugPrint());

    [TestMethod]
    public void Given_TimeControllerWithMockedSetupAsSut_When_GetSeconds_Then_MockedResultShouldBeSetRight()
        => this.Test(r => r
            .GivenASimpleSystem(() =>
            {
                var timeProviderMock = Substitute.For<ITimeProvider>();
                timeProviderMock.GetCurrentSeconds().Returns(1);
                return timeProviderMock;
            })
            .When<ITimeProvider>((sut, data) => data.Actual = sut.GetCurrentSeconds())
            .Then(data => data.Should().ActualBe(1)));

    [TestMethod]
    public void Given_TimeControllerWithMockedSetupAsSutDocumented_When_GetSeconds_Then_MockedResultShouldBeSetRight()
        => this.Test(r => r
            .GivenASimpleSystem("a mocked time provider", () =>
            {
                var timeProviderMock = Substitute.For<ITimeProvider>();
                timeProviderMock.GetCurrentSeconds().Returns(1);
                return timeProviderMock;
            })
            .When<ITimeProvider>("called get seconds", (sut, data) => data.Actual = sut.GetCurrentSeconds())
            .Then("mocked value should be present", data => data.Should().ActualBe(1)));
}
