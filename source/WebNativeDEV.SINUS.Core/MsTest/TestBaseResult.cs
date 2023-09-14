// <copyright file="TestBaseResult.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Result of a test run.
/// </summary>
internal class TestBaseResult : ITestBaseResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestBaseResult"/> class.
    /// </summary>
    /// <param name="success">Execution result state.</param>
    /// <param name="scope">The instance to the dependency store.</param>
    public TestBaseResult(bool success, TestBaseScopeContainer scope)
    {
        this.Success = success;
        this.Scope = scope;
    }

    /// <summary>
    /// Gets a value indicating whether the execution state was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the test base instance.
    /// </summary>
    internal TestBaseScopeContainer Scope { get; }
}
