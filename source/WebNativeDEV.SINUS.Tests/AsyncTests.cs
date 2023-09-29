// <copyright file="AsyncTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using System.Threading;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable SA1204 // Static elements should appear before instance elements

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
    public void Given_EventDrivenPattern_When_HandleARequest_Then_ResponsesShouldBeReceived()
    {
        // see: https://masstransit.io/documentation/concepts/testing
        // In most cases, developers want to test that
        // * message consumption is successful,
        // * consumer behavior is as expected, and
        // * messages are published and/or sent.
        this.Test(r => r
            .GivenASystem(data =>
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

    [TestMethod]
    public void Given_ALotOfAsyncTasks_When_StoringSomeValuesInTasks_Then_AllAsyncOperationsShouldLeadToAnObject()
    {
        this.Test(r => r
            .Given(data => data.Sut = (string id) => this.Given_AsyncTasks_When_StoringSomeValuesInTasks_Then_AllAsyncOperationsShouldLeadToAnObject($"RunIndex{id}"))
            .When(data =>
            {
                int currentValue = 1;
                var tasks = new List<Task>();
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        string id = (currentValue++).ToString("00");

                        data[$"created {id}"] = DateTime.Now.ToLongTimeString();

                        try
                        {
                            data[$"started Process: {id}"] = DateTime.Now.ToLongTimeString();
                            (data.Sut as Action<string> ?? throw new InvalidDataException()).Invoke(id);
                            data[$"stopped Process: {id}"] = DateTime.Now.ToLongTimeString();
                        }
                        catch (Exception exc)
                        {
                            throw new InvalidOperationException($"Execution failed in {id}: " + exc.Message, exc);
                        }
                    }));
                }

                data.Actual = tasks.ToArray();
            })
            .Then(data =>
            {
                var tasks = data.Actual as Task[] ?? throw new InvalidDataException();
                Task.WaitAll(tasks, 600_000);
            })
            .DebugPrint()).Should().BeSuccessful();
    }

    public static string DefaultDataDisplayName(MethodInfo methodInfo, object[] data)
        => TestNamingConventionManager.DynamicDataDisplayNameAddValueFromLastArgument(methodInfo, data);

    public static IEnumerable<object?[]> NoValues => new[] { new object?[] { string.Empty }, };

    [TestMethod]
    [DynamicData(
        nameof(NoValues),
        DynamicDataDisplayName = nameof(DefaultDataDisplayName))]
    public void Given_AsyncTasks_When_StoringSomeValuesInTasks_Then_AllAsyncOperationsShouldLeadToAnObject(string scenario)
    {
        this.Test(scenario, r => r
            .Given()
            .When(data =>
            {
                for (int i = 0; i < 50; i++)
                {
                    int index = i;
                    Task.Run(() =>
                    {
                        data.Store(new Tuple<int, Guid>(index, Guid.NewGuid()));
                    });
                }
            })
            .Then(data =>
            {
                data.WaitUntil(store => store.Count(item => item.Value is Tuple<int, Guid>) == 100, 60_000);
            })
            .Debug(data =>
            {
                if (string.IsNullOrWhiteSpace(scenario))
                {
                    data.PrintStore();
                }
            })).Should().BeSuccessful();
    }

    [TestMethod]
    public void Given_AsyncMethodCalls_When_StoringSomeValues_Then_AllAsyncOperationsShouldLeadToAnObject()
    {
        this.Test(r => r
            .Given()
            .When(async data =>
            {
                data.Actual = await GetValue();
            })
            .Then(data =>
            {
                data.Should().ActualBe(0815);
            })
            .DebugPrint());
    }

    [TestMethod]
    public void Given_AnAsyncCreatedSut_When_Using_Then_ItShouldBeCreatedProperly()
    {
        this.Test(r => r
            .GivenASystemAsync(async () => await GetValue())
            .When<object>(async (sut, data) => data.Actual = ((int)sut) + await GetValue())
            .Then(async data => data.Actual.Should().Be(await Task.FromResult(1630))))
            .Should().BeSuccessful();
    }

    [TestMethod]
    public void Given_AnAsyncCreatedSutWithRunStore_When_Using_Then_ItShouldBeCreatedProperly()
    {
        this.Test(r => r
            .GivenASystemAsync(async (data) => data.KeyActual + (await GetValue()).ToString())
            .When<object>(async (sut, data) => data.Actual = ((string)sut) + (await GetValue()).ToString())
            .Then(async data => ((string?)data.Actual).Should().Contain(await Task.FromResult("815")))
            .DebugPrint())
            .Should().BeSuccessful();
    }

    [TestMethod]
    public void Given_AnAsyncCreatedAndDescribedSut_When_Using_Then_ItShouldBeCreatedProperly()
    {
        this.Test(r => r
            .GivenASystemAsync("an async created integer as sut", async () => await GetValue())
            .When<object>("adding sut and an integer from async method", async (sut, data) => data.Actual = ((int)sut) + await GetValue())
            .Then("result comparison async should work", async data => data.Actual.Should().Be(await Task.FromResult(1630))))
            .Should().BeSuccessful();
    }

    private static async Task<int> GetValue()
    {
        var t = Task.FromResult(0815);

        Task.WaitAll(Task.Delay(200), t);

        return await t;
    }
}
