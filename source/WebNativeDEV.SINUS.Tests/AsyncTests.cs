// <copyright file="AsyncTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Threading;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1201 // Elements should appear in the correct order

[TestClass]
public class AsyncTests : TestBase
{
    public interface IProducer<T>
    {
        void Produce(T message);
    }

    public interface IConsumer<T>
    {
        T Consume();
    }

    public class TestingEvent
    {
        public TestingEvent(string? message = null)
        {
            this.Message = message;
        }

        public string? Message { get; set; }

        public override string ToString()
        {
            return this.Message ?? "Message: <null>";
        }
    }

    public class TestedEvent
    {
        public TestedEvent(string? message = null)
        {
            this.Message = message;
        }

        public string? Message { get; set; }

        public override string ToString()
        {
            return this.Message ?? "Message: <null>";
        }
    }

    public class TestHandler
    {
        public TestHandler(IProducer<TestedEvent> producer, IConsumer<TestingEvent> consumer)
        {
            this.Producer = producer;
            this.Consumer = consumer;
        }

        public IProducer<TestedEvent> Producer { get; }

        public IConsumer<TestingEvent> Consumer { get; }

        public void Handle()
        {
            TestingEvent? message;
            while ((message = this.Consumer.Consume()) != null)
            {
                Thread.Sleep(500);
                this.Producer.Produce(new TestedEvent() { Message = "Re: 1 - " + message.Message });
                Thread.Sleep(500);
                this.Producer.Produce(new TestedEvent() { Message = "Re: 2 - " + message.Message });
                Thread.Sleep(500);
                Thread.Sleep(500);
                this.Producer.Produce(new TestedEvent() { Message = "Re: 3 - " + message.Message });
            }
        }
    }

    public class ContainerEventArgs : EventArgs
    {
        public ContainerEventArgs(params object[] data)
        {
            this.Data = data;
        }

        public object[] Data { get; set; }
    }

    [TestMethod]
    public void Given_TestBusFromMassTransit_When_ExecutingARequest_Then_NoErrorShouldHappen()
    {
        // see: https://masstransit.io/documentation/concepts/testing
        // In most cases, developers want to test that
        // * message consumption is successful,
        // * consumer behavior is as expected, and
        // * messages are published and/or sent.
        this.Test(r => r
            .GivenASimpleSystem(data =>
            {
                var consumer = Substitute.For<IConsumer<TestingEvent>>();
                var producer = Substitute.For<IProducer<TestedEvent>>();

                consumer.Configure().Consume().Returns(
                        new TestingEvent("Message 1"),
                        new TestingEvent("Message 2"),
                        new TestingEvent("Message 3"),
                        null);

                producer.Configure()
                    .WhenForAnyArgs(x => x.Produce(default!))
                    .Do(info => data.Store(info.Arg<TestedEvent>()));

                data["producer"] = producer;
                data["consumer"] = consumer;

                return new TestHandler(
                        producer,
                        consumer);
            })
            .When<TestHandler>((sut, data) =>
            {
                sut.Handle();
            })
            .Then(data =>
            {
                data.Count(x => x.Value is TestedEvent).Should().Be(9);
            })).Should().BeSuccessful();
    }
}
