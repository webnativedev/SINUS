// <copyright file="EventTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Events;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
[BusinessRequirements(
    "EventHandling",
    "B001:Should get events")]
[TechnicalRequirements(
    "EventHandling")]
public class EventTests : TestBase
{
    [TestMethod]
    public void Given_ANormalTest_When_Running_Then_AllExecutionEventsShouldBeReceived()
    {
        var categories = new List<RunCategory>();

        this.Test(r => r
            .Listen<ExecutedEventBusEventArgs>((sender, data, e) => categories.Add(e.Output.RunCategory))
            .Given()
            .When(data => data["Test"] = "true")
            .Then());

        categories.Should().ContainInOrder(
            RunCategory.Given,
            RunCategory.When,
            RunCategory.Then,
            RunCategory.Close);
        categories.Should().HaveCount(4);
    }

    [TestMethod]
    public void Given_ANormalTest_When_RunningWithDebug_Then_AllExecutionEventsShouldBeReceived()
    {
        var categories = new List<RunCategory>();

        this.Test(r => r
            .Listen<ExecutedEventBusEventArgs>((sender, data, e) => categories.Add(e.Output.RunCategory))
            .Given()
            .When(data => data["Test"] = "true")
            .Then()
            .DebugPrint());

        categories.Should().ContainInOrder(
            RunCategory.Given,
            RunCategory.When,
            RunCategory.Then,
            RunCategory.Debug,
            RunCategory.Close);
        categories.Should().HaveCount(5);
    }

    [TestMethod]
    public void Given_AFailingTest_When_Fail_Then_CheckedExceptionsShouldBeVisibleInListenMethod()
    {
        var events = new List<ExceptionChangedEventBusEventArgs>();

        this.Test(r => r
            .Listen<ExceptionChangedEventBusEventArgs>(
                (sender, data, e) => events.Add(e))
            .Given()
            .When(data => throw new InvalidOperationException())
            .ThenShouldHaveFailedWith<InvalidOperationException>()
            .DebugPrint());

        events.Select(e => e.Exception.GetType().Name + "_" + e.IsChecked)
              .Should()
              .ContainInOrder(
                "InvalidOperationException_False",
                "InvalidOperationException_True");
        events.Should().HaveCount(2);
    }

    [TestMethod]
    public void Given_AFailingTest_When_FailInGiven_Then_CheckedExceptionsShouldBeVisibleInListenMethod()
    {
        var events = new List<ExceptionChangedEventBusEventArgs>();

        this.Test(r => r
            .Listen<ExceptionChangedEventBusEventArgs>(
                (sender, data, e) => events.Add(e))
            .Given(data => throw new InvalidOperationException())
            .When(data => throw new InvalidDataException())
            .ThenShouldHaveFailedWith<InvalidOperationException>()
            .DebugPrint());

        events.Select(e => e.Exception.GetType().Name + "_" + e.IsChecked)
              .Should()
              .ContainInOrder(
                "InvalidOperationException_False",
                "InvalidOperationException_True");
        events.Should().HaveCount(2);
    }

    [TestMethod]
    public void Given_ANormalTest_When_RunningWithDebug_Then_AllExecutionEventsShouldBeFilled()
    {
        var args = new List<ExecutedEventBusEventArgs>();

        this.Test(r => r
            .Listen<ExecutedEventBusEventArgs>((sender, data, e) => args.Add(e))
            .Given()
            .When(data => data["Test"] = "true")
            .Then()
            .DebugPrint());

        args.Should().ContainItemsAssignableTo<ExecutedEventBusEventArgs>();
    }
}
