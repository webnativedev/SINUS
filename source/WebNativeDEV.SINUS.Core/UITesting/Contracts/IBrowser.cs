// <copyright file="IBrowser.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// The IBrowser interface represents an instance of a view
/// that loads a given URL and can interact with the page.
/// </summary>
public interface IBrowser : IDisposable
{
    /// <summary>
    /// Gets the title of the web page as displayed in the tab in plain text.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Gets the absolute url that is currently navigated to.
    /// </summary>
    Uri? Url { get; }

    /// <summary>
    /// Gets the source code of the webpage typically in HTML.
    /// </summary>
    string? PageSource { get; }

    /// <summary>
    /// Gets the human readable page name.
    /// </summary>
    string? HumanReadablePageName { get; }

    /// <summary>
    /// Gets the native selenium object, but as object to reduce the dependency to selenium.
    /// Should not be needed and is only a fallback if something essentially is missing in the
    /// framework, that requires native access.
    /// </summary>
    /// <remarks>
    /// If this is required often, please open an issue on github.
    /// </remarks>
    /// <returns>Returns an object that is typically an IWebDriver.</returns>
    object? GetBaseObject();

    /// <summary>
    /// Finds all UI elements that are required to check their data or to interact with.
    /// Should not be needed and is only a fallback if IDs are reqally not possible or are not handy.
    /// </summary>
    /// <remarks>
    /// If this is required often, please open an issue on github.
    /// </remarks>
    /// <param name="xpath">An XPath expression pointing to the elements that are requested.</param>
    /// <returns>
    /// A collection of IContent that represents the requested UI objects in the browser.
    /// </returns>
    IEnumerable<IContent> FindElements(string xpath);

    /// <summary>
    /// Finds UI elements based on HTML-driven IDs (that must be unique inside a website).
    /// </summary>
    /// <param name="id">The HTML ID.</param>
    /// <param name="timeoutInSeconds">Amount of seconds to wait until we consider the element as non-existent.</param>
    /// <returns>A content element that represents a UI element.</returns>
    IContent FindElement(string id, int timeoutInSeconds = 0);

    /// <summary>
    /// Leaves the current website and navigates to the given URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    /// <returns>The current browser object (for fluent API usage).</returns>
    IBrowser NavigateTo(string url);

    /// <summary>
    /// Leaves the current website and navigates to the given URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    /// <returns>The current browser object (for fluent API usage).</returns>
    IBrowser NavigateTo(Uri url);

    /// <summary>
    /// Finds the current element that is active.
    /// </summary>
    /// <returns>A content element that represents a UI element.</returns>
    IContent FindActiveElement();

    /// <summary>
    /// Runs javascript code on a currently loaded webpage.
    /// </summary>
    /// <param name="js">The code to execute.</param>
    /// <param name="content">UI components related to the code.</param>
    /// <returns>The current browser object (for fluent API usage).</returns>
    IBrowser ExecuteScript(string js, params IContent[] content);

    /// <summary>
    /// Creates a png image of the currently loaded browser (content only.
    /// </summary>
    /// <param name="filename">The filename to store the scrrenshot.</param>
    /// <returns>The current browser object (for fluent API usage).</returns>
    IBrowser TakeScreenshot(string? filename = null);
}
