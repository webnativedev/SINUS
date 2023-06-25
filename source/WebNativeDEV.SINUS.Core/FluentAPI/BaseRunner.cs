// <copyright file="BaseRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Base Class for Runners.
/// </summary>
internal abstract class BaseRunner : IDisposable
{
    private readonly List<IDisposable> disposables;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRunner"/> class.
    /// </summary>
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    public BaseRunner(ILoggerFactory loggerFactory)
    {
        this.Logger = loggerFactory.CreateLogger<BrowserRunner>();
        this.Logger.LogDebug("Created a log runner");

        this.DataBag = new Dictionary<string, object?>();
        this.disposables = new List<IDisposable>();
    }

    /// <summary>
    /// Gets the list of disposables (= the list of objects to automatically dispose).
    /// </summary>
    protected List<IDisposable> Disposables => this.disposables;

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
    /// Implementation of the disposal as called by IDisposable.Dispose.
    /// </summary>
    /// <param name="disposing">True if called by Dispose; False if called by Destructor.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
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
            }

            this.disposedValue = true;
        }
    }
}
