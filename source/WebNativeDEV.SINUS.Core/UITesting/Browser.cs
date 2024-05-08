// <copyright file="Browser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;
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
    private readonly string contentFolder;
    private readonly string id;
    private IWebDriver? driver;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Browser"/> class.
    /// </summary>
    /// <param name="driver">Underlying Selenium WebDrivers.</param>
    /// <param name="contentFolder">Folder to store data.</param>
    /// <param name="humanReadablePageName">Logical page name.</param>
    /// <param name="id">Single identifier that identifies the browser uniquely inside the test session.</param>
    public Browser(IWebDriver driver, string contentFolder = "./", string? humanReadablePageName = null, string? id = null)
    {
        this.Logger = TestBaseSingletonContainer.CreateLogger<Browser>();
        this.Logger.LogInformation(
            "Broswer object created {DriverName} - logger: {LoggerName} - content: {ContentFolder}",
            driver?.GetType()?.ToString() ?? LoggerConstants.NullString,
            this.Logger.GetType().ToString(),
            contentFolder);

        this.driver = Ensure.NotNull(driver, nameof(driver));
        this.contentFolder = contentFolder;
        this.HumanReadablePageName = humanReadablePageName;
        this.id = id ?? "<no id>";

        if (id != null)
        {
            TestBaseSingletonContainer.TestBaseUsageStatisticsManager.SetAttribute(
                this.id,
                TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserCreated,
                true);
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

    /// <inheritdoc/>
    public string? Title
    {
        get
        {
            this.Logger.LogInformation("Title requested {Title}", this.driver?.Title ?? LoggerConstants.NullString);
            return this.driver?.Title;
        }
    }

    /// <inheritdoc/>
    public Uri? Url
    {
        get
        {
            this.Logger.LogInformation("Url requested {Url}", this.driver?.Url ?? LoggerConstants.NullString);
            return new Uri(this.driver?.Url ?? throw new InvalidOperationException("no Url available"));
        }
    }

    /// <inheritdoc/>
    public string? PageSource
    {
        get
        {
            this.Logger.LogInformation("PageSource requested {Source}", this.driver?.PageSource ?? LoggerConstants.NullString);
            return this.driver?.PageSource;
        }
    }

    /// <inheritdoc/>
    public string? HumanReadablePageName { get; }

    /// <summary>
    /// Gets the logger instance.
    /// </summary>
    private ILogger Logger { get; }

    /// <inheritdoc/>
    public object? GetBaseObject()
    {
        this.Logger.LogInformation("Driver requested {Driver}", this.driver?.GetType()?.ToString() ?? LoggerConstants.NullString);
        return this.driver;
    }

    /// <inheritdoc/>
    public IEnumerable<IContent> FindElements(string xpath)
    {
        this.Logger.LogInformation("Find Elements based on xpath: {XPath}", xpath);

        return Ensure.NotNull(this.driver)
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
            return new Content(Ensure.NotNull(this.driver).FindElement(By.Id(id)));
        }

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeoutInSeconds));
        IWebElement firstResult = wait.Until(e => e.FindElement(By.Id(id)));

        return new Content(firstResult);
    }

    /// <inheritdoc/>
    public IBrowser NavigateTo(Uri url)
    {
        this.Logger.LogInformation("Navigate to {Url}", url?.ToString() ?? LoggerConstants.NullString);

        this.driver?.Navigate()?.GoToUrl(url);
        return this;
    }

    /// <inheritdoc/>
    public IBrowser NavigateTo(string url)
        => this.NavigateTo(new Uri(url));

    /// <inheritdoc/>
    public IContent FindActiveElement()
    {
        this.Logger.LogInformation("Find Active Element");
        return new Content(Ensure.NotNull(this.driver).SwitchTo().ActiveElement());
    }

    /// <inheritdoc/>
    public IBrowser ExecuteScript(string js, params IContent[] content)
    {
        this.Logger.LogInformation("Execute Javascript {JS} on {Count} items", js, content?.Length ?? 0);

        (this.driver as IJavaScriptExecutor)?.ExecuteScript(
            js,
            content?.Select(x => x.GetBaseObject()) ?? []);
        return this;
    }

    /// <inheritdoc/>
    public IBrowser TakeScreenshot(string? filename = null)
    {
        this.Logger.LogInformation("Take Screenshot requested");
        string resolvedFilename = Path.GetFullPath(filename ??
            Path.Combine(
                this.contentFolder,
                $"Screenshot_{DateTime.Now:yyyy-MM-dd__HH-mm-ss-fffffff}.png"));
        this.Logger.LogInformation("Screenshot filename resolved to {Filename}", resolvedFilename);

        Screenshot? screenshot = (this.driver as ITakesScreenshot)?.GetScreenshot();
        screenshot?.SaveAsFile(resolvedFilename);
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
    /// Shows the object in the debugger.
    /// </summary>
    /// <returns>A representative plain text string.</returns>
    [ExcludeFromCodeCoverage]
    private string GetDebuggerDisplay()
    {
        return $"Broswer object created {this.driver} - content: {this.contentFolder}";
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
                this.driver?.Dispose();
                this.driver = null;

                TestBaseSingletonContainer.TestBaseUsageStatisticsManager.SetAttribute(
                    this.id,
                    TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserDisposed,
                    true);
            }

            this.disposedValue = true;
        }
    }
}
