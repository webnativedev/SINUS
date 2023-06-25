// <copyright file="IContent.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Interface that represents a UI element inside a Browser.
/// </summary>
public interface IContent
{
    /// <summary>
    /// Gets the html tag name.
    /// </summary>
    string TagName { get; }

    /// <summary>
    /// Gets the content of an html ui element.
    /// </summary>
    string Text { get; }

    /// <summary>
    /// Gets a value indicating whether this UI element is enabled or not.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Gets a value indicating whether this UI element is selected or not.
    /// </summary>
    bool Selected { get; }

    /// <summary>
    /// Gets a value indicating whether this UI element is displayed or not.
    /// </summary>
    bool Displayed { get; }

    /// <summary>
    /// Returns the base object from selenium (IWebElement).
    /// </summary>
    /// <returns>Instance as object to reduce the coupling to the framework below.</returns>
    object GetBaseObject();

    /// <summary>
    /// Clears the content of an UI element.
    /// </summary>
    void Clear();

    /// <summary>
    /// Sends keys to the control as if they would be typed on the keyboard.
    /// </summary>
    /// <param name="text">The content to send.</param>
    void SendKeys(string text);

    /// <summary>
    /// Clicks the control like clicked with a mouse button.
    /// </summary>
    void Click();

    /// <summary>
    /// Returns the html attribute of the ui element.
    /// </summary>
    /// <param name="attributeName">The html attribute name.</param>
    /// <returns>The value of the html attribute.</returns>
    string GetAttribute(string attributeName);

    /// <summary>
    /// Returns the dom attribute of the ui element.
    /// </summary>
    /// <param name="attributeName">The dom attribute name.</param>
    /// <returns>The value of the dom attribute.</returns>
    string GetDomAttribute(string attributeName);

    /// <summary>
    /// Returns the dom property of the ui element.
    /// </summary>
    /// <param name="propertyName">The dom property name.</param>
    /// <returns>The value of the dom property.</returns>
    string GetDomProperty(string propertyName);

    /// <summary>
    /// Returns the css value of the ui element.
    /// </summary>
    /// <param name="propertyName">The css property name.</param>
    /// <returns>The value of the css property.</returns>
    string GetCssValue(string propertyName);
}
