// <copyright file="BrowserFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// This class represents a factory for creating Browsers of a given type (e.g.: Chrome).
/// </summary>
internal sealed class BrowserFactory : IBrowserFactory, IDisposable
{
    private readonly ILogger logger;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserFactory"/> class.
    /// </summary>
    public BrowserFactory()
    {
        this.logger = TestBaseSingletonContainer.CreateLogger<BrowserFactory>();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BrowserFactory"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    ~BrowserFactory()
    {
        this.Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public IBrowser CreateBrowser(Uri url, TestBaseScopeContainer scope, string? humanReadablePageName = null, BrowserFactoryOptions? options = null)
    {
        scope = Ensure.NotNull(scope);

        this.logger.LogInformation("Create Browser requested for {Url}", url);
        var driver = TestBaseSingletonContainer.WebDriverFactory.CreateWebDriver(
            options ?? new BrowserFactoryOptions()
            {
                Headless = true,
                IgnoreSslErrors = true,
            },
            scope.TestBase);

        driver.Navigate().GoToUrl(url);

        return new Browser(
            driver,
            scope.TestBase.TestContext.TestRunResultsDirectory ?? throw new InvalidDataException("LogDir not set"),
            humanReadablePageName,
            scope.TestName);
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
                // add managed resources to free here
            }

            this.disposedValue = true;
        }
    }
}
