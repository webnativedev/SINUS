// <copyright file="ChromeBrowserFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting.Chrome;

using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using WebNativeDEV.SINUS.Core.UITesting;

/// <summary>
/// This class represents a factory for creating Chrome Browser instances to execute tests on.
/// </summary>
internal sealed class ChromeBrowserFactory : BrowserFactory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChromeBrowserFactory"/> class.
    /// </summary>
    /// <param name="runDir">Folder of the test.</param>
    /// <param name="logDir">Folder of the log.</param>
    /// <param name="loggerFactory">Factory to create logger-objects.</param>
    public ChromeBrowserFactory(string runDir, string logDir, ILoggerFactory loggerFactory)
        : base(runDir, logDir, loggerFactory)
    {
    }

    /// <summary>
    /// Gets the logger for the ChromeBrowserFactory.
    /// </summary>
    private ILogger Logger => this.LoggerFactory.CreateLogger<ChromeBrowserFactory>();

    /// <inheritdoc/>
    protected override ChromeDriver CreateWebDriver(BrowserFactoryOptions options)
    {
        this.Logger.LogInformation("Create driver instance for chrome with options '{Options}'", options);

#pragma warning disable CA2000
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.EnableVerboseLogging = true;
        service.EnableAppendLog = true;
        service.LogPath = Path.Combine(
            this.LogsDir,
            $"chromedriverservices_{DateTime.Now:yyyy-MM-dd__HH-mm-ss-fffffff}.log");
#pragma warning restore CA2000

        this.Logger.LogInformation("write chromedriverservice-logs to: {Path}", service.LogPath);

        var chromeOptions = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
            PageLoadStrategy =
                OpenQA.Selenium.PageLoadStrategy.Eager, // wait until DomContentLoaded Event
        };

        if (options.IgnoreSslErrors)
        {
            chromeOptions.AddArguments("--ignore-certificate-errors");
        }

        if (options.Headless)
        {
            chromeOptions.AddArguments("--headless=chrome"); // or =new
        }

        var driver = new ChromeDriver(service, chromeOptions);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

        return driver;
    }
}
