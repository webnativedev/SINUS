// <copyright file="BrowserRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using System;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents a class that manages the execution of a test based on a given-when-then sequence.
/// </summary>
internal sealed class BrowserRunner : Runner, IBrowserRunner, IGivenBrowser, IWhenBrowser, IThenBrowser
{
    private const string DefaultEndpoint = "https://localhost:10001";

    private IBrowser? browser;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserRunner"/> class.
    /// </summary>
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    /// <param name="factory">Reference to the browser factory to use (e.g.: for chrome browsers).</param>
    public BrowserRunner(ILoggerFactory loggerFactory, IBrowserFactory factory)
        : base(loggerFactory)
    {
        this.Factory = factory;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BrowserRunner"/> class.
    /// </summary>
    ~BrowserRunner()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Gets the factory that creates browser instances.
    /// </summary>
    private IBrowserFactory Factory { get; }

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string? humanReadablePageName, string url)
        => this.GivenABrowserAt(humanReadablePageName, new Uri(url));

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt((string? humanReadablePageName, string url) website)
        => this.GivenABrowserAt(website.humanReadablePageName, website.url);

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string? humanReadablePageName, Uri url)
    => (IGivenBrowser)this.Run(
            "Given",
            $"a Browser at {url} - {humanReadablePageName}",
            () => this.browser = this.Factory.CreateBrowser(url));

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, string url)
        where TProgram : class
        => this.GivenASystemAndABrowserAt<TProgram>(humanReadablePageName, endpoint, new Uri(url));

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, Uri url)
        where TProgram : class
        => (IGivenBrowser)this.Run(
            "Given",
            $"a SUT {endpoint} and a Browser",
            () =>
            {
                this.CreateSut<TProgram>(endpoint);
                this.GivenABrowserAt(humanReadablePageName, url);
            });

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>((string? humanReadablePageName, string? browserPageToStart) page)
        where TProgram : class
        => this.GivenASystemAndABrowserAtDefaultEndpoint<TProgram>(page.humanReadablePageName, page.browserPageToStart);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>(string? humanReadablePageName, string? browserPageToStart = null)
        where TProgram : class
        => this.GivenASystemAndABrowserAt<TProgram>(humanReadablePageName, DefaultEndpoint, DefaultEndpoint + (browserPageToStart ?? string.Empty));

    /// <inheritdoc/>
    public IWhenBrowser When(string description, Action<IBrowser, Dictionary<string, object?>>? action = null)
    {
        if (action == null)
        {
            this.IsPreparedOnly = true;
        }

        return (IWhenBrowser)this.Run("When", description, () => action?.Invoke(
            this.browser ?? throw new InvalidOperationException("no browser created"),
            this.DataBag));
    }

    /// <inheritdoc/>
    public IThenBrowser Then(string description, Action<IBrowser, Dictionary<string, object?>>? action = null)
        => (IThenBrowser)this.Run("Then", description, () => action?.Invoke(
            this.browser ?? throw new InvalidOperationException("no browser created"),
            this.DataBag));

    /// <inheritdoc/>
    public IDisposable Debug(Action<IBrowser, Dictionary<string, object?>>? action = null)
        => (IDisposable)this.Run("Debug", string.Empty, () => action?.Invoke(
            this.browser ?? throw new InvalidOperationException("no browser created"),
            this.DataBag));

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.browser?.Dispose();
                this.browser = null;
            }

            this.disposedValue = true;
        }

        base.Dispose(disposing);
    }
}
