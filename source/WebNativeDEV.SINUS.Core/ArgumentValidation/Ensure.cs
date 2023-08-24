// <copyright file="Ensure.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.ArgumentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class that is used to ensure rules similar to data contracts.
/// </summary>
public static class Ensure
{
    /// <summary>
    /// Ensures that the item is not null by throwing exception if so.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    /// <param name="item">The object to check.</param>
    /// <param name="name">The parameter name of the item to check.</param>
    /// <returns>The item.</returns>
    /// <exception cref="ArgumentValidationException">Exception thrown if item is null.</exception>
    public static T NotNull<T>(T? item, string? name = null)
    {
        return item
            ?? throw new ArgumentValidationException(nameof(NotNull), item, name);
    }
}
