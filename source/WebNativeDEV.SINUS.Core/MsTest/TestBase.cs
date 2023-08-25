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
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.Core.Ioc.Contracts;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
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
    protected void Test(Action<IBrowserRunner> action)
    {
        Ensure.NotNull(action);

        IBrowserRunner runner = this.Test();
        action.Invoke(runner);

        if(runner is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
