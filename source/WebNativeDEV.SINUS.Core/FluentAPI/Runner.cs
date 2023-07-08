// <copyright file="Runner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.Sut;
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
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    public Runner(TestBase testBase)
        : base(testBase)
    {
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Runner"/> class.
    /// </summary>
    ~Runner()
    {
        this.Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public IGiven Given(string description, Action<Dictionary<string, object?>>? action = null)
        => (IGiven)this.Run(RunCategory.Given, description, () => action?.Invoke(this.DataBag));

    /// <inheritdoc/>
    public IGivenWithSut GivenASystem<TProgram>(string description)
            where TProgram : class
        => (IGivenWithSut)this.Run(
                RunCategory.Given,
                $"a SUT in memory: " + description,
                () => this.CreateSut<TProgram>());

    /// <inheritdoc/>
    public IWhen When(string description, Action<Dictionary<string, object?>>? action = null)
    {
        this.IsPreparedOnly = this.IsPreparedOnly || action == null;

        return (IWhen)this.Run(RunCategory.When, description, () => action?.Invoke(this.DataBag));
    }

    /// <inheritdoc/>
    public IWhen When(string description, Action<HttpClient, Dictionary<string, object?>>? action)
    {
        this.IsPreparedOnly = this.IsPreparedOnly || action == null;

        return (IWhen)this.Run(
            RunCategory.When,
            description,
            () => action?.Invoke(
                this.HttpClient,
                this.DataBag));
    }

    /// <inheritdoc/>
    public IThen Then(string description, Action<Dictionary<string, object?>>? action = null)
        => (IThen)this.Run(
            RunCategory.Then,
            description,
            () => action?.Invoke(this.DataBag));

    /// <inheritdoc/>
    public IDisposable Debug(Action<Dictionary<string, object?>>? action = null)
        => this.Run(RunCategory.Debug, string.Empty, () => action?.Invoke(this.DataBag));
}
