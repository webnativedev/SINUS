// <copyright file="IWhenBrowser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;

using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents in the given-when-then sequence the when part
/// especially with focus on browser-usage.
/// This interface allows to create a proper Fluent API.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716", Justification = "Required by pattern")]
public interface IWhenBrowser : IWhen
{
    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="actions">Defines the parts to check.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThenBrowser Then(string description, params Action<IBrowser, IRunStore>[] actions);

    /// <summary>
    /// Allows to define the Then-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="actions">Defines the parts to check.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IThenBrowser Then(params Action<IBrowser, IRunStore>[] actions);
}
