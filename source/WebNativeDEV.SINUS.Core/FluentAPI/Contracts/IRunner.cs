// <copyright file="IRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using System;
using System.Collections.Generic;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents an interface that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API.
/// </summary>
public interface IRunner
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
    IGiven Given(string description, Action<Dictionary<string, object?>>? action = null);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the SUT.</typeparam>
    /// <param name="description">Plain text description.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSUT GivenASystem<TProgram>(string description)
        where TProgram : class;
}
