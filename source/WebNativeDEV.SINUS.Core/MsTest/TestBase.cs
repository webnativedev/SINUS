// <copyright file="TestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.Model;

/// <summary>
/// Represents an abstract test base that allows later unit tests to
/// access different convenience methods on top.
/// </summary>
[TestClass]
public abstract class TestBase
{
    private string testName = string.Empty;

    /// <summary>
    /// Gets or sets the TestContext injected by the framework.
    /// </summary>
    public TestContext TestContext { get; set; } = null!;

    /// <summary>
    /// Gets the name of the current test.
    /// </summary>
    public string TestName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(this.testName))
            {
                this.testName = this.TestContext.TestName!;
            }

            return this.testName;
        }
    }

    /// <summary>
    /// Creates a runner and uses the action to execute the test (including TestName overwrite).
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected ITestBaseResult Test(string? scenario, Action<IBrowserRunner> action)
    {
        Ensure.NotNull(scenario);
        this.testName = TestNamingConventionManager.DynamicDataDisplayNameAddScenario(
            this.TestContext.TestName,
            scenario);

        return this.Test(action);
    }

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected ITestBaseResult Test(Action<IBrowserRunner> action)
    {
        var scope = new TestBaseScopeContainer(this);
        Ensure.NotNull(action).Invoke(scope.Runner);
        (scope.Runner as ICloseable)?.Close();

        InterpretResult(scope);
        scope.Runner.Dispose();

        return new TestBaseResult(TestOutcome.Success, scope);
    }

    private static void InterpretResult(TestBaseScopeContainer scope)
    {
        var logger = scope.CreateLogger<TestBase>();

        if (scope.IsPreparedOnly)
        {
            logger.LogWarning("The test result is evaluated as inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
            if (scope.ExpectedOutcome != TestOutcome.Inconclusive)
            {
                Assert.Inconclusive("The test result is evaluated as inconclusive, because it was rated 'only-prepared' when seeing no 'When'-part.");
            }

            return;
        }

        if (scope.Exceptions.HasUncheckedElements)
        {
            logger.LogError(
                "The test result is evaluated as failed, because exceptions occured. Count: {Count}; Types: {Types}",
                scope.Exceptions.Count,
                scope.Exceptions.GetContentAsString());

            if (scope.ExpectedOutcome != TestOutcome.Failure)
            {
                Assert.Fail($"The test result is evaluated as failed, because exceptions occured. Count: {scope.Exceptions.Count}; Types: {scope.Exceptions.GetContentAsString()}");
            }

            return;
        }

        logger.LogInformation("The test result is evaluated as successful. (Checked Exceptions: {CheckedExceptionCount})", scope.Exceptions.Count);
    }
}
