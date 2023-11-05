// <copyright file="IExceptionStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using System;
using System.Collections.Generic;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// The exception store interface.
/// </summary>
public interface IExceptionStore
{
    /// <summary>
    /// Gets the count of exceptions in the store.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Gets a value indicating whether elements exist that are unchecked.
    /// </summary>
    bool HasUncheckedElements { get; }

    /// <summary>
    /// Count elements of type.
    /// </summary>
    /// <typeparam name="T">Identifying exception type.</typeparam>
    /// <returns>The count as integer.</returns>
    int CountOfType<T>()
        where T : Exception;

    /// <summary>
    /// Adds an exception classified in a section.
    /// </summary>
    /// <param name="runCategory">Section identifier.</param>
    /// <param name="exception">Exception to store.</param>
    void Add(RunCategory runCategory, Exception exception);

    /// <summary>
    /// Gets a string representation of the full store content.
    /// </summary>
    /// <returns>A tuple based plain text.</returns>
    string? GetContentAsString();

    /// <summary>
    /// Set all items as checked.
    /// </summary>
    void SetAllChecked();

    /// <summary>
    /// Set all items of type as checked.
    /// </summary>
    /// <typeparam name="T">Identifying type.</typeparam>
    void SetAllCheckedOfType<T>()
        where T : Exception;

    /// <summary>
    /// Check if an element in the store exists.
    /// </summary>
    /// <returns>Whether count > 0.</returns>
    bool Any();
}
