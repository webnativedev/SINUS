﻿// <copyright file="IThen.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;

using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Represents in the given-when-then sequence the then part.
/// This interface allows to create a proper Fluent API.
/// </summary>
public interface IThen : IDisposable
{
    /// <summary>
    /// Method that can be used to write further debug information in case we want to write more details
    /// to the log.
    /// </summary>
    /// <param name="action">Pass in your debug code here.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IThen Debug(Action<IRunStore>? action = null);

    /// <summary>
    /// Method that is used to write all debug information to the logger.
    /// </summary>
    /// <param name="order">The field to order the data for printing.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IThen DebugPrint(RunStorePrintOrder order = RunStorePrintOrder.KeySorted);

    /// <summary>
    /// Method that is used to write all debug information and additional data to the logger.
    /// </summary>
    /// <param name="order">The field to order the data for printing.</param>
    /// <param name="additionalData">Data that needs to be printed for information additionally.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IThen DebugPrint(RunStorePrintOrder order, (string, object?)[] additionalData);

    /// <summary>
    /// Method that is used to write all debug information and additional data to the logger.
    /// </summary>
    /// <param name="additionalData">Data that needs to be printed for information additionally.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IThen DebugPrint((string, object?)[] additionalData);

    /// <summary>
    /// Method that is used to write all debug information and additional data to the logger.
    /// </summary>
    /// <param name="order">The field to order the data for printing.</param>
    /// <param name="key">The key from a key-value pair to add in the debug printing.</param>
    /// <param name="value">The value from a key-value pair to add in the debug printing.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IThen DebugPrint(RunStorePrintOrder order, string key, object? value);

    /// <summary>
    /// Method that is used to write all debug information and additional data to the logger.
    /// </summary>
    /// <param name="key">The key from a key-value pair to add in the debug printing.</param>
    /// <param name="value">The value from a key-value pair to add in the debug printing.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IThen DebugPrint(string key, object? value);

    /// <summary>
    /// Method that is used to set the expectation of the test to fail.
    /// </summary>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IDisposable ExpectFail();

    /// <summary>
    /// Method that is used to set the expectation of the test to inconclusive.
    /// </summary>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IDisposable ExpectInconclusive();
}
