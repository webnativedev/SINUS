// <copyright file="TestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Events;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.Sut;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents an abstract test base that allows later unit tests to
/// access different convenience methods on top.
/// </summary>
[TestClass]
public abstract class TestBase
{
    static TestBase()
    {
        Setup();
    }

    /// <summary>
    /// Gets the IoC Container.
    /// </summary>
    public static IContainer Container { get; } = new Container();

    /// <summary>
    /// Gets or sets the TestContext injected by the framework.
    /// </summary>
    public TestContext? TestContext { get; set; }

    /// <summary>
    /// Gets the run directory where tests are executed.
    /// </summary>
    public string RunDir => this.TestContext?.TestRunDirectory ?? ".";

    /// <summary>
    /// Gets the run directory where tests are logging to.
    /// </summary>
    public string LogsDir => this.TestContext?.TestRunResultsDirectory ?? ".";

    /// <summary>
    /// Gets the name of the current test.
    /// </summary>
    public string TestName => this.TestContext?.TestName ?? "<unnamed>";

    /// <summary>
    /// Sets up the TestBase.
    /// </summary>
    /// <param name="setup">An action to register further services.</param>
    /// <param name="assemblyTestContext">TestContext injected from AssemblyInitialize.</param>
    protected static void Setup(Action<IContainer>? setup = null, TestContext? assemblyTestContext = null)
    {
        Container.Register<ILoggerFactory>(() =>
        {
            return Microsoft.Extensions.Logging.LoggerFactory.Create(
                builder =>
                {
                    builder.AddConsole(options =>
                    {
                        options.FormatterName = "SinusConsoleFormatter";
                    }).AddConsoleFormatter<SinusConsoleFormatter, ConsoleFormatterOptions>(options =>
                    {
                        options.IncludeScopes = true;
                    });
                });
        }).AsSingleton();
        Container.Register<IEventBus, EventBus>().AsSingleton();
        Container.Register<IWebDriverFactory>(() => new ChromeWebDriverFactory()).AsSingleton();
        Container.Register<IBrowserFactory>(() => new BrowserFactory()).AsSingleton();
        Container.Register<IExecutionEngine, ExecutionEngine>().AsSingleton();

        if (assemblyTestContext != null)
        {
            Container.Register<IAssemblyTestContext>(
                () => new AssemblyTestContext(assemblyTestContext)).AsSingleton();
        }

        setup?.Invoke(Container);
    }

    /// <summary>
    /// Tear down for the test base.
    /// </summary>
    protected static void TearDown()
    {
        Browser.PrintBrowserUsageStatistic();
        SinusWafUsageStatisticsManager.PrintWafUsageStatistic();
    }

    /// <summary>
    /// Creates a Runner object to run Tests on.
    /// </summary>
    /// <returns>An object of runner.</returns>
    protected IBrowserRunner Test() => new Runner(this);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected TestBaseResult Test(Action<IBrowserRunner> action)
    {
        Ensure.NotNull(action);

        IBrowserRunner runner = this.Test();
        action.Invoke(runner);
        runner.Dispose();

        return new TestBaseResult(true, this);
    }

    /// <summary>
    /// Registers an event handler.
    /// </summary>
    /// <typeparam name="TEventBusEventArgs">Type of the event args.</typeparam>
    /// <param name="handler">The event handler.</param>
    /// <returns>The current test base instance.</returns>
    protected TestBase Listen<TEventBusEventArgs>(Action<object, TEventBusEventArgs> handler)
        where TEventBusEventArgs : EventBusEventArgs
    {
        Container.Resolve<IEventBus>().Subscribe<TEventBusEventArgs>(
            (sender, e) => handler.Invoke(
                                sender,
                                (e as TEventBusEventArgs) ?? throw new InvalidCastException()));
        return this;
    }
}
