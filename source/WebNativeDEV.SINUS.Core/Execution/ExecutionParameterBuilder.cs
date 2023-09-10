// <copyright file="ExecutionParameterBuilder.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution;

using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.MsTest;
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
    internal ExecutionParameterBuilder(TestBase testBase, Runner runner, TestNamingConventionManager namings, RunCategory runCategory, int exceptionsCount)
    {
        this.TestBase = testBase;
        this.Runner = runner;
        this.Namings = namings;
        this.RunCategory = runCategory;
        this.ExceptionsCount = exceptionsCount;
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
            this.Actions.AddRange(actions);
        }
        else if (action != null)
        {
            this.Actions.Add(action);
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

            // Actual action
            RunActions = this.RunActions && ((this.Actions?.Any() ?? false) || (this.SetupActions?.Any() ?? false)),

            // System under Test parameter
            CreateSut = this.CreateSut,
            SutType = this.SutType,
            SutEndpoint = this.SutEndpoint,
        };

        parameters.Actions.AddRange(this.Actions ?? new List<Action?>());
        parameters.SetupActions.AddRange(this.SetupActions ?? new List<Action<ExecutionSetupParameters>?>());

        return parameters;
    }
}
