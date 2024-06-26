﻿// <copyright file="RunStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;

/// <summary>
/// Represents the store that is used in test runners.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RunStore"/> class.
/// </remarks>
/// <param name="scope">The scoped dependencies.</param>
internal sealed class RunStore(TestBaseScopeContainer scope) : IRunStore
{
    private readonly ConcurrentDictionary<string, object?> store = new();
    private readonly ILogger logger = TestBaseSingletonContainer.CreateLogger<RunStore>();

    /// <inheritdoc/>
    public string KeyActual => "actual";

    /// <inheritdoc/>
    public string KeySut => "SystemUnderTest";

    /// <inheritdoc/>
    public string? TestName => scope?.TestName;

    /// <summary>
    /// Gets or sets the actual value.
    /// </summary>
    public object? Actual
    {
        get => this.ReadActualObject();
        set => this.StoreActual(value);
    }

    /// <summary>
    /// Gets or sets the system under test.
    /// </summary>
    public object? Sut
    {
        get => this.ReadSutObject();
        set => this.StoreSut(value);
    }

    /// <summary>
    /// Indexer that can be used instead of Read and Store methods.
    /// </summary>
    /// <param name="key">The key to find values.</param>
    /// <returns>An object represented by the key.</returns>
    public object? this[string key]
    {
        get
        {
            return this.ReadObject(key);
        }

        set
        {
            this.Store(key, value);
        }
    }

    /// <inheritdoc/>
    public IRunStore Store(string key, object? item)
    {
        var containsKey = this.store.ContainsKey(key);
        var oldValue = !containsKey ? null : this.store[key];
        this.store[key] = item;

        scope.EventBus?.Publish(this, new RunStoreDataStoredEventBusEventArgs(key, item, !containsKey, oldValue));

        return this;
    }

    /// <inheritdoc/>
    public IRunStore Store(object? item)
    {
        return this.Store(Guid.NewGuid().ToString(), item);
    }

    /// <inheritdoc/>
    public IRunStore StoreLog(string log)
    {
        string keyTime = DateTime.Now.ToString("HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
        string uniqueGen = Guid.NewGuid().ToString("N")[..8];
        string key = $"{keyTime}_{uniqueGen}";
        return this.Store(key, log);
    }

    /// <summary>
    /// Stores the value that is actually calculated normally in a When block.
    /// </summary>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public IRunStore InitActual()
    {
        return this.Store(this.KeyActual, null);
    }

    /// <inheritdoc/>
    public IRunStore StoreActual(object? item)
    {
        return this.Store(this.KeyActual, item);
    }

    /// <inheritdoc/>
    public IRunStore StoreActual<TSut>(Func<TSut, object?> calculationFunction)
    {
        return this.StoreActual(calculationFunction?.Invoke(this.ReadSut<TSut>()));
    }

    /// <inheritdoc/>
    public IRunStore StoreSut(object? systemUnderTest)
    {
        return this.Store(this.KeySut, systemUnderTest);
    }

    /// <inheritdoc/>
    public T Read<T>(string key)
    {
        var result = this.ReadObject(key);
        if (result is not T)
        {
            throw new InvalidCastException("expected result is of different type");
        }

        return (T)result;
    }

    /// <summary>
    /// Gets the instance from store based on the type (that can only appear once in the store).
    /// </summary>
    /// <typeparam name="T">Type the result is searched for, checked against and casted into.</typeparam>
    /// <returns>An instance from the store.</returns>
    public T Read<T>()
    {
        var result = this.store.Values.OfType<T>().ToList();
        if (result.Count != 1)
        {
            throw new InvalidOperationException("no distinct result possible");
        }

        return result[0];
    }

    /// <inheritdoc/>
    public IList<object?> ReadPrefix(string prefix)
    {
        return this.store
                .Where(x => x.Key.StartsWith(prefix, StringComparison.InvariantCulture))
                .Select(x => x.Value)
                .ToList();
    }

    /// <inheritdoc/>
    public object? ReadObject(string key)
    {
        if (!this.store.TryGetValue(key, out object? value))
        {
            return null;
        }

        return value;
    }

    /// <inheritdoc/>
    public T ReadActual<T>()
    {
        return this.Read<T>(this.KeyActual);
    }

    /// <inheritdoc/>
    public object? ReadActualObject()
    {
        return this.ReadObject(this.KeyActual);
    }

    /// <inheritdoc/>
    public T ReadSut<T>()
    {
        return this.Read<T>(this.KeySut);
    }

    /// <inheritdoc/>
    public object? ReadSutObject()
    {
        return this.ReadObject(this.KeySut);
    }

    /// <summary>
    /// Prints the content to the logger.
    /// </summary>
    /// <param name="order">The field to order the data for printing.</param>
    /// <returns>The RunStore for a fluent api.</returns>
    public IRunStore PrintStore(RunStorePrintOrder order = RunStorePrintOrder.KeySorted)
        => this.PrintImplementation(this.store, order);

    /// <summary>
    /// Prints the content to the logger.
    /// </summary>
    /// <param name="key">The key to print.</param>
    /// <param name="value">The value to print.</param>
    /// <returns>The RunStore for a fluent api.</returns>
    public IRunStore PrintAdditional(string key, object? value)
    {
        this.logger.LogInformation(
            "| {Key}: {Value} (Type: {Type})",
            key ?? LoggerConstants.NullString,
            value ?? LoggerConstants.NullString,
            value?.GetType()?.FullName ?? LoggerConstants.NullString);

        return this;
    }

    /// <inheritdoc/>
    public int Count(Func<KeyValuePair<string, object?>, bool> condition)
    {
        int count = 0;

        foreach (var item in this.store)
        {
            count += condition(item) ? 1 : 0;
        }

        return count;
    }

    /// <inheritdoc/>
    public void WaitUntil(Func<IRunStore, bool> condition, int timeoutInMs = 5000)
    {
        var waitTask = Task.Run(async () =>
        {
            while (condition?.Invoke(this) ?? true)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        });

        Task.Run(async () =>
        {
            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeoutInMs)).ConfigureAwait(false))
            {
                throw new TimeoutException();
            }
        }).Wait();
    }

    /// <summary>
    /// Disposes all items that are IDisposable.
    /// </summary>
    public void DisposeAllDisposables()
    {
        this.store
            .Values
            .Where(x => x != null)
            .OfType<IDisposable>()
            .ToList()
            .ForEach(d => d.Dispose());
    }

    /// <summary>
    /// Prints the content to the logger.
    /// </summary>
    /// <param name="data">The key/value pair to print.</param>
    /// <param name="order">The field to order the data for printing.</param>
    /// <returns>The RunStore for a fluent api.</returns>
    private RunStore PrintImplementation(IDictionary<string, object?> data, RunStorePrintOrder order)
    {
        var builder = new StringBuilder(10000);

        builder.AppendLine("Content dump:");
        builder.AppendLine(LoggerConstants.SeparationLine);
        builder.AppendLine(CultureInfo.InvariantCulture, $"| Count: {data.Keys.Count}");
        builder.AppendLine(LoggerConstants.SeparationLine);

        IEnumerable<KeyValuePair<string, string>> dataList = data
            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString() ?? LoggerConstants.NullString))
            .ToList();

        switch (order)
        {
            case RunStorePrintOrder.Unsorted:
                break;
            case RunStorePrintOrder.KeySorted:
                dataList = dataList.OrderBy(dataItem => dataItem.Key);
                break;
            case RunStorePrintOrder.ValueSorted:
                dataList = dataList.OrderBy(dataItem => dataItem.Value);
                break;
        }

        foreach (var dataItem in dataList)
        {
            string typeName = dataItem.Value?.GetType()?.FullName ?? LoggerConstants.NullString;
            builder.AppendLine(CultureInfo.InvariantCulture, $"| {dataItem.Key}: {dataItem.Value} (Type: {typeName})");
        }

        builder.AppendLine(LoggerConstants.SeparationLine);

        this.logger.LogInformation("{StoreContent}", builder.ToString());

        return this;
    }
}
