// <copyright file="AssertExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Assertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebNativeDEV.SINUS.Core.FluentAPI;

/// <summary>
/// Custom Assert functions.
/// </summary>
public static class AssertExtensions
{
    /// <summary>
    /// Adds an equality-assertion method specifically for the actual value
    /// that can be called by Assert.That.
    /// </summary>
    /// <typeparam name="T">Type of the values.</typeparam>
    /// <param name="assert">Instance to the object to extend.</param>
    /// <param name="store">The run-store including the actual data.</param>
    /// <param name="expected">Value to compare against (including type check).</param>
    public static void AreEqualToActual<T>(this Assert assert, RunStore store, T expected)
    {
        if (assert is null)
        {
            throw new ArgumentNullException(nameof(assert));
        }

        if (store is null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        var actual = store.ReadActual<T>();

        Assert.AreEqual(expected, actual);
    }
}
