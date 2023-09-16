// <copyright file="ITestNamingConventionManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Contracts;

using WebNativeDEV.SINUS.Core.FluentAPI.Model;

/// <summary>
/// Defines an interface for an implementation that is responsible
/// for checking all naming conventions of a test.
/// </summary>
public interface ITestNamingConventionManager
{
    /// <summary>
    /// Gets the name of the test.
    /// </summary>
    string TestName { get; }

    /// <summary>
    /// Gets a value indicating whether this name is only a maintenance action,
    /// so it does not need to be checked any further.
    /// </summary>
    bool IsMaintenance { get; }

    /// <summary>
    /// Gets the description of the Given part.
    /// </summary>
    string GivenDescription { get; }

    /// <summary>
    /// Gets the description of the When part.
    /// </summary>
    string WhenDescription { get; }

    /// <summary>
    /// Gets the description of the Then part.
    /// </summary>
    string ThenDescription { get; }

    /// <summary>
    /// This function returns the corresponding description based on the category.
    /// </summary>
    /// <param name="category">Defines which description to return.</param>
    /// <returns>A plain text string description.</returns>
    string GetName(RunCategory category);

    /// <summary>
    /// This function returns the corresponding description based on the category
    /// in a human readable way.
    /// </summary>
    /// <param name="category">Defines which description to return.</param>
    /// <param name="description">The explicitly defined description.</param>
    /// <param name="index">The id of the current run.</param>
    /// <param name="count">The amount of runs.</param>
    /// <returns>A plain text human readable string description.</returns>
    string GetReadableDescription(RunCategory category, string? description, int index, int count);

    /// <summary>
    /// Gets the description from test name in a human readable way.
    /// </summary>
    /// <param name="category">Defines which description to return.</param>
    /// <returns>A plain text human readable string description.</returns>
    string GetReadableName(RunCategory category);
}