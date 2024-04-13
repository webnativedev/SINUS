// <copyright file="IRunStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

using WebNativeDEV.SINUS.Core.FluentAPI.Model;

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

/// <summary>
/// Interface representing a run store.
/// </summary>
public interface IRunStore
{
    /// <summary>
    /// Gets the Key-String of "Actual".
    /// </summary>
    string KeyActual { get; }

    /// <summary>
    /// Gets the Key-String of "Sut".
    /// </summary>
    string KeySut { get; }

    /// <summary>
    /// Gets the tests name (including scenario adaption).
    /// </summary>
    string? TestName { get; }

    /// <summary>
    /// Gets or sets the actual value.
    /// </summary>
    object? Actual { get; set; }

    /// <summary>
    /// Gets or sets the system under test.
    /// </summary>
    object? Sut { get; set; }

    /// <summary>
    /// Gets or sets the data of key.
    /// </summary>
    /// <param name="key">The unique identifier of an information.</param>
    /// <returns>The value behind the key.</returns>
    object? this[string key] { get; set; }

    /// <summary>
    /// Disposes all disposables stored in the data store.
    /// </summary>
    void DisposeAllDisposables();

    /// <summary>
    /// Initializes actual with null.
    /// </summary>
    /// <returns>A reference to the data store for fluent usage.</returns>
    IRunStore InitActual();

    /// <summary>
    /// Prints additional data in the given format.
    /// </summary>
    /// <param name="key">The key of the information.</param>
    /// <param name="value">The value of the information.</param>
    /// <returns>A reference to the data store for fluent usage.</returns>
    IRunStore PrintAdditional(string key, object? value);

    /// <summary>
    /// Prints the store content.
    /// </summary>
    /// <param name="order">The field to order the data for printing.</param>
    /// <returns>A reference to the data store for fluent usage.</returns>
    IRunStore PrintStore(RunStorePrintOrder order = RunStorePrintOrder.KeySorted);

    /// <summary>
    /// Reads an information if it is the only information with type T.
    /// </summary>
    /// <typeparam name="T">Unique type of the value inside the data store.</typeparam>
    /// <returns>The information value.</returns>
    T Read<T>();

    /// <summary>
    /// Gets the instance from store based on the key.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <param name="key">Identifier for the instance to get.</param>
    /// <returns>An instance from the store.</returns>
    T Read<T>(string key);

    /// <summary>
    /// Gets the instances from store based on the key.
    /// </summary>
    /// <param name="prefix">Identifier prefix for the instance to get.</param>
    /// <returns>An instance list from the store.</returns>
    IList<object?> ReadPrefix(string prefix);

    /// <summary>
    /// Reads the value that is returned by the system under test as a result.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <returns>An instance from the store.</returns>
    T ReadActual<T>();

    /// <summary>
    /// Reads the value that is returned by the system under test as a result.
    /// </summary>
    /// <returns>An instance from the store.</returns>
    object? ReadActualObject();

    /// <summary>
    /// Gets the instance from store based on the key as object.
    /// </summary>
    /// <param name="key">Identifier for the instance to get.</param>
    /// <returns>An instance from the store as object.</returns>
    object? ReadObject(string key);

    /// <summary>
    /// Reads the value that represents the system under.
    /// </summary>
    /// <typeparam name="T">Type the result is checked against and casted into.</typeparam>
    /// <returns>An instance from the store.</returns>
    T ReadSut<T>();

    /// <summary>
    /// Reads the value that represents the system under test as object.
    /// </summary>
    /// <returns>An instance from the store as object.</returns>
    object? ReadSutObject();

    /// <summary>
    /// Stores an instance without the need of a key.
    /// The key will be generated using GUIDs.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    IRunStore Store(object? item);

    /// <summary>
    /// Stores data into to the store.
    /// </summary>
    /// <param name="key">Unique key that identifies the value.</param>
    /// <param name="item">An instance to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    /// <exception cref="ArgumentNullException">Item should not be null.</exception>
    IRunStore Store(string key, object? item);

    /// <summary>
    /// Stores log entry into to the store.
    /// </summary>
    /// <param name="log">Item to log</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    IRunStore StoreLog(string log);

    /// <summary>
    /// Stores the value that is actually calculated normally in a When block.
    /// </summary>
    /// <param name="item">The item to store.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    IRunStore StoreActual(object? item);

    /// <summary>
    /// Stores the value that is actually calculated in When.
    /// </summary>
    /// <typeparam name="TSut">Type of the system under test.</typeparam>
    /// <param name="calculationFunction">The block calculating the actual value.</param>
    /// <returns>An instance of the store to support the fluent api style.</returns>
    IRunStore StoreActual<TSut>(Func<TSut, object?> calculationFunction);

    /// <summary>
    /// Stores the System under test.
    /// </summary>
    /// <param name="systemUnderTest">The system under test.</param>
    /// <returns>An instance of the store to be used as fluent api.</returns>
    IRunStore StoreSut(object? systemUnderTest);

    /// <summary>
    /// Counts the number of items that match the condition.
    /// </summary>
    /// <param name="condition">The condition to match.</param>
    /// <returns>The count of elements as integer.</returns>
    public int Count(Func<KeyValuePair<string, object?>, bool> condition);

    /// <summary>
    /// Waits until a state is met.
    /// </summary>
    /// <param name="condition">Checks whether the desired state is reached.</param>
    /// <param name="timeoutInMs">Timeout in MiliSeconds.</param>
    void WaitUntil(Func<IRunStore, bool> condition, int timeoutInMs = 5000);
}