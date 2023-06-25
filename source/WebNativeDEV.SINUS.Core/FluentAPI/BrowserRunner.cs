// <copyright file="BrowserRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using System;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.SUT;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents a class that manages the execution of a test based on a given-when-then sequence.
/// </summary>
internal sealed class BrowserRunner : BaseRunner, IBrowserRunner, IGivenBrowser, IWhenBrowser, IThenBrowser
{
    private IBrowser? browser;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserRunner"/> class.
    /// </summary>
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    /// <param name="factory">Reference to the browser factory to use (e.g.: for chrome browsers).</param>
    public BrowserRunner(ILoggerFactory loggerFactory, IBrowserFactory factory)
        : base(loggerFactory)
    {
        this.Logger = loggerFactory.CreateLogger<BrowserRunner>();
        this.Factory = factory;
        this.browser = null;
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

    /// <summary>
    /// Gets the logger that can be used to print information.
    /// </summary>
    private ILogger Logger { get; }

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string? humanReadablePageName, string url)
        => this.GivenABrowserAt(humanReadablePageName, new Uri(url));

    /// <inheritdoc/>
    public IGivenBrowser GivenABrowserAt(string? humanReadablePageName, Uri url)
    {
        this.Logger.LogInformation("Given: a Browser at {Url} - {Page}", url, humanReadablePageName ?? "null");
        this.browser = this.Factory.CreateBrowser(url);
        return this;
    }

    /// <inheritdoc/>
    public IWhenBrowser When(string description, Action<IBrowser, Dictionary<string, object?>>? action = null)
        => this.Run("When", description, action);

    /// <inheritdoc/>
    public IThenBrowser Then(string description, Action<IBrowser, Dictionary<string, object?>>? action = null)
        => this.Run("Then", description, action);

    /// <inheritdoc/>
    public IDisposable Debug(Action<IBrowser, Dictionary<string, object?>>? action = null)
        => this.Run("Debug", string.Empty, action);

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, string url)
        where TProgram : class
        => this.GivenASystemAndABrowserAt<TProgram>(humanReadablePageName, endpoint, new Uri(url));

    /// <inheritdoc/>
    public IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, Uri url)
        where TProgram : class
    {
        this.Logger.LogInformation("Given: a SUT and a Browser at {Url} - desc: {Page}", endpoint, humanReadablePageName ?? "<none>");

        SinusWebApplicationFactory<TProgram>? builder = null;
        HttpClient? client = null;
        for (int i = 0; i < 3; i++)
        {
            try
            {
                builder = new SinusWebApplicationFactory<TProgram>(endpoint);
                client = builder.CreateClient();
                break;
            }
            catch (IOException exception)
            {
                this.Logger.LogError(exception, "retry attempt {Attempt}", i + 1);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        if (client != null && builder != null)
        {
            this.Disposables.Add(new SinusWebApplicationFactoryResult<TProgram>(builder, client));
        }
        else
        {
            client?.Dispose();
            builder?.Dispose();
        }

        return this.GivenABrowserAt(humanReadablePageName, url);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        this.browser?.Dispose();
        this.browser = null;
    }

    private BrowserRunner Run(string category, string description, Action<IBrowser, Dictionary<string, object?>>? action)
    {
        this.Logger.LogInformation("{Category}: {Description}", category, description);
        action?.Invoke(this.browser ?? throw new InvalidOperationException("Browser not set"), this.DataBag);
        return this;
    }
}
