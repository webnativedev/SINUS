// <copyright file="TestBaseSingletonContainer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// IoC handling for the test base.
/// </summary>
public static class TestBaseSingletonContainer
{
    static TestBaseSingletonContainer()
    {
        LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
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

        WebDriverFactory = new ChromeWebDriverFactory();
        BrowserFactory = new BrowserFactory();
        ExecutionEngine = new ExecutionEngine();
        TestBaseUsageStatisticsManager = new TestBaseUsageStatisticsManager();
        TestBaseStrategy = new SimpleTestBaseStrategy();
    }

    /// <summary>
    /// Gets or sets the logger factory.
    /// </summary>
    public static ILoggerFactory LoggerFactory { get; set; }

    /// <summary>
    /// Gets or sets the web driver factory.
    /// </summary>
    public static IWebDriverFactory WebDriverFactory { get; set; }

    /// <summary>
    /// Gets or sets the browser factory.
    /// </summary>
    public static IBrowserFactory BrowserFactory { get; set; }

    /// <summary>
    /// Gets or sets the assembly context.
    /// </summary>
    public static TestContext? AssemblyTestContext { get; set; }

    /// <summary>
    /// Gets or sets the usage statistics manager for tests.
    /// </summary>
    public static ITestBaseUsageStatisticsManager TestBaseUsageStatisticsManager { get; set; }

    /// <summary>
    /// Gets or sets the test base strategy.
    /// </summary>
    public static ITestBaseStrategy TestBaseStrategy { get; set; }

    /// <summary>
    /// Gets or sets the execution engine.
    /// </summary>
    internal static IExecutionEngine ExecutionEngine { get; set; }

    /// <summary>
    /// Creates a logger for type t.
    /// </summary>
    /// <typeparam name="T">The type as identifier.</typeparam>
    /// <returns>A logger instance.</returns>
    public static ILogger CreateLogger<T>()
    {
        return LoggerFactory.CreateLogger(typeof(T));
    }
}
