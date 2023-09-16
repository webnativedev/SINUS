// <copyright file="EventBusInternalsTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Internals.Events;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Events;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.Events.EventArguments;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

public class EventBusInternalsTests : TestBase
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
    public void Given_ANewTestBusDocumented_When_PublishingEvents_Then_TheyShouldBeReceived()
        => this.Test(r => r
            .GivenASimpleSystem("a new eventbus", new EventBus())
            .When<IEventBus>("publish and subscribe", (sut, data) =>
            {
                sut.Subscribe<EventBusEventArgs>((sender, e) =>
                {
                    e.Should().NotBeNull();
                    data.Actual = true;
                });

                sut.Publish<EventBusEventArgs>(this, EventBusEventArgs.Empty);
            })
            .Then("subscribe should be fired", data => data.ReadActual<bool>().Should().BeTrue()));
}
