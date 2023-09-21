// <copyright file="ExecutionParameter.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Model;

using System;
using System.Collections.Generic;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Represents a class used as input for the execution engine implementation.
/// </summary>
internal class ExecutionParameter
{
    /// <summary>
    /// Gets or sets the reference to the test executed.
    /// </summary>
    public TestBase? TestBase { get; set; }

    /// <summary>
    /// Gets or sets the category or section in which the test is currently.
    /// </summary>
    public RunCategory RunCategory { get; set; }

    /// <summary>
    /// Gets or sets the description passed in by the test.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the actions should be executed or not.
    /// </summary>
    public bool RunActions { get; set; } = true;

    /// <summary>
    /// Gets the actions to execute.
    /// </summary>
    public IList<Action<ExecutionSetupParameters>> SetupActions { get; } = new List<Action<ExecutionSetupParameters>>();

    /// <summary>
    /// Gets the actions to execute.
    /// </summary>
    public IList<Action> Actions { get; } = new List<Action>();

    /// <summary>
    /// Gets or sets the reference to the runner.
    /// </summary>
    public IRunner? Runner { get; set; }

    /// <summary>
    /// Gets or sets the reference to the Naming Conventions manager.
    /// </summary>
    public ITestNamingConventionManager? Namings { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a System under test should be created.
    /// </summary>
    public bool CreateSut { get; set; }

    /// <summary>
    /// Gets or sets a type that represents the class which needs to be instantiated to create a system under test.
    /// </summary>
    public Type? SutType { get; set; }

    /// <summary>
    /// Gets the arguments to hand-over in the creation of a system under test.
    /// </summary>
    public IList<string> SutArgs { get; } = new List<string>();

    /// <summary>
    /// Gets or sets an endpoint address for the system under test.
    /// </summary>
    public string? SutEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the count of exceptions that already happend in earlier execution stages.
    /// </summary>
    public int ExceptionsCount { get; set; }
}
