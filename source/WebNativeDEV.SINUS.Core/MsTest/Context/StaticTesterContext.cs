// <copyright file="StaticTesterContext.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Context;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

/// <summary>
/// Test context for static tester emulating an ms-test injected context.
/// </summary>
internal class StaticTesterContext : TestContext
{
    private readonly IDictionary properties = new Dictionary<object, object>();
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticTesterContext"/> class.
    /// </summary>
    public StaticTesterContext()
    {
        this.properties.Add(nameof(this.TestName), "Given_StaticTest_When_Executed_Then_UniqueExecutionStarts-" + Guid.NewGuid().ToString());
        this.properties.Add(nameof(this.FullyQualifiedTestClassName), "StaticTests");
        this.properties.Add(nameof(this.TestRunResultsDirectory), ".");
        this.properties.Add(nameof(this.TestRunDirectory), ".");
        this.logger = TestBaseSingletonContainer.CreateLogger<StaticTester>();
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    public override IDictionary Properties => this.properties;

    /// <inheritdoc />
    public override void AddResultFile(string fileName)
        => this.WriteLineImplementation("Result file added: " + fileName);

    /// <inheritdoc/>
    public override void Write(string? message)
        => this.WriteLineImplementation(message);

    /// <inheritdoc/>
    public override void Write(string? format, params object?[] args)
        => this.WriteLineImplementation(format, args);

    /// <inheritdoc/>
    public override void WriteLine(string? message)
        => this.WriteLineImplementation(message);

    /// <inheritdoc/>
    public override void WriteLine(string? format, params object?[] args)
        => this.WriteLineImplementation(format, args);

    private void WriteLineImplementation(string? format, params object?[] args)
    {
        #pragma warning disable CA2254 // Vorlage muss ein statischer Ausdruck sein
        #pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
        #pragma warning disable S2629 // Don't use string concatenation in logging message templates.
        this.logger.LogInformation("StaticTestContext: " + (format ?? string.Empty), args);
        #pragma warning restore S2629
        #pragma warning restore CA2254
        #pragma warning restore IDE0079
    }
}
