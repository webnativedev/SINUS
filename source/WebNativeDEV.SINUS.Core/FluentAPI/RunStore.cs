// <copyright file="RunStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the store that is used in test runners.
/// </summary>
public class RunStore
{
    private const string KeyActual = "actual";
    private const string KeySut = "SystemUnderTest";

    private readonly Dictionary<string, object> store = new();
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunStore"/> class.
    /// </summary>
    /// <param name="loggerFactory">Factory to create an own logger instance.</param>
    public RunStore(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<RunStore>();
    }

    /// <summary>
    /// Indexer that can be used instead of Read and Store methods.
    /// </summary>
    /// <param name="key">The key to find values.</param>
    /// <returns>An object represented by the key.</returns>
    public object this[string key]
    {
        get
        {
            return this.Read<object>(key);
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
    public RunStore Store(string key, object item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "item should not be null");
        }

        this.store.Add(key, item);
        return this;
    }

    /// <summary>
    /// Stores the value that is actually calculated normally in a When block.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore StoreActual(object item)
    {
        return this.Store(KeyActual, item);
    }

    /// <summary>
    /// Stores an instance without the need of a key.
    /// The key will be generated using GUIDs.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore Store(object item)
    {
        return this.Store(Guid.NewGuid().ToString(), item);
    }

    /// <summary>
    /// Gets the instance from store based on the key.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <param name="key">Identifier for the instance to get.</param>
    /// <returns>An instance from the store.</returns>
    public T Read<T>(string key)
    {
        var result = this.store[key];
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

        return (T)result.First();
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
    /// Reads the value that is returned by the system under test as a result.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <returns>An instance from the store.</returns>
    public T ReadActual<T>()
    {
        return this.Read<T>(KeyActual);
    }

    /// <summary>
    /// Stores the System under test.
    /// </summary>
    /// <param name="systemUnderTest">The system under test.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    public RunStore StoreSut(object systemUnderTest)
    {
        return this.Store(KeySut, systemUnderTest);
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
    /// Prints the content to the logger.
    /// </summary>
    public void Print()
    {
        this.logger.LogInformation("+----------------------------");

        foreach(var key in this.store.Keys)
        {
            this.logger.LogInformation("| {Key}: {Value}", key, this.store[key]);
        }

        this.logger.LogInformation("+----------------------------");
    }
}
