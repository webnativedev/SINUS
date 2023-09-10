﻿// <copyright file="EventBusTests.cs" company="WebNativeDEV">
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
using WebNativeDEV.SINUS.Core.Events;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
[BusinessRequirements(
    "EventHandling",
    "B001:EventBus exists")]
[TechnicalRequirements(
    "EventHandling")]
public class EventBusTests : TestBase
{
    [TestMethod]
    public void Given_ANewTestBus_When_PublishingEvents_Then_TheyShouldBeReceived()
        => this.Test(r => r
            .GivenASimpleSystem(new EventBus())
            .When<IEventBus>((sut, data) =>
            {
                sut.Subscribe<EventBusEventArgs>((sender, e) =>
                {
                    e.Should().NotBeNull();
                    data.Actual = true;
                });

                sut.Publish<EventBusEventArgs>(this, EventBusEventArgs.Empty);
            })
            .Then(data => data.ReadActual<bool>().Should().BeTrue()));

    [TestMethod]
    public void Given_InternalBus_When_PublishingEvents_Then_TheyShouldBeReceivedInLine()
    => this.Test(r => r
        .Listen<RunStoreDataStoredEventBusEventArgs>(
            (sender, data, e) => data.Actual = data.Sut,
            (sender, data, e) => e.Key == data.KeySut)
        .Given()
        .When(data => data.Sut = 1)
        .Then(data => data.Actual.Should().Be(1)));

    [TestMethod]
    public void Given_InternalBus_When_PublishingEvents_Then_TheyShouldBeReceivedInLineDescribed()
    => this.Test(r => r
        .Listen<RunStoreDataStoredEventBusEventArgs>(
            "stored data",
            (sender, data, e) => data.StoreActual(e),
            (sender, data, e) => e.Key == data.KeySut)
        .Given()
        .When(data => data.Sut = 1)
        .Then(data => data.Actual.Should().NotBeNull()));
}
