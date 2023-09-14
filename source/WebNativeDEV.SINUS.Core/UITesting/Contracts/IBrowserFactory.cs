// <copyright file="IBrowserFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting.Contracts;

using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// This interface represents a factory for creating Browsers of a given type (e.g.: Chrome).
/// </summary>
public interface IBrowserFactory
{
    /// <summary>
    /// Factory method that creates a browser object that can be used to interact with web pages.
    /// </summary>
    /// <param name="url">Initial URL to navigate to.</param>
    /// <param name="testBase">A reference to the executing test.</param>
    /// <param name="humanReadablePageName">Logical name of the page (e.g.: Google and not https://...).</param>
    /// <param name="options">Configures the browser factory.</param>
    /// <returns>An instance of a browser.</returns>
    IBrowser CreateBrowser(Uri url, TestBase testBase, string? humanReadablePageName = null, BrowserFactoryOptions? options = null);
}
