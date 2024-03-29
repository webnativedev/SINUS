﻿// <copyright file="RunStoreAssertionExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;

/// <summary>
/// This class adds extension methods for Fluent Assertions Library.
/// </summary>
public static class RunStoreAssertionExtensions
{
    /// <summary>
    /// Starts the context for the FluentAssertions - FluentAPI.
    /// </summary>
    /// <param name="instance">The instance to operate on.</param>
    /// <returns>An assertion object implementing the checks to given instance.</returns>
    public static RunStoreAssertions Should(this IRunStore instance)
    {
        return new RunStoreAssertions(instance);
    }
}
