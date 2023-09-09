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
        this.Actions = actions ?? (
                action != null
                    ? new List<Action?>() { action }
                    : new List<Action?>());
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

    internal ExecutionParameterBuilder AddDescription(string? description)
    {
        this.Description = description;
        return this;
    }

    internal ExecutionParameterBuilder AddRunActions(bool runActions)
    {
        this.RunActions = runActions;
        return this;
    }

    internal ExecutionParameterBuilder AddSetupActions(Action<ExecutionSetupParameters>? setupAction)
    {
        if (setupAction != null)
        {
            this.SetupActions = new List<Action<ExecutionSetupParameters>?>() { setupAction };
        }
        else
        {
            this.SetupActions = new List<Action<ExecutionSetupParameters>?>();
        }

        return this;
    }

    internal ExecutionParameterBuilder AddSutEndpoint(string? sutEndpoint)
    {
        this.SutEndpoint = sutEndpoint;
        return this;
    }

    internal ExecutionParameterBuilder AddSutType(Type? sutType)
    {
        this.SutType = sutType;
        return this;
    }

    internal ExecutionParameter Build()
    {
        return new ExecutionParameter()
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
            SetupActions = this.SetupActions ?? new List<Action<ExecutionSetupParameters>?>(),
            Actions = this.Actions ?? new List<Action?>(),

            // System under Test parameter
            CreateSut = this.CreateSut,
            SutType = this.SutType,
            SutEndpoint = this.SutEndpoint,
        };
    }
}
