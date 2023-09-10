﻿// <copyright file="Runner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using WebNativeDEV.SINUS.Core.Events;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
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
    IGiven, IGivenWithSut, IGivenWithSimpleSut, IGivenBrowser,
    IWhenBrowser,
    IThenBrowser
{
    private const string DefaultEndpoint = "https://localhost:10001";

    private readonly IBrowserFactory browserFactory;
    private readonly IExecutionEngine executionEngine;
    private readonly ILogger logger;
    private readonly IEventBus eventBus;

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
        this.eventBus = TestBase.Container.Resolve<IEventBus>();
        this.DataBag = new RunStore(this.eventBus);
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
    public IRunStore DataBag { get; }

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

    /// <summary>
    /// Shuts down all network components and sets them to null.
    /// </summary>
    public void Shutdown()
    {
        this.HttpClient?.CancelPendingRequests();
        this.HttpClient?.Dispose();
        this.HttpClient = null;

        this.browser?.Dispose();
        this.browser = null;

        this.webApplicationFactory?.CloseCreatedHost();
        this.webApplicationFactory?.Dispose(); // consider DisposeAsync()
        this.webApplicationFactory = null;

        this.DataBag.DisposeAllDisposables();

#pragma warning disable S1215 // "GC.Collect" should not be called
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();
#pragma warning restore S1215 // "GC.Collect" should not be called
    }

    private Runner RunCreateSut(
            RunCategory runCategory,
            Type? sutType,
            string? sutEndpoint,
            string? description = null)
    {
        return this.Run(
            new ExecutionParameterBuilder(
                this.TestBase,
                this,
                this.NamingConventionManager,
                runCategory,
                this.Exceptions.Count)
            .AddDescription(description)
            .AddRunActions(false)
            .AddSetupActions(null)
            .AddActions(null, null)
            .AddCreateSut(true)
            .AddSutType(sutType)
            .AddSutEndpoint(sutEndpoint)
            .Build());
    }

    private Runner RunCreateSut(
            RunCategory runCategory,
            Action<ExecutionSetupParameters>? setupAction,
            Type? sutType,
            string? sutEndpoint,
            string? description = null)
    {
        return this.Run(
            new ExecutionParameterBuilder(
                this.TestBase,
                this,
                this.NamingConventionManager,
                runCategory,
                this.Exceptions.Count)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(setupAction)
            .AddActions(null, null)
            .AddCreateSut(true)
            .AddSutType(sutType)
            .AddSutEndpoint(sutEndpoint)
            .Build());
    }

    private Runner RunCreateSut(
            RunCategory runCategory,
            Action? action,
            Type? sutType,
            string? sutEndpoint,
            string? description = null)
    {
        return this.Run(
            new ExecutionParameterBuilder(
                this.TestBase,
                this,
                this.NamingConventionManager,
                runCategory,
                this.Exceptions.Count)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(null)
            .AddActions(action, null)
            .AddCreateSut(true)
            .AddSutType(sutType)
            .AddSutEndpoint(sutEndpoint)
            .Build());
    }

    private Runner RunAction(
            RunCategory runCategory,
            Action? action,
            string? description = null)
    {
        return this.Run(
            new ExecutionParameterBuilder(
                this.TestBase,
                this,
                this.NamingConventionManager,
                runCategory,
                this.Exceptions.Count)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(null)
            .AddActions(action, null)
            .AddCreateSut(false)
            .AddSutType(null)
            .AddSutEndpoint(null)
            .Build());
    }

    private Runner RunAction(
            RunCategory runCategory,
            IList<Action?>? actions,
            string? description = null)
    {
        return this.Run(
            new ExecutionParameterBuilder(
                this.TestBase,
                this,
                this.NamingConventionManager,
                runCategory,
                this.Exceptions.Count)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(null)
            .AddActions(null, actions)
            .AddCreateSut(false)
            .AddSutType(null)
            .AddSutEndpoint(null)
            .Build());
    }

    private Runner Run(ExecutionParameter parameter)
    {
        var output = this.executionEngine.Run(parameter);

        if (this.HttpClient == null && output.HttpClient != null)
        {
            this.HttpClient = output.HttpClient;
        }
        else if (this.HttpClient != null && output.HttpClient != null)
        {
            this.HttpClient.Dispose();
            this.HttpClient = output.HttpClient;
        }

        if (this.webApplicationFactory == null && output.WebApplicationFactory != null)
        {
            this.webApplicationFactory = output.WebApplicationFactory;
        }
        else if (this.webApplicationFactory != null && output.WebApplicationFactory != null)
        {
            this.webApplicationFactory.Dispose();
            this.webApplicationFactory = output.WebApplicationFactory;
        }

        this.Exceptions.AddRange(output.Exceptions.Select(exc => (parameter.RunCategory, exc)));
        this.IsPreparedOnly = this.IsPreparedOnly ||
            (output.IsPreparedOnly && parameter.RunCategory == RunCategory.When);

        if (output.RunCategory != RunCategory.Listen)
        {
            this.eventBus.Publish(this, new ExecutedEventBusEventArgs(output));
        }

        return this;
    }

    private Action? InvokeAction<TSut>(Action<TSut, IRunStore>? action)
        where TSut : class
    {
        if (action == null)
        {
            return null;
        }

        return () => action.Invoke(this.DataBag.ReadSut<TSut>(), this.DataBag);
    }

    private IList<Action?>? InvokeAction(Action<IRunStore>[] actions)
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

    private IList<Action?>? InvokeAction(Action<IBrowser, IRunStore>[] actions)
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

    private Action? InvokeAction(Action<IRunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action.Invoke(this.DataBag);
    }

    private Action? InvokeAction(Action<HttpClient, IRunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action?.Invoke(
            this.HttpClient ?? throw new InvalidOperationException("HttpClient was not set in an operation"),
            this.DataBag);
    }

    private Action? InvokeAction(Action<IBrowser, IRunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action?.Invoke(
                this.browser ?? throw new InvalidOperationException("no browser created"),
                this.DataBag);
    }

    private Action? InvokeAction<TEventBusEventArgs>(object sender, TEventBusEventArgs? e, Action<object, IRunStore, TEventBusEventArgs> handler, Func<object, IRunStore, TEventBusEventArgs, bool>? filter)
        where TEventBusEventArgs : EventBusEventArgs
    {
        if (handler == null)
        {
            return null;
        }

        if (e == null)
        {
            throw new InvalidCastException();
        }

        return () =>
        {
            if (filter?.Invoke(sender, this.DataBag, e) ?? true)
            {
                handler.Invoke(sender, this.DataBag, e);
            }
        };
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
                this.RunAction(
                    runCategory: RunCategory.Dispose,
                    action: () =>
                    {
                        this.Shutdown();
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
