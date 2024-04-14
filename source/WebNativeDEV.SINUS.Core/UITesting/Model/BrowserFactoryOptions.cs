// <copyright file="BrowserFactoryOptions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting.Model;

/// <summary>
/// Represents all settings that can be configured from outside.
/// </summary>
public class BrowserFactoryOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserFactoryOptions"/> class.
    /// </summary>
    /// <param name="headless">The headless option.</param>
    /// <param name="ignoreSslErrors">The ignore ssl option.</param>
    /// <param name="webDriver">The selected web driver to use.</param>
    public BrowserFactoryOptions(bool headless = true, bool ignoreSslErrors = true, SupportedWebDriver webDriver = SupportedWebDriver.Chrome)
    {
        this.Headless = headless;
        this.IgnoreSslErrors = ignoreSslErrors;
        this.WebDriver = webDriver;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to start as Headless or not.
    /// </summary>
    public bool Headless { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore SSL errors in connection or not.
    /// </summary>
    public bool IgnoreSslErrors { get; set; }

    /// <summary>
    /// Gets or sets the selected web driver.
    /// </summary>
    public SupportedWebDriver WebDriver { get; set; }

    /// <summary>
    /// Prints the state of the options.
    /// </summary>
    /// <returns>A plain text string with the full state.</returns>
    public override string ToString()
    {
        return $"Headless: {this.Headless}, IgnoreSslErrors: {this.IgnoreSslErrors}";
    }
}
