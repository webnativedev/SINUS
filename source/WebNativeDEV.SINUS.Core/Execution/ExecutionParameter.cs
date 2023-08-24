// <copyright file="ExecutionParameter.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Represents a class used as input for the execution engine implementation.
/// </summary>
public sealed class ExecutionParameter
{
    /// <summary>
    /// Gets the reference to the test executed.
    /// </summary>
    public TestBase? TestBase { get; init; }

    /// <summary>
    /// Gets the category or section in which the test is currently.
    /// </summary>
    public RunCategory RunCategory { get; init; }

    /// <summary>
    /// Gets the description passed in by the test.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets a value indicating whether the actions should be executed or not.
    /// </summary>
    public bool RunActions { get; init; }

    /// <summary>
    /// Gets the actions to execute.
    /// </summary>
    public IList<Action?>? Actions { get; init; }

    /// <summary>
    /// Gets the reference to the runner.
    /// </summary>
    public IRunner? Runner { get; init; }

    /// <summary>
    /// Gets a value indicating whether a System under test should be created.
    /// </summary>
    public bool CreateSut { get; init; }

    /// <summary>
    /// Gets a type that represents the class which needs to be instantiated to create a system under test.
    /// </summary>
    public Type? SutType { get; init; }

    /// <summary>
    /// Gets an endpoint address for the system under test.
    /// </summary>
    public string? SutEndpoint { get; init; }

    /// <summary>
    /// Gets the count of exceptions that already happend in earlier execution stages.
    /// </summary>
    public int ExceptionsCount { get; init; }
}
