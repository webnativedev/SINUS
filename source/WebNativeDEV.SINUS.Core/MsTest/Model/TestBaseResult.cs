// <copyright file="TestBaseResult.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Model;

using System;
using System.Collections.Generic;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;

/// <summary>
/// Result of a test run.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TestBaseResult"/> class.
/// </remarks>
/// <param name="outcome">Execution result state.</param>
/// <param name="scope">The instance to the dependency store.</param>
/// <param name="exceptions">The exceptions if outcome fails.</param>
internal class TestBaseResult(TestOutcome outcome, TestBaseScopeContainer scope, IList<Exception>? exceptions = null) : ITestBaseResult
{
    /// <inheritdoc/>
    public TestOutcome Outcome { get; } = outcome;

    /// <inheritdoc/>
    public IList<Exception>? Exceptions { get; } = exceptions;

    /// <summary>
    /// Gets the test base instance.
    /// </summary>
    internal TestBaseScopeContainer Scope { get; } = scope;
}
