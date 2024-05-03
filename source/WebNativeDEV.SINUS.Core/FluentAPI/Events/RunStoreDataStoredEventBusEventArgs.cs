// <copyright file="RunStoreDataStoredEventBusEventArgs.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.Events.EventArguments;

/// <summary>
/// Event args clas for data changes.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RunStoreDataStoredEventBusEventArgs"/> class.
/// </remarks>
/// <param name="key">Information Key in the data store.</param>
/// <param name="value">Information value in the data store.</param>
/// <param name="isNew">Indicates whether the information is new in the store.</param>
/// <param name="oldValue">Previous value in case the value is not new.</param>
public class RunStoreDataStoredEventBusEventArgs(string key, object? value, bool isNew, object? oldValue) : EventBusEventArgs
{
    /// <summary>
    /// Gets the key of the information in the data store.
    /// </summary>
    public string Key { get; } = key;

    /// <summary>
    /// Gets the value of the information in the data store.
    /// </summary>
    public object? Value { get; } = value;

    /// <summary>
    /// Gets a value indicating whether this information
    /// was newly created in the data store or only updated.
    /// </summary>
    public bool IsNew { get; } = isNew;

    /// <summary>
    /// Gets the old value before an update.
    /// </summary>
    public object? OldValue { get; } = oldValue;
}
