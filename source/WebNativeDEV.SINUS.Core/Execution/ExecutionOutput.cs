// <copyright file="ExecutionOutput.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents a class used as output for the execution engine implementation.
/// </summary>
public sealed class ExecutionOutput
{
    /// <summary>
    /// Gets the exceptions that were raised during execution.
    /// </summary>
    public IList<Exception> Exceptions { get; } = new List<Exception>();

    /// <summary>
    /// Gets or sets the HttpClient created for the execution.
    /// </summary>
    public HttpClient? HttpClient { get; internal set; }

    /// <summary>
    /// Gets or sets the Web Application Factory as IDisposable.
    /// </summary>
    public IDisposable? WebApplicationFactory { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether the execution is only prepared.
    /// </summary>
    public bool IsPreparedOnly { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether the execution should run if exceptions were already raised.
    /// </summary>
    public bool ShouldRunIfAlreadyExceptionOccured { get; internal set; }
}
