// <copyright file="ExecutedEventBusEventArgs.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Events.EventArguments;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;

/// <summary>
/// Event Arg for execution event.
/// </summary>
public class ExecutedEventBusEventArgs : EventBusEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutedEventBusEventArgs"/> class.
    /// </summary>
    /// <param name="output">The output of the execution.</param>
    public ExecutedEventBusEventArgs(IExecutionOutput output)
        : base()
    {
        this.Output = output;
    }

    /// <summary>
    /// Gets the Execution output.
    /// </summary>
    public IExecutionOutput Output { get; }
}
