// <copyright file="IBrowserRunner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

using System;
using WebNativeDEV.SINUS.Core.UITesting.Model;

/// <summary>
/// Represents an interface that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API including the browser.
/// </summary>
public interface IBrowserRunner : IRunner, IGivenBrowser, IWhenBrowser, IThenBrowser
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

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="endpoint">Url that should be loaded by the browser initially.</param>
    /// <param name="url">Initial url to load.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, string url, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="endpoint">Url that should be loaded by the browser initially.</param>
    /// <param name="url">Initial url to load.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string endpoint, string url, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the SUT.</typeparam>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="endpoint">Url that should be loaded by the browser initially.</param>
    /// <param name="url">Initial url to load.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string? humanReadablePageName, string endpoint, Uri url, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the SUT.</typeparam>
    /// <param name="endpoint">Url that should be loaded by the browser initially.</param>
    /// <param name="url">Initial url to load.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAt<TProgram>(string endpoint, Uri url, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// The System under test will be started at a default endpoint.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="browserPageToStart">
    /// The location where the browser should be started realtive to the server startup default address.
    /// Example "/view".
    /// </param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>(string? humanReadablePageName, string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// The System under test will be started at a default endpoint.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="browserPageToStart">
    /// The location where the browser should be started realtive to the server startup default address.
    /// Example "/view".
    /// </param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>(string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// The System under test will be started at a default endpoint.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="page">The human readable page bame and its relative location.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAtDefaultEndpoint<TProgram>((string? humanReadablePageName, string? browserPageToStart) page, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// The System under test will be started at a random endpoint.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="humanReadablePageName">The logical information about the page.</param>
    /// <param name="browserPageToStart">
    /// The location where the browser should be started realtive to the server startup random address.
    /// Example "/view".
    /// </param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAtRandomEndpoint<TProgram>(string? humanReadablePageName, string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// The System under test will be started at a random endpoint.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="browserPageToStart">
    /// The location where the browser should be started realtive to the server startup random address.
    /// Example "/view".
    /// </param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAtRandomEndpoint<TProgram>(string? browserPageToStart = null, BrowserFactoryOptions? options = null)
        where TProgram : class;

    /// <summary>
    /// Allows to define the Browser-based Given-Action in a Given-When-Then sequence.
    /// The System under test will be started at a random endpoint.
    /// </summary>
    /// <typeparam name="TProgram">Generic pointing to the class that bootstraps the Sut.</typeparam>
    /// <param name="page">The human readable page bame and its relative location.</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>
    /// An object that will point to the runner.
    /// The interface helps to reduce the set of options to only
    /// the appropriate in the sequence.
    /// </returns>
    IGivenBrowser GivenASystemAndABrowserAtRandomEndpoint<TProgram>((string? humanReadablePageName, string? browserPageToStart) page, BrowserFactoryOptions? options = null)
        where TProgram : class;
}
