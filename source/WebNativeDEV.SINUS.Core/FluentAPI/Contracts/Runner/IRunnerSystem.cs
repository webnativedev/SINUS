// <copyright file="IRunnerSystem.cs" company="WebNativeDEV">
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
public interface IRunnerSystem : IRunner
{
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

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the Sut.</typeparam>
    /// <param name="description">Plain text description.</param>
    /// <param name="args">The arguments to hand-over into the sut.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSut GivenASystem<TProgram>(string description, params string[] args)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">The type to bootstrap the Sut.</typeparam>
    /// <param name="args">The arguments to hand-over into the sut.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSut GivenASystem<TProgram>(params string[] args)
        where TProgram : class;
}
