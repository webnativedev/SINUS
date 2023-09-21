// <copyright file="ExceptionChangedEventBusEventArgs.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Events;

using System;
using WebNativeDEV.SINUS.Core.Events.EventArguments;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Exception handling event args.
/// </summary>
public class ExceptionChangedEventBusEventArgs : EventBusEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionChangedEventBusEventArgs"/> class.
    /// </summary>
    /// <param name="item">A raw item from the exception store.</param>
    internal ExceptionChangedEventBusEventArgs(ExceptionStoreItem item)
    {
        this.RunCategory = item.RunCategory;
        this.Exception = item.Exception;
        this.IsChecked = item.IsCheckedInThenClause;
    }

    /// <summary>
    /// Gets the run category.
    /// </summary>
    public RunCategory RunCategory { get; }

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Gets a value indicating whether this exception is an expected exception (=already checked).
    /// </summary>
    public bool IsChecked { get; }
}
