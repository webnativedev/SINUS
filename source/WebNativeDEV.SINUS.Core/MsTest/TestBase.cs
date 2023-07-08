// <copyright file="TestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.MsTest.Chrome;

/// <summary>
/// Represents an abstract test base that allows later unit tests to
/// access different convenience methods on top.
/// </summary>
[TestClass]
public abstract class TestBase
{
    private static TestContext? testContextAssembly;
    private static TestContext? testContextClass;
    private static ILoggerFactory? defaultLoggerFactory;

    private ILoggerFactory? loggerFactory;
    private ILogger? logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    protected TestBase()
    {
    }

    /// <summary>
    /// Gets or sets the logger factory setup to create logger instances.
    /// </summary>
    public static ILoggerFactory DefaultLoggerFactory
    {
        get => defaultLoggerFactory ??= Microsoft.Extensions.Logging.LoggerFactory.Create(
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
        set => defaultLoggerFactory = value;
    }

    /// <summary>
    /// Gets a logger factory to create a logger object.
    /// </summary>
    public ILoggerFactory LoggerFactory => this.loggerFactory ??= DefaultLoggerFactory;

    /// <summary>
    /// Gets the run directory where tests are executed.
    /// </summary>
    public string RunDir
    {
        get
        {
            this.Logger.LogDebug("RunDir accessed");
            return testContextAssembly?.TestRunDirectory ?? ".";
        }
    }

    /// <summary>
    /// Gets the run directory where tests are logging to.
    /// </summary>
    public string LogsDir
    {
        get
        {
            this.Logger.LogDebug("LogDir accessed");
            return testContextAssembly?.TestLogsDir ?? ".";
        }
    }

    /// <summary>
    /// Gets the name of the current test.
    /// </summary>
    public string TestName
    {
        get
        {
            this.Logger.LogDebug("TestName accessed");
            return testContextClass?.TestName ?? "<unnamed>";
        }
    }

    /// <summary>
    /// Gets a logger instance based on <see cref="CreateLogger"/>.
    /// </summary>
    private ILogger Logger => this.logger ??= this.CreateLogger();

    /// <summary>
    /// Method that is called indirectly in assembly startup that is used by the MS-Test Framework.
    /// </summary>
    /// <param name="testContext">The current context of the test execution (assembly level).</param>
    protected static void StoreAssemblyTestContext(TestContext testContext)
    {
        testContextAssembly = testContext;
    }

    /// <summary>
    /// Method that is called indirectly in class initialization that is used by the MS-Test Framework.
    /// </summary>
    /// <param name="testContext">The current context of the test execution (class level).</param>
    /// <returns>Async context.</returns>
    protected static async Task StoreClassTestContext(TestContext testContext)
    {
        testContextClass = testContext;
        await Task.FromResult(testContext).ConfigureAwait(false);
    }

    protected static void PrintBrowserUsageStatistic()
    {
        var logger = DefaultLoggerFactory.CreateLogger<TestBase>();
        logger.LogInformation("+--------------------------------");
        logger.LogInformation("| Tests Including Browsers");
        foreach(var id in Browser.TestsIncludingBrowsers)
        {
            var disposedInfo = Browser.TestsDisposingBrowsers.Contains(id)
                                    ? "disposed"
                                    : "leak";
            logger.LogInformation("| {Id} ({DisposedInfo})", id, disposedInfo);
        }
        logger.LogInformation("+--------------------------------");
    }

    /// <summary>
    /// Creates a Runner object to run Tests on.
    /// </summary>
    /// <returns>An object of runner.</returns>
    protected IRunner Test() => new Runner(this);

    /// <summary>
    /// Creates a logger object.
    /// </summary>
    /// <returns>A reference to a newly created logger.</returns>
    private ILogger CreateLogger()
        => this.LoggerFactory.CreateLogger(this.GetType());
}
