// <copyright file="BaseRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.DevTools.V112.WebAuthn;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest.SUT;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Base Class for Runners.
/// </summary>
internal abstract class BaseRunner : IDisposable
{
    private const int RetryCountCreatingSUT = 6;

    private readonly List<IDisposable> disposables;
    private bool disposedValue;
    private HttpClient? httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRunner"/> class.
    /// </summary>
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    public BaseRunner(ILoggerFactory loggerFactory)
    {
        this.Logger = loggerFactory.CreateLogger<BrowserRunner>();
        this.Logger.LogDebug("Created a log for base-runner");

        this.DataBag = new Dictionary<string, object?>();
        this.disposables = new List<IDisposable>();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the test is only a placeholder for later or not.
    /// </summary>
    protected bool IsPreparedOnly { get; set; }

    /// <summary>
    /// Gets the list of disposables (= the list of objects to automatically dispose).
    /// </summary>
    protected List<IDisposable> Disposables => this.disposables;

    /// <summary>
    /// Gets the HttpClient or throws if it does not exist.
    /// </summary>
    protected HttpClient HttpClient =>
        this.httpClient ?? throw new InvalidOperationException("no HttpClient creted");

    /// <summary>
    /// Gets the current state of the test run.
    /// </summary>
    protected Dictionary<string, object?> DataBag { get; }

    /// <summary>
    /// Gets the logger that can be used to print information.
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// Disposes the object as defined in IDisposable.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Basic execution of each step.
    /// </summary>
    /// <param name="category">Part of the flow (given, when, then).</param>
    /// <param name="description">Human readable description what is going to be performed.</param>
    /// <param name="action">The action to be executed.</param>
    /// <returns>A reference to the runner for Fluent API purpose.</returns>
    protected BaseRunner Run(string category, string description, Action action)
    {
        this.Logger.LogInformation("{Category}: {Description}", category, description);
        var watch = Stopwatch.StartNew();
        action?.Invoke();
        watch.Stop();
        this.Logger.LogInformation("{Category}: process took {Elapsed} ms", category, watch.ElapsedMilliseconds);
        return this;
    }

    /// <summary>
    /// Creates a system under test.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the SUT.</typeparam>
    /// <param name="endpoint">If set the public endpoint, else an in-memory SUT is created.</param>
    protected void CreateSUT<TProgram>(string? endpoint = null)
        where TProgram : class
    {
        IDisposable? builder = null;
        this.httpClient = null;
        for (int i = 0; i < RetryCountCreatingSUT; i++)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(endpoint))
                {
                    var privateBuilder = new WebApplicationFactory<TProgram>();
                    this.httpClient = privateBuilder.CreateClient();
                    builder = privateBuilder;
                }
                else
                {
                    var publicBuilder = new SinusWebApplicationFactory<TProgram>(endpoint);
                    this.httpClient = publicBuilder.CreateClient();
                    builder = publicBuilder;
                }

                break;
            }
            catch (IOException exception)
            {
                this.Logger.LogError(exception, "retry attempt {Attempt}", i + 1);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        if (this.httpClient != null && builder != null)
        {
            this.Disposables.Add(builder);
        }
        else
        {
            this.httpClient?.CancelPendingRequests();
            this.httpClient?.Dispose();
            this.httpClient = null;
            builder?.Dispose();
        }
    }

    /// <summary>
    /// Overridable method that is called inside the disposal process.
    /// </summary>
    protected virtual void DisposeChild()
    {
    }

    /// <summary>
    /// Implementation of the disposal as called by IDisposable.Dispose.
    /// </summary>
    /// <param name="disposing">True if called by Dispose; False if called by Destructor.</param>
    protected void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.httpClient?.CancelPendingRequests();
                this.httpClient?.Dispose();
                this.httpClient = null;

                this.DataBag
                    .Values
                    .Where(x => x != null)
                    .OfType<IDisposable>()
                    .ToList()
                    .ForEach(d => d.Dispose());

                this.disposables
                    .Where(x => x != null)
                    .OfType<IDisposable>()
                    .ToList()
                    .ForEach(d => d.Dispose());

                this.DisposeChild();

                if (this.IsPreparedOnly)
                {
                    Assert.Inconclusive("The test is considered inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
                }
            }

            this.disposedValue = true;
        }
    }
}
