// <copyright file="ICloseable.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;

/// <summary>
/// Interface that can be used to show that a close-method is implemented.
/// </summary>
internal interface ICloseable
{
    /// <summary>
    /// Closes the element.
    /// </summary>
    void Close();
}
