﻿// <copyright file="IRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using System;

/// <summary>
/// Represents an interface that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API.
/// </summary>
public interface IRunner : IDisposable
{
    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="action">Defines the execution part.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGiven Given(string description, Action<RunStore>? action = null);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="action">Defines the execution part.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGiven Given(Action<RunStore>? action = null);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the Sut.</typeparam>
    /// <param name="description">Plain text description.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSut GivenASystem<TProgram>(string description)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the Sut.</typeparam>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSut GivenASystem<TProgram>()
        where TProgram : class;
}
