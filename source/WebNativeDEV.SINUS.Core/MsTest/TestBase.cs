// <copyright file="TestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
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
    /// <summary>
    /// Gets or sets the TestContext injected by the framework.
    /// </summary>
    public TestContext TestContext { get; set; } = null!;

    /// <summary>
    /// Creates a runner and uses the action to execute the test (including TestName overwrite).
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected ITestBaseResult Test(Action<IRunnerSystemAndBrowser> action)
    {
        return this.Test(null, action);
    }

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected ITestBaseResult Test(string? scenario, Action<IRunnerSystemAndBrowser> action)
    {
        return TestBaseSingletonContainer.TestBaseStrategy.Test(
            this,
            scenario,
            action);
    }

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="strategy">The strategy that runs the test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected ITestBaseResult Test(string? scenario, ITestBaseStrategy strategy, Action<IRunnerSystemAndBrowser> action)
    {
        return Ensure.NotNull(strategy).Test(
            this,
            scenario,
            action);
    }

    /// <summary>
    /// Minimal execution of a maintenance task as action.
    /// It is a safe method that registers the maintenance task for the different statistics.
    /// </summary>
    /// <param name="action">The action to execute safely.</param>
    /// <returns>Test outcome and exception if any.</returns>
    protected ITestBaseResult Maintenance(Action action)
    {
#pragma warning disable CA1031 // don't catch general exceptions

        var scope = TestBaseSingletonContainer.TestBaseUsageStatisticsManager.Register(this);
        try
        {
            Ensure.NotNull(action).Invoke();
            return new TestBaseResult(TestOutcome.Success, scope);
        }
        catch (Exception exc)
        {
            return new TestBaseResult(TestOutcome.Failure, scope, [exc]);
        }

#pragma warning restore CA1031 // don't catch general exceptions
    }
}
