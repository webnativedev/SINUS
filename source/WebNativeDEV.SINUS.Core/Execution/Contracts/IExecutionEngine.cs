// <copyright file="IExecutionEngine.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Contracts;

/// <summary>
/// Represents an interface to the execution engine.
/// </summary>
internal interface IExecutionEngine
{
    /// <summary>
    /// Runs the execution.
    /// </summary>
    /// <param name="parameter">The input value object.</param>
    /// <returns>An output value object.</returns>
    ExecutionOutput Run(ExecutionParameter? parameter);
}