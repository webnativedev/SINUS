// <copyright file="IBrowserRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using System;

/// <summary>
/// Represents an interface that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API including the browser.
/// </summary>
public interface IBrowserRunner
{
    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="url">Url that should be loaded by the browser initially.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt(string? humanReadablePageName, Uri url);

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="url">Url that should be loaded by the browser initially.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt(string? humanReadablePageName, string url);

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the SUT.</typeparam>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="endpoint">Url that should be loaded by the browser initially.</param>
    /// <param name="url">Initial url to load.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, string url)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the SUT.</typeparam>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="endpoint">Url that should be loaded by the browser initially.</param>
    /// <param name="url">Initial url to load.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, Uri url)
        where TProgram : class;
}
