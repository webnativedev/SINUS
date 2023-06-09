﻿// <copyright file="BrowserFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// This class represents a factory for creating Browsers of a given type (e.g.: Chrome).
/// </summary>
internal abstract class BrowserFactory : IBrowserFactory, IDisposable
{
    private string runDir;
    private string logsDir;
    private ILogger? logger;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserFactory"/> class.
    /// </summary>
    /// <param name="runDir">Directory to store data to.</param>
    /// <param name="logsDir">Directory to log information to.</param>
    /// <param name="loggerFactory">Responsible for creating log-objects.</param>
    protected BrowserFactory(string runDir, string logsDir, ILoggerFactory loggerFactory)
    {
        this.LoggerFactory = loggerFactory;
        this.Logger.LogDebug("BrowserFactory created {Type} {LogDir} {RunDir}", this.GetType().ToString(), logsDir, runDir);

        this.runDir = runDir;
        this.logsDir = logsDir;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BrowserFactory"/> class.
    /// </summary>
    ~BrowserFactory()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Gets or sets the directory where your generated data should be stored at.
    /// </summary>
    public string RunDir
    {
        get
        {
            this.Logger.LogDebug("RunDir queried: {RunDir}", this.runDir);
            return this.runDir;
        }

        set
        {
            this.Logger.LogDebug("RunDir set: {RunDir}", this.runDir);
            this.runDir = value;
        }
    }

    /// <summary>
    /// Gets or sets the directory where logs should be written to.
    /// </summary>
    public string LogsDir
    {
        get
        {
            this.Logger.LogDebug("LogsDir queried: {LogsDir}", this.logsDir);
            return this.logsDir;
        }

        set
        {
            this.Logger.LogDebug("LogDir set: {LogsDir}", this.logsDir);
            this.logsDir = value;
        }
    }

    /// <summary>
    /// Gets the loggerfactory for creating new loggers.
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Gets the logger based on the factory.
    /// </summary>
    private ILogger Logger
    {
        get
        {
            return this.logger ??= this.LoggerFactory.CreateLogger<BrowserFactory>();
        }
    }

    /// <inheritdoc/>
    public virtual IBrowser CreateBrowser(Uri url)
    {
        this.Logger.LogInformation("Create Browser requested for {Url}", url);

        var driver = this.CreateWebDriver();
        driver.Navigate().GoToUrl(url);

        return new Browser(
            driver,
            this.LoggerFactory,
            this.LogsDir ?? throw new InvalidDataException("LogDir not set"));
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
    /// Creates an instance of a web driver. This is a native selenium driver and will
    /// be covered by a Browser object.
    /// </summary>
    /// <returns>A native selenium webdriver to load and interact with web pages.</returns>
    protected abstract IWebDriver CreateWebDriver();

    /// <summary>
    /// Implementation of the disposal as called by IDisposable.Dispose.
    /// </summary>
    /// <param name="disposing">True if called by Dispose; False if called by Destructor.</param>
    protected virtual void Dispose(bool disposing)
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
