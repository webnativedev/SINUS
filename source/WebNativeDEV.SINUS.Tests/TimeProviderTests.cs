﻿// <copyright file="TimeProviderTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        => this.Test()
            .Given("a time provider", data => data.StoreSut(new TimeProvider()))
            .When("Reading the seconds", data => data.StoreActual<TimeProvider>(sut => sut.GetCurrentSeconds()))
            .Then("Check if less than 60", (data) => Assert.IsTrue(data.ReadActual<int>() is >= 0 and <= 60))
            .Dispose();

    [TestMethod]
    public void Given_TimeProvider_When_CheckingToString_Then_TheyShouldReturnSomethingValid()
        => this.Test()
            .Given("a time provider", data => data.StoreSut(new TimeProvider()))
            .When("Reading the seconds", data => data.StoreActual<TimeProvider>(sut => sut.ToString()))
            .Then("Check if string is long", (data) => Assert.IsTrue(data.ReadActual<string>().Length > 16))
            .DebugPrint()
            .Dispose();

    [TestMethod]
    [ExpectedException(typeof(AssertFailedException))]
    public void Given_TimeController_When_DependencyTimeProviderIsNull_Then_Throw()
        => this.Test()
            .Given("a time controller", data => data.StoreSut(new TimeController(null!, null!)))
            .When("sut can not be created", data => data["not-available"] = 1)
            .Then("Check if controller exists", (data) => Assert.IsNotNull(data.ReadSut<TimeController>()))
            .DebugPrint()
            .Dispose();

    [TestMethod]
    public void Given_TimeControllerWithMockedSetup_When_GetSeconds_Then_MockedResultShouldBePresent()
        => this.Test()
            .Given("a time controller", data => {
                var timeProviderMock = new Mock<ITimeProvider>();
                timeProviderMock
                    .Setup(x => x.GetCurrentSeconds())
                    .Returns(1);
                ITimeProvider provider = timeProviderMock.Object;

                var loggerMock = new Mock<ILogger<TimeController>>();
                ILogger<TimeController> logger = loggerMock.Object;

                data.StoreSut(new TimeController(provider, logger));
            })
            .When("sut can be created", data => data.Actual = data.ReadSut<TimeController>().GetSeconds())
            .Then("Check if controller exists", (data) => data.ReadActual<int>().Should().Be(1))
            .DebugPrint()
            .Dispose();

}
