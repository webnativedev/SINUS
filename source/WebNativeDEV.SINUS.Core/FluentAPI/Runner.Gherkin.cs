// <copyright file="Runner.Gherkin.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Events.EventArguments;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.Core.UITesting.Model;

/// <summary>
/// Base Class for Runners.
/// </summary>
internal sealed partial class Runner
{
    /// <inheritdoc/>
    public IRunner Listen<TEventBusEventArgs>(string description, Action<object, IRunStore, TEventBusEventArgs> handler, Func<object, IRunStore, TEventBusEventArgs, bool>? filter = null)
        where TEventBusEventArgs : EventBusEventArgs
    {
        this.scope.EventBus.Subscribe<TEventBusEventArgs>(
            (sender, e) => this.RunAction(
                    runCategory: RunCategory.Listen,
                    description: description,
                    action: this.InvokeAction<TEventBusEventArgs>(sender, e as TEventBusEventArgs, handler, filter)));
        return this;
    }

    /// <inheritdoc/>
    public IRunner Listen<TEventBusEventArgs>(Action<object, IRunStore, TEventBusEventArgs> handler, Func<object, IRunStore, TEventBusEventArgs, bool>? filter = null)
        where TEventBusEventArgs : EventBusEventArgs
    {
        this.scope.EventBus.Subscribe<TEventBusEventArgs>(
            (sender, e) => this.RunAction(
                    runCategory: RunCategory.Listen,
                    action: this.InvokeAction<TEventBusEventArgs>(sender, e as TEventBusEventArgs, handler, filter)));
        return this;
    }

    /// <inheritdoc/>
    public IGiven Given(string description, Action<IRunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IGiven Given(Action<IRunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystem(string description, Func<object> sutFactory)
        => this.RunAction(
                runCategory: RunCategory.Given,
                description: description,
                action: this.InvokeAction((data) => data.StoreSut(sutFactory?.Invoke())));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystem(Func<object> sutFactory)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction((data) => data.StoreSut(sutFactory?.Invoke())));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystem(Func<IRunStore, object> sutFactory)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction((data) => data.StoreSut(sutFactory?.Invoke(this.scope.DataBag))));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystem(string description, object sut)
        => this.RunAction(
                runCategory: RunCategory.Given,
                description: description,
                action: this.InvokeAction((data) => data.StoreSut(sut)));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystem(object sut)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction((data) => data.StoreSut(sut)));

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>(string description)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                description: description,
                sutType: typeof(TProgram),
                sutArgs: null,
                sutEndpoint: null);

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>()
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                sutType: typeof(TProgram),
                sutArgs: null,
                sutEndpoint: null);

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>(string description, params string[] args)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                description: description,
                sutType: typeof(TProgram),
                sutArgs: args,
                sutEndpoint: null);

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>(params string[] args)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                sutType: typeof(TProgram),
                sutArgs: args,
                sutEndpoint: null);

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystemAsync(string description, Func<Task<object>> sutFactory)
        => this.RunAction(
                runCategory: RunCategory.Given,
                description: description,
                action: this.InvokeAction(async (data) => data.StoreSut(await Ensure.NotNull(sutFactory).Invoke().ConfigureAwait(false))));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystemAsync(Func<Task<object>> sutFactory)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction(async (data) => data.StoreSut(await Ensure.NotNull(sutFactory).Invoke().ConfigureAwait(false))));

    /// <inheritdoc/>
    public IGivenWithSimpleSut GivenASystemAsync(Func<IRunStore, Task<object>> sutFactory)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction(async (data) => data.StoreSut(await Ensure.NotNull(sutFactory).Invoke(this.scope.DataBag).ConfigureAwait(false))));

    /// <inheritdoc/>
    public IWhen When(string? description, Action<IRunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When(Action<IRunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When(string description, Action<HttpClient, IRunStore>? action)
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When(Action<HttpClient, IRunStore>? action)
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IThen Then(string? description, params Action<IRunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                actions: this.InvokeAction(actions));

    /// <inheritdoc/>
    public IThen Then(params Action<IRunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                actions: this.InvokeAction(actions));

    /// <inheritdoc/>
    public IThen ThenNoError(string? description)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                action: () =>
                {
                    this.scope.Exceptions.Count.Should().Be(0);
                });

    /// <inheritdoc/>
    public IThen ThenNoError()
        => this.RunAction(
                runCategory: RunCategory.Then,
                action: () =>
                {
                    this.scope.Exceptions.Count.Should().Be(0);
                });

    /// <inheritdoc/>
    public IThen ThenShouldHaveFailed()
        => this.RunAction(
                runCategory: RunCategory.Then,
                action: () =>
                {
                    this.scope.Exceptions.Count.Should().Be(1);
                    this.scope.Exceptions.SetAllChecked();
                });

    /// <inheritdoc/>
    public IThen ThenShouldHaveFailed(string description)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                action: () =>
                {
                    this.scope.Exceptions.Count.Should().Be(1);
                    this.scope.Exceptions.SetAllChecked();
                });

    /// <inheritdoc/>
    public IThen ThenShouldHaveFailedWith<T>()
        where T : Exception
        => this.RunAction(
                runCategory: RunCategory.Then,
                action: () =>
                {
                    this.scope.Exceptions.CountOfType<T>().Should().Be(1);
                    this.scope.Exceptions.SetAllCheckedOfType<T>();
                });

    /// <inheritdoc/>
    public IThen Debug(Action<IBrowser, IRunStore>? action = null, bool shouldRun = true)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: this.InvokeAction(action),
                runActions: shouldRun);

    /// <inheritdoc/>
    public IThen Debug(Action<IRunStore>? action = null, bool shouldRun = true)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: this.InvokeAction(action),
                runActions: shouldRun);

    /// <inheritdoc/>
    public IThen DebugBreak()
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: this.InvokeAction((data) => Debugger.Break()),
                runActions: Debugger.IsAttached);

    /// <inheritdoc/>
    public IThen DebugPrint(RunStorePrintOrder order = RunStorePrintOrder.KeySorted)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () => this.scope.DataBag.PrintStore(order));

    /// <inheritdoc/>
    public IThen DebugPrint((string, object?)[] additionalData)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () =>
                {
                    this.scope.DataBag.PrintStore();
                    foreach (var data in additionalData)
                    {
                        this.scope.DataBag.PrintAdditional(data.Item1, data.Item2);
                    }
                });

    /// <inheritdoc/>
    public IThen DebugPrint(RunStorePrintOrder order, (string, object?)[] additionalData)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () =>
                {
                    this.scope.DataBag.PrintStore(order);
                    foreach (var data in additionalData)
                    {
                        this.scope.DataBag.PrintAdditional(data.Item1, data.Item2);
                    }
                });

    /// <inheritdoc/>
    public IThen DebugPrint(RunStorePrintOrder order, string key, object? value)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () =>
                {
                    this.scope.DataBag.PrintStore(order);
                    this.scope.DataBag.PrintAdditional(key, value);
                });

    /// <inheritdoc/>
    public IThen DebugPrint(string key, object? value)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () =>
                {
                    this.scope.DataBag.PrintStore();
                    this.scope.DataBag.PrintAdditional(key, value);
                });

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string? humanReadablePageName, string url, BrowserFactoryOptions? options = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(url), humanReadablePageName, options));

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string url, BrowserFactoryOptions? options = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(url), options));

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt((string? humanReadablePageName, string url) website, BrowserFactoryOptions? options = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(website.url), website.humanReadablePageName, options));

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string? humanReadablePageName, Uri url, BrowserFactoryOptions? options = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(url, humanReadablePageName, options));

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(Uri url, BrowserFactoryOptions? options = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(url, options));

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtSpecificEndpoint<TProgram>(string? humanReadablePageName, string endpoint, string url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(url), humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtSpecificEndpoint<TProgram>(string endpoint, string url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(url), options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtSpecificEndpoint<TProgram>(string? humanReadablePageName, string endpoint, Uri url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(url, humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtSpecificEndpoint<TProgram>(string endpoint, Uri url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(url, options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>((string? humanReadablePageName, string? browserPageToStart) page, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserForDefaultSutAction(page.browserPageToStart, page.humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: DefaultEndpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>(string? humanReadablePageName, string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserForDefaultSutAction(browserPageToStart, humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: DefaultEndpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>(string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserForDefaultSutAction(browserPageToStart, options),
                sutType: typeof(TProgram),
                sutEndpoint: DefaultEndpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtRandomEndpoint<TProgram>((string? humanReadablePageName, string? browserPageToStart) page, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                setupAction: this.InvokeCreateBrowserForRandomSutAction(page.browserPageToStart, page.humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: ExecutionEngine.RandomEndpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtRandomEndpoint<TProgram>(string? humanReadablePageName, string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                setupAction: this.InvokeCreateBrowserForRandomSutAction(browserPageToStart, humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: ExecutionEngine.RandomEndpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtRandomEndpoint<TProgram>(string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                setupAction: this.InvokeCreateBrowserForRandomSutAction(browserPageToStart, options),
                sutType: typeof(TProgram),
                sutEndpoint: ExecutionEngine.RandomEndpoint);

    /// <inheritdoc/>
    public IWhenBrowser When(string description, Action<IBrowser, IRunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhenBrowser When(Action<IBrowser, IRunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When<TSut>(Action<TSut, IRunStore>? action)
        where TSut : class
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction<TSut>(action));

    /// <inheritdoc/>
    public IWhen When<TSut>(string description, Action<TSut, IRunStore>? action)
        where TSut : class
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction<TSut>(action));

    /// <inheritdoc/>
    public IThenBrowser Then(string description, params Action<IBrowser, IRunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                actions: this.InvokeAction(actions));

    /// <inheritdoc/>
    public IThenBrowser Then(params Action<IBrowser, IRunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                actions: this.InvokeAction(actions));

    /// <inheritdoc/>
    public IDisposable ExpectFail()
    {
        this.scope.ExpectedOutcome = TestOutcome.Failure;
        return this;
    }

    /// <inheritdoc/>
    public IDisposable ExpectInconclusive()
    {
        this.scope.ExpectedOutcome = TestOutcome.Inconclusive;
        return this;
    }
}