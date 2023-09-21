// <copyright file="RunStoreDataStoredEventBusEventArgs.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.Events.EventArguments;

/// <summary>
/// Event args clas for data changes.
/// </summary>
public class RunStoreDataStoredEventBusEventArgs : EventBusEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunStoreDataStoredEventBusEventArgs"/> class.
    /// </summary>
    /// <param name="key">Information Key in the data store.</param>
    /// <param name="value">Information value in the data store.</param>
    /// <param name="isNew">Indicates whether the information is new in the store.</param>
    /// <param name="oldValue">Previous value in case the value is not new.</param>
    public RunStoreDataStoredEventBusEventArgs(string key, object? value, bool isNew, object? oldValue)
    {
        this.Key = key;
        this.Value = value;
        this.IsNew = isNew;
        this.OldValue = oldValue;
    }

    /// <summary>
    /// Gets the key of the information in the data store.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the value of the information in the data store.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets a value indicating whether this information
    /// was newly created in the data store or only updated.
    /// </summary>
    public bool IsNew { get; }

    /// <summary>
    /// Gets the old value before an update.
    /// </summary>
    public object? OldValue { get; }
}
