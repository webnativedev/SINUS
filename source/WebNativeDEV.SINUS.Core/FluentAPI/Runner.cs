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
internal sealed class Runner : BaseRunner, IRunner, IGiven, IWhen, IThen
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    /// <param name="loggerFactory">LoggerFactory to create a logger instance for the test.</param>
    public Runner(ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        this.Logger = loggerFactory.CreateLogger<Runner>();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Runner"/> class.
    /// </summary>
    ~Runner()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Gets the logger that can be used to print information.
    /// </summary>
    private ILogger Logger { get; }

    /// <inheritdoc/>
    public IGiven Given(string description, Action<Dictionary<string, object?>>? action = null)
        => this.Run("Given", description, action);

    /// <inheritdoc/>
    public IGiven GivenASystem<TProgram>(string description)
            where TProgram : class
    {
        this.Logger.LogInformation("Given: a SUT - desc: {Page}", description);

        WebApplicationFactory<TProgram>? builder = null;
        HttpClient? client = null;
        for (int i = 0; i < 3; i++)
        {
            try
            {
                builder = new WebApplicationFactory<TProgram>();
                client = builder.CreateClient();
                break;
            }
            catch (IOException exception)
            {
                this.Logger.LogError(exception, "retry attempt {Attempt}", i + 1);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        if (client != null && builder != null)
        {
            this.Disposables.Add(new SinusWebApplicationFactoryResult<TProgram>(builder, client));
        }
        else
        {
            client?.Dispose();
            builder?.Dispose();
        }

        return this.Given(description, null);
    }

    /// <inheritdoc/>
    public IWhen When(string description, Action<Dictionary<string, object?>>? action = null)
        => this.Run("When", description, action);

    /// <inheritdoc/>
    public IWhen When(string description, Action<HttpClient, Dictionary<string, object?>>? action)
        => this.Run("When", description, action);

    /// <inheritdoc/>
    public IThen Then(string description, Action<Dictionary<string, object?>>? action = null)
        => this.Run("Then", description, action);

    /// <inheritdoc/>
    public IDisposable Debug(Action<Dictionary<string, object?>>? action = null)
        => this.Run("Debug", string.Empty, action);

    private Runner Run(string category, string description, Action<Dictionary<string, object?>>? action)
    {
        this.Logger.LogInformation("{Category}: {Description}", category, description);
        action?.Invoke(this.DataBag);
        return this;
    }

    private Runner Run(string category, string description, Action<HttpClient, Dictionary<string, object?>>? action)
    {
        this.Logger.LogInformation("{Category}: {Description}", category, description);
        var client = this.Disposables?.OfType<ISinusWebApplicationFactoryResult>()?.FirstOrDefault()?.HttpClient;
        action?.Invoke(
            client ?? throw new InvalidOperationException("no client found"),
            this.DataBag);
        return this;
    }
}
