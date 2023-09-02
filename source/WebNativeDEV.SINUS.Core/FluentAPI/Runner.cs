// <copyright file="Runner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Extensions;
using WebNativeDEV.SINUS.Core.Sut;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Base Class for Runners.
/// </summary>
internal sealed partial class Runner : IBrowserRunner,
    IGiven, IGivenWithSut, IGivenBrowser,
    IWhenBrowser,
    IThenBrowser
{
    private const string DefaultEndpoint = "https://localhost:10001";

    private readonly IBrowserFactory browserFactory;
    private readonly IExecutionEngine executionEngine;
    private readonly ILogger logger;

    private bool disposedValue;

    private ISinusWebApplicationFactory? webApplicationFactory;
    private IBrowser? browser;

    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    /// <param name="testBase">Reference to the test base creating the runner.</param>
    public Runner(TestBase testBase)
    {
        this.TestBase = testBase;
        this.logger = TestBase.Container.Resolve<ILoggerFactory>().CreateLogger<Runner>();
        this.executionEngine = TestBase.Container.Resolve<IExecutionEngine>();
        this.browserFactory = TestBase.Container.Resolve<IBrowserFactory>();
        this.NamingConventionManager = new TestNamingConventionManager(testBase.TestName);

        this.logger.LogDebug("Created a log for base-runner");
    }

    /// <summary>
    /// Gets a value indicating whether the test is only a placeholder for later or not.
    /// </summary>
    public bool IsPreparedOnly { get; private set; }

    /// <summary>
    /// Gets the exceptions that occured during the execution.
    /// </summary>
    public List<(RunCategory, Exception)> Exceptions { get; } = new();

    /// <summary>
    /// Gets the HttpClient or throws if it does not exist.
    /// </summary>
    public HttpClient? HttpClient { get; private set; }

    /// <summary>
    /// Gets the current state of the test run.
    /// </summary>
    public RunStore DataBag { get; } = new();

    /// <summary>
    /// Gets the reference to the TestBase that creates the runner.
    /// </summary>
    public TestBase TestBase { get; }

    /// <summary>
    /// Gets the reference to the naming convention manager.
    /// </summary>
    public TestNamingConventionManager NamingConventionManager { get; }

    /// <summary>
    /// Disposes the object as defined in IDisposable.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private Runner Run(
            RunCategory runCategory,
            string? description = null,
            bool runActions = true,
            Action? action = null,
            Action<ExecutionSetupParameters>? setupAction = null,
            IList<Action?>? actions = null,
            bool createSut = false,
            Type? sutType = null,
            string? sutEndpoint = null)
    {
        var output = this.executionEngine.Run(new ExecutionParameter()
        {
            // Dependencies
            TestBase = this.TestBase,
            Runner = this,
            Namings = this.NamingConventionManager,

            // Meta information
            RunCategory = runCategory,
            Description = description,
            ExceptionsCount = this.Exceptions.Count,

            // Actual action
            RunActions = runActions && ((actions?.Any() ?? false) || action != null || setupAction != null),
            SetupActions = setupAction != null
                    ? new List<Action<ExecutionSetupParameters>?>() { setupAction }
                    : new List<Action<ExecutionSetupParameters>?>() { },
            Actions = actions ?? (
                action != null
                    ? new List<Action?>() { action }
                    : new List<Action?>()),

            // System under Test parameter
            CreateSut = createSut,
            SutType = sutType,
            SutEndpoint = sutEndpoint,
        });

        this.HttpClient = output.HttpClient;
        this.webApplicationFactory = output.WebApplicationFactory;
        this.Exceptions.AddRange(output.Exceptions.Select(exc => (runCategory, exc)));
        this.IsPreparedOnly = this.IsPreparedOnly ||
            (output.IsPreparedOnly && runCategory == RunCategory.When);

        return this;
    }

    private IList<Action?>? InvokeAction(Action<RunStore>[] actions)
    {
        if (actions == null)
        {
            return null;
        }

        List<Action?> pureActions = new();
        actions
            .ToList()
            .ForEach(action =>
                {
                    var pureAction = this.InvokeAction(action);
                    if (pureAction != null)
                    {
                        pureActions.Add(pureAction);
                    }
                });

        return pureActions;
    }

    private IList<Action?>? InvokeAction(Action<IBrowser, RunStore>[] actions)
    {
        if (actions == null)
        {
            return null;
        }

        List<Action?> pureAction = new();
        actions
            .ToList()
            .ForEach(action => pureAction.Add(this.InvokeAction(action)));

        return pureAction;
    }

    private Action? InvokeAction(Action<RunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action.Invoke(this.DataBag);
    }

    private Action? InvokeAction(Action<HttpClient, RunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action?.Invoke(
            this.HttpClient ?? throw new InvalidOperationException("HttpClient was not set in an operation"),
            this.DataBag);
    }

    private Action? InvokeAction(Action<IBrowser, RunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action?.Invoke(
                this.browser ?? throw new InvalidOperationException("no browser created"),
                this.DataBag);
    }

    private Action InvokeCreateBrowserForDefaultSutAction(string? browserPageToStart, BrowserFactoryOptions? options)
    => this.InvokeCreateBrowserAction(
        url: new Uri(DefaultEndpoint + (browserPageToStart ?? string.Empty)),
        options: options);

    private Action InvokeCreateBrowserForDefaultSutAction(string? browserPageToStart, string? humanReadablePageName, BrowserFactoryOptions? options)
        => this.InvokeCreateBrowserAction(
            url: new Uri(DefaultEndpoint + (browserPageToStart ?? string.Empty)),
            humanReadablePageName: humanReadablePageName,
            options: options);

    private Action<ExecutionSetupParameters> InvokeCreateBrowserForRandomSutAction(string? browserPageToStart, BrowserFactoryOptions? options)
        => (param) => this.InvokeCreateBrowserAction(
            url: new Uri(param.Endpoint + (browserPageToStart ?? string.Empty)),
            options: options)?.Invoke();

    private Action<ExecutionSetupParameters> InvokeCreateBrowserForRandomSutAction(string? browserPageToStart, string? humanReadablePageName, BrowserFactoryOptions? options)
        => (param) => this.InvokeCreateBrowserAction(
                url: new Uri(param.Endpoint + (browserPageToStart ?? string.Empty)),
                humanReadablePageName: humanReadablePageName,
                options: options)?.Invoke();

    private Action InvokeCreateBrowserAction(Uri url, BrowserFactoryOptions? options)
        => this.InvokeCreateBrowserAction(url, null, options);

    private Action InvokeCreateBrowserAction(Uri url, string? humanReadablePageName, BrowserFactoryOptions? options)
    {
        return () => this.browser = this.browserFactory.CreateBrowser(
            url,
            this.TestBase,
            humanReadablePageName,
            options);
    }

    /// <summary>
    /// Implementation of the disposal as called by IDisposable.Dispose.
    /// </summary>
    /// <param name="disposing">True if called by Dispose; False if called by Destructor.</param>
    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Run(
                    runCategory: RunCategory.Dispose,
                    action: () =>
                    {
                        this.HttpClient?.CancelPendingRequests();
                        this.HttpClient?.Dispose();
                        this.HttpClient = null;

                        this.browser?.Dispose();
                        this.browser = null;

                        this.webApplicationFactory?.CloseCreatedHost();
                        this.webApplicationFactory?.Dispose();
                        this.webApplicationFactory = null;

                        this.DataBag.DisposeAllDisposables();
                    });
            }

            this.disposedValue = true;

            if (this.IsPreparedOnly)
            {
                this.logger.LogWarning("The test result is evaluated as inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
                Assert.Inconclusive("The test result is evaluated as inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
            }

            if (this.Exceptions.Any())
            {
                this.logger.LogError(
                    "The test result is evaluated as failed, because exceptions occured. Count: {Count}; Types: {Types}",
                    this.Exceptions.Count,
                    string.Join(',', this.Exceptions.Select(x => $"({x.Item1},{x.Item2.GetType().Name})")));
                Assert.Fail($"The test result is evaluated as failed, because exceptions occured. Count: {this.Exceptions.Count}; Types: {string.Join(',', this.Exceptions.Select(x => $"({x.Item1},{x.Item2.GetType().Name})"))}");
            }

            this.logger.LogInformation("The test result is evaluated as successful.");
        }
    }
}
