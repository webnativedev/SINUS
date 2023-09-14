// <copyright file="IWebDriverFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting.Contracts;

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.UITesting.Model;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Interface that creates a web driver.
/// </summary>
public interface IWebDriverFactory
{
    /// <summary>
    /// Creates an instance of a web driver. This is a native selenium driver and will
    /// be covered by a Browser object.
    /// </summary>
    /// <param name="options">Configuration options.</param>
    /// <param name="testBase">Reference to the test creating the web driver.</param>
    /// <returns>A native selenium webdriver to load and interact with web pages.</returns>
    IWebDriver CreateWebDriver(BrowserFactoryOptions options, TestBase testBase);
}
