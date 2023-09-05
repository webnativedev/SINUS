// <copyright file="Runner.Gherkin.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using WebNativeDEV.SINUS.Core.Execution;
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
internal sealed partial class Runner
{
    /// <inheritdoc/>
    public IGiven Given(string description, Action<RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IGiven Given(Action<RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.Given,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>(string description)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                description: description,
                sutType: typeof(TProgram),
                sutEndpoint: null);

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>()
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                sutType: typeof(TProgram),
                sutEndpoint: null);

    /// <inheritdoc/>
    public IWhen When(string? description, Action<RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When(Action<RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When(string description, Action<HttpClient, RunStore>? action)
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhen When(Action<HttpClient, RunStore>? action)
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IThen Then(string? description, params Action<RunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                actions: this.InvokeAction(actions));

    /// <inheritdoc/>
    public IThen Then(params Action<RunStore>[] actions)
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
                    if (this.Exceptions.Any())
                    {
                        Assert.Fail("Exception was not thrown.");
                    }
                });

    /// <inheritdoc/>
    public IThen ThenNoError()
        => this.RunAction(
                runCategory: RunCategory.Then,
                action: () =>
                {
                    if (this.Exceptions.Any())
                    {
                        Assert.Fail("Exception was not thrown.");
                    }
                });

    /// <inheritdoc/>
    public IThen ThenShouldHaveFailed()
        => this.RunAction(
                runCategory: RunCategory.Then,
                action: () =>
                {
                    if (!this.Exceptions.Any())
                    {
                        Assert.Fail("Expected exception was not thrown.");
                    }
                });

    /// <inheritdoc/>
    public IThen ThenShouldHaveFailed(string description)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                action: () =>
                {
                    if (!this.Exceptions.Any())
                    {
                        Assert.Fail("Expected exception was not thrown.");
                    }
                });

    /// <inheritdoc/>
    public IThen ThenShouldHaveFailedWith<T>()
        where T : Exception
        => this.RunAction(
                runCategory: RunCategory.Then,
                action: () =>
                {
                    if (!this.Exceptions
                            .Select(x => x.Item2)
                            .Any(e => e is T))
                    {
                        Assert.Fail("Expected exception was not thrown.");
                    }
                });

    /// <inheritdoc/>
    public IDisposable Debug(Action<IBrowser, RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IDisposable Debug(Action<RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IDisposable DebugPrint()
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () => this.DataBag.PrintStore());

    /// <inheritdoc/>
    public IDisposable DebugPrint((string, object?)[] additionalData)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () =>
                {
                    this.DataBag.PrintStore();
                    foreach (var data in additionalData)
                    {
                        this.DataBag.PrintAdditional(data.Item1, data.Item2);
                    }
                });

    /// <inheritdoc/>
    public IDisposable DebugPrint(string key, object? value)
        => this.RunAction(
                runCategory: RunCategory.Debug,
                action: () =>
                {
                    this.DataBag.PrintStore();
                    this.DataBag.PrintAdditional(key, value);
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
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, string url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(url), humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string endpoint, string url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(new Uri(url), options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, Uri url, BrowserFactoryOptions? options = null)
        where TProgram : class
        => this.RunCreateSut(
                runCategory: RunCategory.Given,
                action: this.InvokeCreateBrowserAction(url, humanReadablePageName, options),
                sutType: typeof(TProgram),
                sutEndpoint: endpoint);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string endpoint, Uri url, BrowserFactoryOptions? options = null)
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
    public IWhenBrowser When(string description, Action<IBrowser, RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                description: description,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IWhenBrowser When(Action<IBrowser, RunStore>? action = null)
        => this.RunAction(
                runCategory: RunCategory.When,
                action: this.InvokeAction(action));

    /// <inheritdoc/>
    public IThenBrowser Then(string description, params Action<IBrowser, RunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                description: description,
                actions: this.InvokeAction(actions));

    /// <inheritdoc/>
    public IThenBrowser Then(params Action<IBrowser, RunStore>[] actions)
        => this.RunAction(
                runCategory: RunCategory.Then,
                actions: this.InvokeAction(actions));
}