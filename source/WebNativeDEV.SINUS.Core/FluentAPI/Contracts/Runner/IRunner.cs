// <copyright file="IRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;

using System;
using WebNativeDEV.SINUS.Core.Events.EventArguments;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

/// <summary>
/// Represents an interface that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API.
/// </summary>
public interface IRunner
{
    /// <summary>
    /// Registers an event handler.
    /// </summary>
    /// <typeparam name="TEventBusEventArgs">Type of the event args.</typeparam>
    /// <param name="description">Plain text description.</param>
    /// <param name="handler">The event handler.</param>
    /// <param name="filter">The execution filter.</param>
    /// <returns>The current test base instance.</returns>
    IRunner Listen<TEventBusEventArgs>(string description, Action<object, IRunStore, TEventBusEventArgs> handler, Func<object, IRunStore, TEventBusEventArgs, bool>? filter = null)
        where TEventBusEventArgs : EventBusEventArgs;

    /// <summary>
    /// Registers an event handler.
    /// </summary>
    /// <typeparam name="TEventBusEventArgs">Type of the event args.</typeparam>
    /// <param name="handler">The event handler.</param>
    /// <param name="filter">The execution filter.</param>
    /// <returns>The current test base instance.</returns>
    IRunner Listen<TEventBusEventArgs>(Action<object, IRunStore, TEventBusEventArgs> handler, Func<object, IRunStore, TEventBusEventArgs, bool>? filter = null)
        where TEventBusEventArgs : EventBusEventArgs;

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="action">Defines the execution part.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGiven Given(string description, Action<IRunStore>? action = null);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="action">Defines the execution part.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGiven Given(Action<IRunStore>? action = null);
}
