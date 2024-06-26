﻿// <copyright file="ExecutionOutput.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Execution.Model;

using System;
using System.Collections.Generic;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.Sut.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Represents a class used as output for the execution engine implementation.
/// </summary>
internal sealed class ExecutionOutput : IExecutionOutput
{
    /// <summary>
    /// Gets the exceptions that were raised during execution.
    /// </summary>
    public IList<Exception> Exceptions { get; } = [];

    /// <summary>
    /// Gets or sets the HttpClient created for the execution.
    /// </summary>
    public HttpClient? HttpClient { get; set; }

    /// <summary>
    /// Gets or sets the Web Application Factory as IDisposable.
    /// </summary>
    public ISinusWebApplicationFactory? WebApplicationFactory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the execution is only prepared.
    /// </summary>
    public bool IsPreparedOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the execution should run if exceptions were already raised.
    /// </summary>
    public bool ShouldRunIfAlreadyExceptionOccured { get; set; }

    /// <summary>
    /// Gets or sets a system under test endpoint.
    /// </summary>
    public string? SutEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the run category.
    /// </summary>
    public RunCategory RunCategory { get; set; }

    /// <summary>
    /// Gets or sets the reference to the test.
    /// </summary>
    public TestBase? TestBase { get; set; }
}
