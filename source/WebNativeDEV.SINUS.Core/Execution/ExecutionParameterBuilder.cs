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
    internal ExecutionParameterBuilder(TestBase testBase, Runner runner, TestNamingConventionManager namings, RunCategory runCategory, int exceptionsCount)
    {
        this.TestBase = testBase;
        this.Runner = runner;
        this.Namings = namings;
        this.RunCategory = runCategory;
        this.ExceptionsCount = exceptionsCount;
    }

    internal ExecutionParameterBuilder AddActions(Action? action, IList<Action?>? actions)
    {
        this.Actions = actions ?? (
                action != null
                    ? new List<Action?>() { action }
                    : new List<Action?>());
        return this;
    }

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
        this.SetupActions = new List<Action<ExecutionSetupParameters>?>() { setupAction };
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
