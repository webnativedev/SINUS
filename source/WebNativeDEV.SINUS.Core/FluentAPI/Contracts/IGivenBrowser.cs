// <copyright file="IGivenBrowser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Represents in the given-when-then sequence the given part
/// especially with focus on browser usage.
/// This interface allows to create a proper Fluent API.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716", Justification = "Required by pattern")]
public interface IGivenBrowser
{
    /// <summary>
    /// Allows to define the When-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="description">Plain text description.</param>
    /// <param name="action">Defines the execution part.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only the appropriate in the sequence.
    /// </returns>
    IWhenBrowser When(string description, Action<IBrowser, RunStore>? action = null);
}
