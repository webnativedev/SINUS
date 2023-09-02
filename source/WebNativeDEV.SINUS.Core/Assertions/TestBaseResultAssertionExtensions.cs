﻿// <copyright file="TestBaseResultAssertionExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Assertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.MsTest;

/// <summary>
/// This class adds extension methods for Fluent Assertions Library.
/// </summary>
public static class TestBaseResultAssertionExtensions
{
    /// <summary>
    /// Starts the context for the FluentAssertions - FluentAPI.
    /// </summary>
    /// <param name="instance">The instance to operate on.</param>
    /// <returns>An assertion object implementing the checks to given instance.</returns>
    public static TestBaseResultAssertions Should(this TestBaseResult instance)
    {
        return new TestBaseResultAssertions(instance);
    }
}