// <copyright file="ExceptionStoreItem.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Stores the captured exceptions and rates them.
/// </summary>
internal class ExceptionStoreItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionStoreItem"/> class.
    /// </summary>
    /// <param name="runCategory">Section where exception was thrown.</param>
    /// <param name="exception">Exception thrown.</param>
    public ExceptionStoreItem(RunCategory runCategory, Exception exception)
    {
        this.RunCategory = runCategory;
        this.Exception = exception;
        this.IsCheckedInThenClause = false;
    }

    /// <summary>
    /// Gets or sets the run category.
    /// </summary>
    public RunCategory RunCategory { get; set; }

    /// <summary>
    /// Gets or sets the exception to be thrown.
    /// </summary>
    public Exception Exception { get; set; }

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
        return $"({this.RunCategory}, {this.Exception.GetType().Name}, {this.IsCheckedInThenClauseName})";
    }
}
