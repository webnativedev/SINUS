// <copyright file="Runner.cs" company="WebNativeDEV">
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
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
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
internal sealed partial class Runner : IBrowserRunner
{
    private const string DefaultEndpoint = "https://localhost:10001";

    private readonly ILogger logger;
    private readonly TestBaseScopeContainer scope;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    /// <param name="scope">Reference to the scope dependencies.</param>
    public Runner(TestBaseScopeContainer scope)
    {
        this.scope = scope;
        this.scope.Runner = this;

        this.logger = scope.CreateLogger<Runner>();
        this.logger.LogDebug("Created a log for base-runner");
    }

    /// <summary>
    /// Disposes the object as defined in IDisposable.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private Runner RunCreateSut(
            RunCategory runCategory,
            Type? sutType,
            IEnumerable<string>? sutArgs,
            string? sutEndpoint,
            string? description = null)
    {
        return this.Run(
            new ExecutionParameterBuilder(
                this.scope,
                runCategory)
            .AddDescription(description)
            .AddRunActions(false)
            .AddSetupActions(null)
            .AddActions(null, null)
            .AddCreateSut(true)
            .AddSutType(sutType)
            .AddSutArgs(sutArgs)
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
                this.scope,
                runCategory)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(setupAction)
            .AddActions(null, null)
            .AddCreateSut(true)
            .AddSutType(sutType)
            .AddSutArgs(null)
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
                this.scope,
                runCategory)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(null)
            .AddActions(action, null)
            .AddCreateSut(true)
            .AddSutType(sutType)
            .AddSutArgs(null)
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
                this.scope,
                runCategory)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(null)
            .AddActions(action, null)
            .AddCreateSut(false)
            .AddSutType(null)
            .AddSutArgs(null)
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
                this.scope,
                runCategory)
            .AddDescription(description)
            .AddRunActions(true)
            .AddSetupActions(null)
            .AddActions(null, actions)
            .AddCreateSut(false)
            .AddSutType(null)
            .AddSutArgs(null)
            .AddSutEndpoint(null)
            .Build());
    }

    private Runner Run(ExecutionParameter parameter)
    {
        var output = this.scope.ExecutionEngine.Run(parameter);

        if (this.scope.HttpClient == null && output.HttpClient != null)
        {
            this.scope.HttpClient = output.HttpClient;
        }
        else if (this.scope.HttpClient != null && output.HttpClient != null)
        {
            this.scope.HttpClient.Dispose();
            this.scope.HttpClient = output.HttpClient;
        }

        if (this.scope.WebApplicationFactory == null && output.WebApplicationFactory != null)
        {
            this.scope.WebApplicationFactory = output.WebApplicationFactory;
        }
        else if (this.scope.WebApplicationFactory != null && output.WebApplicationFactory != null)
        {
            this.scope.WebApplicationFactory.Dispose();
            this.scope.WebApplicationFactory = output.WebApplicationFactory;
        }

        foreach (var e in output.Exceptions)
        {
            this.scope.Exceptions.Add(new ExceptionStoreItem(parameter.RunCategory, e));
        }

        this.scope.IsPreparedOnly = this.scope.IsPreparedOnly ||
            (output.IsPreparedOnly && parameter.RunCategory == RunCategory.When);

        if (output.RunCategory != RunCategory.Listen)
        {
            this.scope.EventBus.Publish(this, new ExecutedEventBusEventArgs(output));
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

        return () => action.Invoke(this.scope.DataBag.ReadSut<TSut>(), this.scope.DataBag);
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

        return () => action.Invoke(this.scope.DataBag);
    }

    private Action? InvokeAction(Action<HttpClient, IRunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action?.Invoke(
            this.scope.HttpClient ?? throw new InvalidOperationException("HttpClient was not set in an operation"),
            this.scope.DataBag);
    }

    private Action? InvokeAction(Action<IBrowser, IRunStore>? action)
    {
        if (action == null)
        {
            return null;
        }

        return () => action?.Invoke(
                this.scope.Browser ?? throw new InvalidOperationException("no browser created"),
                this.scope.DataBag);
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
            if (filter?.Invoke(sender, this.scope.DataBag, e) ?? true)
            {
                handler.Invoke(sender, this.scope.DataBag, e);
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
        return () => this.scope.Browser = this.scope.BrowserFactory.CreateBrowser(
            url,
            this.scope.TestBase,
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
                        this.scope.Shutdown();
                        this.scope.Runner = null;
                    });
            }

            this.disposedValue = true;

            if (this.scope.IsPreparedOnly)
            {
                this.logger.LogWarning("The test result is evaluated as inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
                Assert.Inconclusive("The test result is evaluated as inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
            }

            if (this.scope.Exceptions.Any(e => !e.IsCheckedInThenClause))
            {
                this.logger.LogError(
                    "The test result is evaluated as failed, because exceptions occured. Count: {Count}; Types: {Types}",
                    this.scope.Exceptions.Count,
                    string.Join(',', this.scope.Exceptions.Select(x => x.GetTupleString())));
                Assert.Fail($"The test result is evaluated as failed, because exceptions occured. Count: {this.scope.Exceptions.Count}; Types: {string.Join(',', this.scope.Exceptions.Select(x => x.GetTupleString()))}");
            }

            this.logger.LogInformation("The test result is evaluated as successful. (Checked Exceptions: {CheckedExceptionCount})", this.scope.Exceptions.Count(e => e.IsCheckedInThenClause));
        }
    }
}
