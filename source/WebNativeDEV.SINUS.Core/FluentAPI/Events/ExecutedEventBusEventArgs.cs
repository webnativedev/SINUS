// <copyright file="ExecutedEventBusEventArgs.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.Events.EventArguments;
using WebNativeDEV.SINUS.Core.Execution.Contracts;

/// <summary>
/// Event Arg for execution event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExecutedEventBusEventArgs"/> class.
/// </remarks>
/// <param name="output">The output of the execution.</param>
public class ExecutedEventBusEventArgs(IExecutionOutput output) : EventBusEventArgs()
{
    /// <summary>
    /// Gets the Execution output.
    /// </summary>
    public IExecutionOutput Output { get; } = output;
}
