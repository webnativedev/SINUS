// <copyright file="StaticTesterContext.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Context;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.DevTools.V122.Target;
using System.Collections;
using System.Globalization;

/// <summary>
/// Test context for static tester emulating an ms-test injected context.
/// </summary>
public class StaticTesterContext : TestContext
{
    private readonly IDictionary properties = new Dictionary<object, object>();
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticTesterContext"/> class.
    /// </summary>
    /// <param name="testName">Name of the test.</param>
    /// <param name="className">Name of the related class (full qualified).</param>
    /// <param name="testRunResultsDir">Folder name of the test result.</param>
    /// <param name="testRunDir">Folder name of the test to run in.</param>
    private StaticTesterContext(string? testName, string className, string testRunResultsDir, string testRunDir)
    {
        this.properties.Add(nameof(this.TestName), testName);
        this.properties.Add(nameof(this.FullyQualifiedTestClassName), className);
        this.properties.Add(nameof(this.TestRunResultsDirectory), testRunResultsDir);
        this.properties.Add(nameof(this.TestRunDirectory), testRunDir);
        this.logger = TestBaseSingletonContainer.CreateLogger<StaticTester>();
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    public override IDictionary Properties => this.properties;

    /// <summary>
    /// Factory method to create a context.
    /// </summary>
    /// <param name="testName">Name of the test.</param>
    /// <param name="className">Name of the related class (full qualified).</param>
    /// <param name="addUniqueSuffix">If used multiple times set this to true to distinguish executions.</param>
    /// <param name="testRunResultsDir">Folder name of the test result.</param>
    /// <param name="testRunDir">Folder name of the test to run in.</param>
    /// <returns>An instance of a static tester context.</returns>
    public static StaticTesterContext CreateStaticTest(string? testName = "Given_StaticTest_When_Executed_Then_UniqueExecutionStarts-", string className = "(StaticTests)", bool addUniqueSuffix = true, string testRunResultsDir = ".", string testRunDir = ".")
    {
        return new StaticTesterContext(
            testName + (addUniqueSuffix
                ? Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)
                : string.Empty),
            className,
            testRunResultsDir,
            testRunDir);
    }

    /// <summary>
    /// Factory method to create a context specifically for maintenance.
    /// </summary>
    /// <param name="testName">Name of the test.</param>
    /// <param name="className">Name of the related class (full qualified).</param>
    /// <param name="addUniqueSuffix">If used multiple times set this to true to distinguish executions.</param>
    /// <param name="testRunResultsDir">Folder name of the test result.</param>
    /// <param name="testRunDir">Folder name of the test to run in.</param>
    /// <returns>An instance of a static tester context.</returns>
    public static StaticTesterContext CreateMaintenance(string? testName = "Maintenance_StaticTest_When_Executed_Then_UniqueExecutionStarts-", string className = "(StaticTests)", bool addUniqueSuffix = true, string testRunResultsDir = ".", string testRunDir = ".")
    {
        return new StaticTesterContext(
            testName + (addUniqueSuffix
                ? Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)
                : string.Empty),
            className,
            testRunResultsDir,
            testRunDir);
    }

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
