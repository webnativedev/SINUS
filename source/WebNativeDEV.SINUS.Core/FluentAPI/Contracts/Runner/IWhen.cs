// <copyright file="IWhen.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;

using System;

/// <summary>
/// Represents in the given-when-then sequence the when part.
/// This interface allows to create a proper Fluent API.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716", Justification = "Required by pattern")]
public interface IWhen
{
    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="actions">Defines the execution parts.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen Then(string description, params Action<IRunStore>[] actions);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="actions">Defines the execution parts.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen Then(params Action<IRunStore>[] actions);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen ThenNoError(string description);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen ThenNoError();

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence after
    /// we expect that when fails.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen ThenShouldHaveFailed(string description);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence after
    /// we expect that when fails.
    /// </summary>
    /// <param name="countExceptions">Count of exceptions expected.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen ThenShouldHaveFailed(int countExceptions = 1);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence after
    /// we expect that when fails.
    /// </summary>
    /// <typeparam name="T">Exception type expected.</typeparam>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThen ThenShouldHaveFailedWith<T>()
        where T : Exception;
}
