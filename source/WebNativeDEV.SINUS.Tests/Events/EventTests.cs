// <copyright file="EventTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Events;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
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

        this.Listen<ExecutedEventBusEventArgs>((sender, e) => categories.Add(e.Output.RunCategory));
        this.Test(r => r
            .Given()
            .When(data => data["Test"] = "true")
            .Then());

        categories.Should().ContainInOrder(
            RunCategory.Given,
            RunCategory.When,
            RunCategory.Then,
            RunCategory.Dispose);
        categories.Count.Should().Be(4);
    }

    [TestMethod]
    public void Given_ANormalTest_When_RunningWithDebug_Then_AllExecutionEventsShouldBeReceived()
    {
        var categories = new List<RunCategory>();

        this.Listen<ExecutedEventBusEventArgs>((sender, e) => categories.Add(e.Output.RunCategory));
        this.Test(r => r
            .Given()
            .When(data => data["Test"] = "true")
            .Then()
            .DebugPrint());

        categories.Should().ContainInOrder(
            RunCategory.Given,
            RunCategory.When,
            RunCategory.Then,
            RunCategory.Debug,
            RunCategory.Dispose);
        categories.Count.Should().Be(5);
    }
}
