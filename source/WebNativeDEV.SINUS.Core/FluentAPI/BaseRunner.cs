// <copyright file="BaseRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Extensions;
using WebNativeDEV.SINUS.Core.MsTest.Sut;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Base Class for Runners.
/// </summary>
internal abstract class BaseRunner : IDisposable
{
    private const int RetryCountCreatingSut = 6;

    private readonly List<IDisposable> disposables;
    private bool disposedValue;
    private HttpClient? httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRunner"/> class.
    /// </summary>
    /// <param name="testBase">Reference to the test base creating the runner.</param>
    protected BaseRunner(TestBase testBase)
    {
        this.TestBase = testBase;
        this.Logger = this.TestBase.LoggerFactory.CreateLogger<BrowserRunner>();
        this.Logger.LogInformation("Created a log for base-runner");

        this.DataBag = new RunStore(this.TestBase.LoggerFactory);
        this.Exceptions = new List<(RunCategory, Exception)>();
        this.disposables = new List<IDisposable>();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the test is only a placeholder for later or not.
    /// </summary>
    protected bool IsPreparedOnly { get; set; }

    /// <summary>
    /// Gets the exceptions that occured during the execution.
    /// </summary>
    protected List<(RunCategory, Exception)> Exceptions { get; }

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
    protected RunStore DataBag { get; }

    /// <summary>
    /// Gets the reference to the TestBase that creates the runner.
    /// </summary>
    protected TestBase TestBase { get; }

    /// <summary>
    /// Gets the logger that can be used to print information.
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// Disposes the object as defined in IDisposable.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Basic execution of each step.
    /// </summary>
    /// <param name="category">Part of the flow (given, when, then).</param>
    /// <param name="description">Human readable description what is going to be performed.</param>
    /// <param name="action">The action to be executed.</param>
    /// <param name="shouldRunIfAlreadyExceptionOccured">Indicates whether execution should take place or not.</param>
    /// <returns>A reference to the runner for Fluent API purpose.</returns>
    protected BaseRunner Run(RunCategory category, string description, Action action, bool shouldRunIfAlreadyExceptionOccured)
    {
        using (this.Logger.CreatePerformanceDataScope(category.ToString(), description))
        {
#pragma warning disable CA1031 // do not catch general exception types
            try
            {
                if (this.Exceptions.Count == 0 || shouldRunIfAlreadyExceptionOccured)
                {
                    action?.Invoke();
                }
                else
                {
                    this.Logger.LogInformation(
                        "execution of {Category} skipped, because exceptions are already tracked",
                        category);
                }
            }
            catch (Exception exc)
            {
                this.Logger.LogError(
                    exc,
                    "Exception occured in execution of {Category} - {ExcClass}: {ExcMessage}",
                    category,
                    exc.GetType().ToString(),
                    exc.Message);
                this.Logger.LogError("Stacktrace: {StackTrace}", exc.StackTrace);
                this.Exceptions.Add((category, exc));
            }
#pragma warning restore CA1031 // do not catch general exception types
        }

        return this;
    }

    /// <summary>
    /// Basic execution of each step.
    /// </summary>
    /// <param name="category">Part of the flow (given, when, then).</param>
    /// <param name="description">Human readable description what is going to be performed.</param>
    /// <param name="actions">The array of action to be executed.</param>
    /// <param name="shouldRunIfAlreadyExceptionOccured">Indicates whether execution should take place or not.</param>
    /// <returns>A reference to the runner for Fluent API purpose.</returns>
    protected BaseRunner Run(RunCategory category, string description, IList<Action> actions, bool shouldRunIfAlreadyExceptionOccured)
    {
        var actionCount = actions.Count;
        for (int i = 0; i < actionCount; i++)
        {
            var action = actions[i];
            string prefix = actionCount == 1
                ? string.Empty
                : $"{i + 1:00}: ";

            this.Run(
                category,
                $"{prefix}{description}",
                () => action?.Invoke(),
                shouldRunIfAlreadyExceptionOccured);
        }

        return this;
    }

    /// <summary>
    /// Creates a system under test.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the Sut.</typeparam>
    /// <param name="endpoint">If set the public endpoint, else an in-memory Sut is created.</param>
    protected void CreateSut<TProgram>(string? endpoint = null)
        where TProgram : class
    {
        IDisposable? builder = null;
        this.httpClient = null;
        for (int i = 0; i < RetryCountCreatingSut; i++)
        {
            try
            {
                // if endpoint is null then in-memory, else public
                var waf = new SinusWebApplicationFactory<TProgram>(endpoint);
                this.httpClient = waf.CreateClient();
                builder = waf;

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
    /// Implementation of the disposal as called by IDisposable.Dispose.
    /// </summary>
    /// <param name="disposing">True if called by Dispose; False if called by Destructor.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Run(
                    RunCategory.Dispose,
                    "Dispose objects",
                    () =>
                    {
                        this.httpClient?.CancelPendingRequests();
                        this.httpClient?.Dispose();
                        this.httpClient = null;

                        this.DataBag.DisposeAllDisposables();

                        this.disposables
                            .Where(x => x != null)
                            .OfType<IDisposable>()
                            .ToList()
                            .ForEach(d => d.Dispose());
                    },
                    true);

                if (this.IsPreparedOnly)
                {
                    Assert.Inconclusive("The test is considered inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
                }

                if (this.Exceptions.Any())
                {
                    Assert.Fail($"Exceptions occured. Count: {this.Exceptions.Count}");
                }
            }

            this.disposedValue = true;
        }
    }
}
