// <copyright file="Content.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using OpenQA.Selenium;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

/// <summary>
/// Class that represents a UI element inside a Browser.
/// </summary>
internal sealed class Content : IContent
{
    private readonly IWebElement webElement;

    /// <summary>
    /// Initializes a new instance of the <see cref="Content"/> class.
    /// </summary>
    /// <param name="webElement">The basic selenium element to wrap.</param>
    public Content(IWebElement webElement)
    {
        this.webElement = webElement;
    }

    /// <inheritdoc/>
    public string TagName
        => this.webElement.TagName;

    /// <inheritdoc/>
    public string Text
        => this.webElement.Text;

    /// <inheritdoc/>
    public bool Enabled
        => this.webElement.Enabled;

    /// <inheritdoc/>
    public bool Selected
        => this.webElement.Selected;

    /// <inheritdoc/>
    public bool Displayed
        => this.webElement.Displayed;

    /// <inheritdoc/>
    public object GetBaseObject()
        => this.webElement;

    /// <inheritdoc/>
    public void Clear()
        => this.webElement.Clear();

    /// <inheritdoc/>
    public void Click()
        => this.webElement.Click();

    /// <inheritdoc/>
    public string GetAttribute(string attributeName)
        => this.webElement.GetAttribute(attributeName);

    /// <inheritdoc/>
    public string GetCssValue(string propertyName)
        => this.webElement.GetCssValue(propertyName);

    /// <inheritdoc/>
    public string GetDomAttribute(string attributeName)
        => this.webElement.GetDomAttribute(attributeName);

    /// <inheritdoc/>
    public string GetDomProperty(string propertyName)
        => this.webElement.GetDomProperty(propertyName);

    /// <inheritdoc/>
    public void SendKeys(string text)
        => this.webElement.SendKeys(text);
}
