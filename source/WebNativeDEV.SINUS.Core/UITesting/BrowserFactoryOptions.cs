// <copyright file="BrowserFactoryOptions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.UITesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents all settings that can be configured from outside.
/// </summary>
public class BrowserFactoryOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to start as Headless or not.
    /// </summary>
    public bool Headless { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore SSL errors in connection or not.
    /// </summary>
    public bool IgnoreSslErrors { get; set; }

    /// <summary>
    /// Prints the state of the options.
    /// </summary>
    /// <returns>A plain text string with the full state.</returns>
    public override string ToString()
    {
        return $"Headless: {this.Headless}, IgnoreSslErrors: {this.IgnoreSslErrors}";
    }
}
