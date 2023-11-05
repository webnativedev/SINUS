// <copyright file="ExecutionParameterBuilder.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Execution Parameter class that implements the builder pattern.
/// </summary>
internal sealed class ExecutionParameterBuilder : ExecutionParameter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionParameterBuilder"/> class.
    /// </summary>
    /// <param name="testBase">The testbase.</param>
    /// <param name="runner">The runner.</param>
    /// <param name="namings">The naming convention checker.</param>
    /// <param name="runCategory">The run category.</param>
    /// <param name="exceptionsCount">The count of exceptions.</param>
    internal ExecutionParameterBuilder(TestBase testBase, IRunnerBrowser? runner, ITestNamingConventionManager namings, RunCategory runCategory, int exceptionsCount)
    {
        this.TestBase = testBase;
        this.Runner = runner;
        this.Namings = namings;
        this.RunCategory = runCategory;
        this.ExceptionsCount = exceptionsCount;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionParameterBuilder"/> class.
    /// </summary>
    /// <param name="scope">Reference to all dependencies.</param>
    /// <param name="runCategory">The run category.</param>
    internal ExecutionParameterBuilder(TestBaseScopeContainer scope, RunCategory runCategory)
        : this(scope.TestBase, scope.Runner, scope.NamingConventionManager, runCategory, scope.Exceptions.Count)
    {
    }

    /// <summary>
    /// Adds actions.
    /// </summary>
    /// <param name="action">Single action to add if actions is null.</param>
    /// <param name="actions">The actions to add. Action will be ignored if not null.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddActions(Action? action, IList<Action?>? actions)
    {
        if (actions != null)
        {
            foreach (var a in actions)
            {
                if (a != null)
                {
                    this.Actions.Add(a);
                }
            }

            return this;
        }

        if (action != null)
        {
            this.Actions.Add(action);
            return this;
        }

        return this;
    }

    /// <summary>
    /// Adds the flag to create a system under test.
    /// </summary>
    /// <param name="createSut">Indicates whether a system under test should be created.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddCreateSut(bool createSut)
    {
        this.CreateSut = createSut;
        return this;
    }

    /// <summary>
    /// Adds description of the step to execute.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddDescription(string? description)
    {
        this.Description = description;
        return this;
    }

    /// <summary>
    /// Adds the information whether actions should be run.
    /// </summary>
    /// <param name="runActions">The information whether actions should be run.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddRunActions(bool runActions)
    {
        this.RunActions = runActions;
        return this;
    }

    /// <summary>
    /// Adds the setup action.
    /// </summary>
    /// <param name="setupAction">The setup action.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddSetupActions(Action<ExecutionSetupParameters>? setupAction)
    {
        if (setupAction != null)
        {
            this.SetupActions.Add(setupAction);
        }

        return this;
    }

    /// <summary>
    /// Add the system under test endpoint.
    /// </summary>
    /// <param name="sutEndpoint">The system under test endpoint.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddSutEndpoint(string? sutEndpoint)
    {
        this.SutEndpoint = sutEndpoint;
        return this;
    }

    /// <summary>
    /// Add system under test type.
    /// </summary>
    /// <param name="sutType">The system under test type.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddSutType(Type? sutType)
    {
        this.SutType = sutType;
        return this;
    }

    /// <summary>
    /// Add system under test arguments.
    /// </summary>
    /// <param name="sutArgs">The system under test args.</param>
    /// <returns>A reference to the builder instance.</returns>
    internal ExecutionParameterBuilder AddSutArgs(IEnumerable<string>? sutArgs)
    {
        if (sutArgs != null)
        {
            foreach (var arg in sutArgs)
            {
                this.SutArgs.Add(arg);
            }
        }

        return this;
    }

    /// <summary>
    /// Builds the stored information to the final execution parameter object.
    /// </summary>
    /// <returns>The resulting execution parameter.</returns>
    internal ExecutionParameter Build()
    {
        var parameters = new ExecutionParameter()
        {
            // Dependencies
            TestBase = this.TestBase,
            Runner = this.Runner,
            Namings = this.Namings,

            // Meta information
            RunCategory = this.RunCategory,
            ExceptionsCount = this.ExceptionsCount,

            Description = this.Description,

            // System under Test parameter
            CreateSut = this.CreateSut,
            SutType = this.SutType,
            SutEndpoint = this.SutEndpoint,
        };

        // Actual action
        foreach (var a in this.Actions ?? new List<Action>())
        {
            parameters.Actions.Add(a);
        }

        foreach (var sa in this.SetupActions ?? new List<Action<ExecutionSetupParameters>>())
        {
            parameters.SetupActions.Add(sa);
        }

        parameters.RunActions = this.RunActions && (parameters.Actions.Any() || parameters.SetupActions.Any());

        foreach (var arg in this.SutArgs ?? new List<string>())
        {
            parameters.SutArgs.Add(arg);
        }

        return parameters;
    }
}
