// <copyright file="IExecutionOutput.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Contracts;

using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// The execution output of the execution engine.
/// </summary>
public interface IExecutionOutput
{
    /// <summary>
    /// Gets a list of exceptions.
    /// </summary>
    IList<Exception> Exceptions { get; }

    /// <summary>
    /// Gets a value indicating whether the test is classified as prepared only.
    /// </summary>
    bool IsPreparedOnly { get; }

    /// <summary>
    /// Gets the section in which the exceptions occured.
    /// </summary>
    RunCategory RunCategory { get; }
}