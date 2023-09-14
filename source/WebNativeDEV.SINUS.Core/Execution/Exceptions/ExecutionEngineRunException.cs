// <copyright file="ExecutionEngineRunException.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents exceptions that occured during the run in the execution engine.
/// </summary>
public class ExecutionEngineRunException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngineRunException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ExecutionEngineRunException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngineRunException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">The source exception.</param>
    public ExecutionEngineRunException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionEngineRunException"/> class.
    /// </summary>
    public ExecutionEngineRunException()
    {
    }
}
