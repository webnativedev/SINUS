﻿// <copyright file="ExceptionStoreItem.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using System;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Stores the captured exceptions and rates them.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExceptionStoreItem"/> class.
/// </remarks>
/// <param name="runCategory">Section where exception was thrown.</param>
/// <param name="exception">Exception thrown.</param>
internal class ExceptionStoreItem(RunCategory runCategory, Exception exception)
{
    /// <summary>
    /// Gets or sets the run category.
    /// </summary>
    public RunCategory RunCategory { get; set; } = runCategory;

    /// <summary>
    /// Gets or sets the exception to be thrown.
    /// </summary>
    public Exception Exception { get; set; } = exception;

    /// <summary>
    /// Gets or sets a value indicating whether the exception is checked (=expected by unit test).
    /// </summary>
    public bool IsCheckedInThenClause { get; set; }

    /// <summary>
    /// Gets the readable state whether it was an expected/checked exception or not.
    /// </summary>
    public string IsCheckedInThenClauseName => this.IsCheckedInThenClause ? "checked" : "error";

    /// <summary>
    /// Readable representation of the item.
    /// </summary>
    /// <returns>The plain text readable string.</returns>
    public string GetTupleString()
    {
        return $"({this.RunCategory}, {this.Exception.GetType().Name}, '{this.Exception.Message}', {this.IsCheckedInThenClauseName})";
    }
}
