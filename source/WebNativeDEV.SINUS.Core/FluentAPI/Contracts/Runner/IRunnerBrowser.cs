// <copyright file="IRunnerBrowser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;

using System;
using WebNativeDEV.SINUS.Core.UITesting.Model;

/// <summary>
/// Represents an interface that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API including the browser.
/// </summary>
public interface IRunnerBrowser : IRunner
{
    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="url">Url that should be loaded by the browser initially.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt(string? humanReadablePageName, Uri url, BrowserFactoryOptions? options = null);

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="url">Url that should be loaded by the browser initially.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt(Uri url, BrowserFactoryOptions? options = null);

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="url">Url that should be loaded by the browser initially.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt(string? humanReadablePageName, string url, BrowserFactoryOptions? options = null);

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="url">Url that should be loaded by the browser initially.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt(string url, BrowserFactoryOptions? options = null);

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <param name="website">Tuple containing a human readable name and an URL.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenABrowserAt((string? humanReadablePageName, string url) website, BrowserFactoryOptions? options = null);
}
