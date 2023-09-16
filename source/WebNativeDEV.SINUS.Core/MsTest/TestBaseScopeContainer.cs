// <copyright file="TestBaseScopeContainer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Events;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.Sut.Contracts;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CA1822 // Member mark as static

/// <summary>
/// Holds all the references that are scoped to a single test.
/// </summary>
public class TestBaseScopeContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestBaseScopeContainer"/> class.
    /// </summary>
    /// <param name="testBase">Reference to a test.</param>
    public TestBaseScopeContainer(TestBase testBase)
    {
        this.TestBase = testBase;
        this.EventBus = new EventBus();
        this.DataBag = new RunStore(this);
        this.NamingConventionManager = new TestNamingConventionManager(this);
        this.IsPreparedOnly = false;
        this.Exceptions = new ExceptionStore();

        var stackTrace = new StackTrace(1, true);
        MethodBase? methodBase = null;
        foreach (var frame in stackTrace.GetFrames())
        {
            var method = frame.GetMethod();
            if(method?.GetCustomAttributes(typeof(TestMethodAttribute), true)?.Any() ?? false)
            {
                methodBase = method;
                break;
            }
        }

        this.Method = methodBase
            ?? throw new InvalidOperationException("test method could not be evaluated");

        this.Runner = new Runner(this);

        this.TestBaseUsageStatisticsManager.Register(this);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the test is only a placeholder for later or not.
    /// </summary>
    public bool IsPreparedOnly { get; set; }

    /// <summary>
    /// Gets the exceptions that occured during the execution.
    /// </summary>
    public IExceptionStore Exceptions { get; }

    /// <summary>
    /// Gets the assembly context.
    /// </summary>
    public TestContext? AssemblyTestContext => TestBaseSingletonContainer.AssemblyTestContext;

    /// <summary>
    /// Gets the usage statistic manager.
    /// </summary>
    public ITestBaseUsageStatisticsManager TestBaseUsageStatisticsManager => TestBaseSingletonContainer.TestBaseUsageStatisticsManager;

    /// <summary>
    /// Gets the reflection object for the method.
    /// </summary>
    public MethodBase Method { get; private set; }

    /// <summary>
    /// Gets a reference to the test base.
    /// </summary>
    internal TestBase TestBase { get; }

    /// <summary>
    /// Gets or sets the web application factory.
    /// </summary>
    internal ISinusWebApplicationFactory? WebApplicationFactory { get; set; }

    /// <summary>
    /// Gets or sets the created browser.
    /// </summary>
    internal IBrowser? Browser { get; set; }

    /// <summary>
    /// Gets or sets the HttpClient or throws if it does not exist.
    /// </summary>
    internal HttpClient? HttpClient { get; set; }

    /// <summary>
    /// Gets the event bus.
    /// </summary>
    internal IEventBus EventBus { get; private set; }

    /// <summary>
    /// Gets the data store.
    /// </summary>
    internal IRunStore DataBag { get; }

    /// <summary>
    /// Gets the naming convention manager.
    /// </summary>
    internal ITestNamingConventionManager NamingConventionManager { get; }

    /// <summary>
    /// Gets the logger factory.
    /// </summary>
    internal ILoggerFactory LoggerFactory => TestBaseSingletonContainer.LoggerFactory;

    /// <summary>
    /// Gets the web driver factory.
    /// </summary>
    internal IWebDriverFactory WebDriverFactory => TestBaseSingletonContainer.WebDriverFactory;

    /// <summary>
    /// Gets the browser factory.
    /// </summary>
    internal IBrowserFactory BrowserFactory => TestBaseSingletonContainer.BrowserFactory;

    /// <summary>
    /// Gets or sets the runner.
    /// </summary>
    internal IBrowserRunner Runner { get; set; }

    /// <summary>
    /// Gets the execution engine.
    /// </summary>
    internal IExecutionEngine ExecutionEngine => TestBaseSingletonContainer.ExecutionEngine;

    /// <summary>
    /// Creates a logger for type t.
    /// </summary>
    /// <typeparam name="T">The type as identifier.</typeparam>
    /// <returns>A logger instance.</returns>
    internal ILogger CreateLogger<T>()
        => TestBaseSingletonContainer.CreateLogger<T>();

    /// <summary>
    /// Shuts down all network components and sets them to null.
    /// </summary>
    internal void Shutdown()
    {
        this.HttpClient?.CancelPendingRequests();
        this.HttpClient?.Dispose();
        this.HttpClient = null;

        this.Browser?.Dispose();
        this.Browser = null;

        this.WebApplicationFactory?.CloseCreatedHost();
        this.WebApplicationFactory?.Dispose(); // consider DisposeAsync()
        this.WebApplicationFactory = null;

        this.DataBag.DisposeAllDisposables();

#pragma warning disable S1215 // "GC.Collect" should not be called
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();
#pragma warning restore S1215 // "GC.Collect" should not be called
    }
}
