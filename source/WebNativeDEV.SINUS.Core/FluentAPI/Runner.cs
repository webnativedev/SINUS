// <copyright file="Runner.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.SUT;

/// <summary>
/// Represents a class that manages the execution of a test based on a given-when-then sequence.
/// This interface allows to create a proper Fluent API.
/// </summary>
internal class Runner : BaseRunner, IRunner, IGiven, IGivenWithSUT, IWhen, IThen
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    public Runner(ILoggerFactory loggerFactory)
        : base(loggerFactory)
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
        => (IGiven)this.Run("Given", description, () => action?.Invoke(this.DataBag));

    /// <inheritdoc/>
    public IGivenWithSUT GivenASystem<TProgram>(string description)
            where TProgram : class
        => (IGivenWithSUT)this.Run(
                "Given",
                $"Given: a SUT in memory",
                () =>
                {
                    this.CreateSUT<TProgram>();
                    this.Given(description);
                });

    /// <inheritdoc/>
    public IWhen When(string description, Action<Dictionary<string, object?>>? action = null)
    {
        if (action == null)
        {
            this.IsPreparedOnly = true;
        }

        return (IWhen)this.Run("When", description, () => action?.Invoke(this.DataBag));
    }

    /// <inheritdoc/>
    public IWhen When(string description, Action<HttpClient, Dictionary<string, object?>>? action)
    {
        if (action == null)
        {
            this.IsPreparedOnly = true;
        }

        return (IWhen)this.Run(
            "When",
            description,
            () => action?.Invoke(
                this.HttpClient,
                this.DataBag));
    }

    /// <inheritdoc/>
    public IThen Then(string description, Action<Dictionary<string, object?>>? action = null)
        => (IThen)this.Run(
            "Then",
            description,
            () => action?.Invoke(this.DataBag));

    /// <inheritdoc/>
    public IDisposable Debug(Action<Dictionary<string, object?>>? action = null)
        => this.Run("Debug", string.Empty, () => action?.Invoke(this.DataBag));
}
