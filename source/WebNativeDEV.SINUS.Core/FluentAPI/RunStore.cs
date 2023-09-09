// <copyright file="RunStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Events;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Represents the store that is used in test runners.
/// </summary>
public class RunStore : IRunStore
{
    /// <summary>
    /// Standard Key name for the actual value.
    /// </summary>
    public const string KeyActual = "actual";

    /// <summary>
    /// Standard Key name for the system-under-test value.
    /// </summary>
    public const string KeySut = "SystemUnderTest";

    private readonly Dictionary<string, object?> store = new();
    private readonly ILogger logger;
    private readonly IEventBus eventBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunStore"/> class.
    /// </summary>
    public RunStore(IEventBus eventBus)
    {
        this.logger = TestBase.Container.Resolve<ILoggerFactory>().CreateLogger<RunStore>();
        this.eventBus = eventBus;
    }

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

    /// <summary>
    /// Stores data into to the store.
    /// </summary>
    /// <param name="key">Unique key that identifies the value.</param>
    /// <param name="item">An instance to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    /// <exception cref="ArgumentNullException">Item should not be null.</exception>
    public RunStore Store(string key, object? item)
    {
        var containsKey = this.store.ContainsKey(key);
        var oldValue = !containsKey ? null : this.store[key];
        this.store[key] = item;

        this.eventBus.Publish(this, new RunStoreDataStoredEventBusEventArgs(key, item, !containsKey, oldValue));

        return this;
    }

    /// <summary>
    /// Stores an instance without the need of a key.
    /// The key will be generated using GUIDs.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore Store(object? item)
    {
        return this.Store(Guid.NewGuid().ToString(), item);
    }

    /// <summary>
    /// Stores the value that is actually calculated normally in a When block.
    /// </summary>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore InitActual()
    {
        return this.Store(KeyActual, null);
    }

    /// <summary>
    /// Stores the value that is actually calculated normally in a When block.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore StoreActual(object? item)
    {
        return this.Store(KeyActual, item);
    }

    /// <summary>
    /// Stores the value that is actually calculated in When.
    /// </summary>
    /// <typeparam name="TSut">Type of the system under test.</typeparam>
    /// <param name="calculationFunction">The block calculating the actual value.</param>
    /// <returns>An instance of the store to support the fluent api style.</returns>
    public RunStore StoreActual<TSut>(Func<TSut, object?> calculationFunction)
    {
        return this.StoreActual(calculationFunction?.Invoke(this.ReadSut<TSut>()));
    }

    /// <summary>
    /// Stores the System under test.
    /// </summary>
    /// <param name="systemUnderTest">The system under test.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore StoreSut(object? systemUnderTest)
    {
        return this.Store(KeySut, systemUnderTest);
    }

    /// <summary>
    /// Gets the instance from store based on the key.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <param name="key">Identifier for the instance to get.</param>
    /// <returns>An instance from the store.</returns>
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

    /// <summary>
    /// Gets the instance from store based on the key as object.
    /// </summary>
    /// <param name="key">Identifier for the instance to get.</param>
    /// <returns>An instance from the store as object.</returns>
    public object? ReadObject(string key)
    {
        return this.store[key];
    }

    /// <summary>
    /// Reads the value that is returned by the system under test as a result.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <returns>An instance from the store.</returns>
    public T ReadActual<T>()
    {
        return this.Read<T>(KeyActual);
    }

    /// <summary>
    /// Reads the value that is returned by the system under test as a result.
    /// </summary>
    /// <returns>An instance from the store.</returns>
    public object? ReadActualObject()
    {
        return this.ReadObject(KeyActual);
    }

    /// <summary>
    /// Reads the value that represents the system under.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <returns>An instance from the store.</returns>
    public T ReadSut<T>()
    {
        return this.Read<T>(KeySut);
    }

    /// <summary>
    /// Reads the value that represents the system under test as object.
    /// </summary>
    /// <returns>An instance from the store as object.</returns>
    public object? ReadSutObject()
    {
        return this.ReadObject(KeySut);
    }

    /// <summary>
    /// Prints the content to the logger.
    /// </summary>
    /// <returns>The RunStore for a fluent api.</returns>
    public RunStore PrintStore()
    {
        return this.Print(this.store);
    }

    /// <summary>
    /// Prints the content to the logger.
    /// </summary>
    /// <param name="key">The key to print.</param>
    /// <param name="value">The value to print.</param>
    /// <returns>The RunStore for a fluent api.</returns>
    public RunStore PrintAdditional(string key, object? value)
    {
        this.logger.LogInformation(
            "| {Key}: {Value} (Type: {Type})",
            key ?? "<null>",
            value ?? "<null>",
            value?.GetType()?.FullName ?? "<null>");

        return this;
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
    /// <returns>The RunStore for a fluent api.</returns>
    private RunStore Print(Dictionary<string, object?> data)
    {
        this.logger.LogInformation("+----------------------------");
        this.logger.LogInformation("| Count: {Count}", data.Keys.Count);
        this.logger.LogInformation("+----------------------------");

        foreach (var key in data.Keys)
        {
            this.logger.LogInformation("| {Key}: {Value} (Type: {Type})", key, data[key] ?? "<null>", data[key]?.GetType()?.FullName ?? "<null>");
        }

        this.logger.LogInformation("+----------------------------");

        return this;
    }
}
