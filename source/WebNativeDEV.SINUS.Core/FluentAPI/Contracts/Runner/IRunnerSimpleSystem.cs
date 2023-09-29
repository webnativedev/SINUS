// <copyright file="IRunnerSimpleSystem.cs" company="WebNativeDEV">
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
public interface IRunnerSimpleSystem : IRunner
{
    /// <summary>
    /// Allows to define the Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="sutFactory">The factory that creates a simple Sut.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSimpleSut GivenASystem(string description, Func<object> sutFactory);

    /// <summary>
    /// Allows to define the Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="sutFactory">The factory that creates a simple Sut.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSimpleSut GivenASystem(Func<object> sutFactory);

    /// <summary>
    /// Allows to define the Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="sutFactory">The factory that creates a simple Sut.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSimpleSut GivenASystem(Func<IRunStore, object> sutFactory);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="sut">The instance pointing to the System under test.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSimpleSut GivenASystem(string description, object sut);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="sut">The instance pointing to the System under test.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IGivenWithSimpleSut GivenASystem(object sut);
}
