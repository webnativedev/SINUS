// <copyright file="Runner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.Sut;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Represents a class that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API.
/// </summary>
internal class Runner : BaseRunner, IRunner, IGiven, IGivenWithSut, IWhen, IThen
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    /// <param name="testBase">Reference to the test base creating the runner.</param>
    public Runner(TestBase testBase)
        : base(testBase)
    {
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Runner"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    ~Runner()
    {
        this.Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public IGiven Given(string description, Action<RunStore>? action = null)
        => (IGiven)this.Run(
            RunCategory.Given,
            description,
            () => action?.Invoke(this.DataBag),
            false);

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>(string description)
            where TProgram : class
        => (IGivenWithSut)this.Run(
                RunCategory.Given,
                $"a SUT in memory: " + description,
                () => this.CreateSut<TProgram>(),
                false);

    /// <inheritdoc/>
    public IWhen When(string description, Action<RunStore>? action = null)
    {
        this.IsPreparedOnly = this.IsPreparedOnly || action == null;

        return (IWhen)this.Run(
            RunCategory.When,
            description,
            () => action?.Invoke(this.DataBag),
            false);
    }

    /// <inheritdoc/>
    public IWhen When(string description, Action<HttpClient, RunStore>? action)
    {
        this.IsPreparedOnly = this.IsPreparedOnly || action == null;

        return (IWhen)this.Run(
            RunCategory.When,
            description,
            () => action?.Invoke(
                this.HttpClient,
                this.DataBag),
            false);
    }

    /// <inheritdoc/>
    public IThen Then(string description, params Action<RunStore>[] actions)
    {
        List<Action> pureAction = new();
        actions.ToList().ForEach(action => pureAction.Add(() => action?.Invoke(this.DataBag)));

        return (IThen)this.Run(
                RunCategory.Then,
                description,
                pureAction,
                true);
    }

    /// <inheritdoc/>
    public IDisposable Debug(Action<RunStore>? action = null)
        => this.Run(
            RunCategory.Debug,
            string.Empty,
            () => action?.Invoke(this.DataBag),
            true);

    /// <inheritdoc/>
    public IDisposable DebugPrint()
        => this.Run(
                RunCategory.Debug,
                string.Empty,
                () => this.DataBag.Print(),
                true);
}
