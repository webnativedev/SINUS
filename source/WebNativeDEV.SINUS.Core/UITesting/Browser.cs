// <copyright file="Browser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// The browser class represents an instance of a view
/// that loads a given URL and can interact with the page.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
#pragma warning disable CA1724 // ignoring conflicting name with "OpenQA.Selenium.DevTools.V112.Browser"
internal sealed class Browser : IBrowser
#pragma warning restore CA1724
{
    private readonly IWebDriver driver;
    private readonly string contentFolder;
    private readonly string id;
    private bool disposedValue;
    private ILogger? logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Browser"/> class.
    /// </summary>
    /// <param name="driver">Underlying Selenium WebDrivers.</param>
    /// <param name="contentFolder">Folder to store data.</param>
    /// <param name="id">Single identifier that identifies the browser uniquely inside the test session.</param>
    /// <param name="loggerFactory">The factory to create logger-objects.</param>
    public Browser(IWebDriver driver, ILoggerFactory loggerFactory, string contentFolder = "./", string? id = null)
    {
        this.LoggerFactory = loggerFactory;
        this.Logger.LogInformation(
            "Broswer object created {DriverName} - logger: {LoggerName} - content: {ContentFolder}",
            driver?.GetType()?.ToString() ?? "null",
            this.Logger.GetType().ToString(),
            contentFolder);

        this.driver = driver ?? throw new ArgumentNullException(nameof(driver), "driver null");
        this.contentFolder = contentFolder;
        this.id = id ?? "<no id>";

        if(id != null)
        {
            TestsIncludingBrowsers.Add(id);
        }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Browser"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    ~Browser()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Gets a list of test-identifiers that includes a browser.
    /// </summary>
    public static List<string> TestsIncludingBrowsers { get; } = new List<string>();

    /// <summary>
    /// Gets a list of test-identifiers that released the browser after using it.
    /// </summary>
    public static List<string> TestsDisposingBrowsers { get; } = new List<string>();

    /// <inheritdoc/>
    public string? Title
    {
        get
        {
            this.Logger.LogInformation("Title requested {Title}", this.driver.Title ?? "null");
            return this.driver?.Title;
        }
    }

    /// <inheritdoc/>
    public Uri? Url
    {
        get
        {
            this.Logger.LogInformation("Url requested {Url}", this.driver.Url ?? "null");
            return this.driver?.Url == null
                ? null
                : new Uri(this.driver.Url);
        }
    }

    /// <inheritdoc/>
    public string? PageSource
    {
        get
        {
            this.Logger.LogInformation("PageSource requested {Source}", this.driver.PageSource ?? "null");
            return this.driver.PageSource;
        }
    }

    /// <summary>
    /// Gets the factory to create logger instances.
    /// </summary>
    private ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Gets the logger instance.
    /// </summary>
    private ILogger Logger
    {
        get
        {
            return this.logger ??= this.LoggerFactory.CreateLogger<Browser>();
        }
    }

    /// <inheritdoc/>
    public object? GetBaseObject()
    {
        this.Logger.LogInformation("Driver requested {Driver}", this.driver?.GetType()?.ToString() ?? "null");
        return this.driver;
    }

    /// <inheritdoc/>
    public IEnumerable<IContent> FindElements(string xpath)
    {
        this.Logger.LogInformation("Find Elements based on xpath: {XPath}", xpath);

        return this.driver
            .FindElements(By.XPath(xpath))
            .Select(webElement => new Content(webElement));
    }

    /// <inheritdoc/>
    public IContent FindElement(string id, int timeoutInSeconds = 0)
    {
        this.Logger.LogInformation(
            "Find Element based on id: {Id} waiting for {TimeoutInSeconds} sec",
            id,
            timeoutInSeconds);
        if (timeoutInSeconds <= 0)
        {
            return new Content(this.driver.FindElement(By.Id(id)));
        }

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeoutInSeconds));
        IWebElement firstResult = wait.Until(e => e.FindElement(By.Id(id)));

        return new Content(firstResult);
    }

    /// <inheritdoc/>
    public IBrowser NavigateTo(Uri url)
    {
        this.Logger.LogInformation("Navigate to {Url}", url?.ToString() ?? "null");

        this.driver.Navigate().GoToUrl(url);
        return this;
    }

    /// <inheritdoc/>
    public IBrowser NavigateTo(string url)
        => this.NavigateTo(new Uri(url));

    /// <inheritdoc/>
    public IContent FindActiveElement()
    {
        this.Logger.LogInformation("Find Active Element");
        return new Content(this.driver.SwitchTo().ActiveElement());
    }

    /// <inheritdoc/>
    public IBrowser ExecuteScript(string js, params IContent[] content)
    {
        this.Logger.LogInformation("Execute Javascript {JS} on {Count} items", js, content?.Length ?? 0);

        ((IJavaScriptExecutor)this.driver).ExecuteScript(
            js,
            content?.Select(x => x.GetBaseObject()) ?? Array.Empty<object>());
        return this;
    }

    /// <inheritdoc/>
    public IBrowser TakeScreenshot(string? filename = null)
    {
        this.Logger.LogInformation("Take Screenshot requested");
        string resolvedFilename = filename ??
            Path.Combine(
                this.contentFolder,
                $"Screenshot_{DateTime.Now:yyyy-MM-dd__HH-mm-ss-fffffff}.png");
        this.Logger.LogInformation("Screenshot filename resolved to {Filename}", resolvedFilename);

        Screenshot? screenshot = (this.driver as ITakesScreenshot)?.GetScreenshot();
        screenshot?.SaveAsFile(resolvedFilename, ScreenshotImageFormat.Png);
        return this;
    }

    /// <summary>
    /// Disposes the object as defined in IDisposable.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
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
                this.Logger.LogInformation("Driver quitted");
                this.driver?.Quit();
                TestsDisposingBrowsers.Add(this.id);
            }

            this.disposedValue = true;
        }
    }

    /// <summary>
    /// Shows the object in the debugger.
    /// </summary>
    /// <returns>A representative plain text string.</returns>
    [ExcludeFromCodeCoverage]
    private string GetDebuggerDisplay()
    {
        return $"Broswer object created {this.driver} - content: {this.contentFolder}";
    }
}
