﻿// <copyright file="IThenBrowser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents in the given-when-then sequence the then part
/// especially with focus on browser usage.
/// This interface allows to create a proper Fluent API.
/// </summary>
public interface IThenBrowser : IDisposable
{
    /// <summary>
    /// Method that can be used to write further debug information in case we want to write more details
    /// to the log.
    /// </summary>
    /// <param name="action">Pass in your debug code here.</param>
    /// <returns>An instance to the runner, so it can be disposed.</returns>
    IDisposable Debug(Action<IBrowser, Dictionary<string, object?>>? action = null);
}
