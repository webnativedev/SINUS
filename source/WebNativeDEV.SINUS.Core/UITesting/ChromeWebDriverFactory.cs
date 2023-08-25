// <copyright file="ChromeWebDriverFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// WebDriverFactory creating Chrome based web drivers.
/// </summary>
internal sealed class ChromeWebDriverFactory : IWebDriverFactory
{
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromeWebDriverFactory"/> class.
    /// </summary>
    public ChromeWebDriverFactory()
    {
        this.logger = TestBase.Container.Resolve<ILoggerFactory>().CreateLogger<ChromeWebDriverFactory>();
    }

    /// <inheritdoc/>
    public IWebDriver CreateWebDriver(BrowserFactoryOptions options, TestBase testBase)
    {
        Ensure.NotNull(options, nameof(options));
        Ensure.NotNull(testBase, nameof(testBase));

        this.logger.LogInformation("Create driver instance for chrome with options '{Options}'", options);

#pragma warning disable CA2000
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.EnableVerboseLogging = true;
        service.EnableAppendLog = true;
        service.LogPath = Path.Combine(
            testBase.LogsDir,
            $"chromedriverservices_{DateTime.Now:yyyy-MM-dd__HH-mm-ss-fffffff}.log");
#pragma warning restore CA2000

        this.logger.LogInformation("write chromedriverservice-logs to: {Path}", service.LogPath);

        var chromeOptions = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
            PageLoadStrategy =
                PageLoadStrategy.Eager, // wait until DomContentLoaded Event
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
