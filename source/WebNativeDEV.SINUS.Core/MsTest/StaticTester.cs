// <copyright file="StaticTester.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.Core.MsTest.Context;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Static Tester that can be used outside of an unit test.
/// </summary>
public class StaticTester : TestBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StaticTester"/> class.
    /// </summary>
    /// <param name="context">The test context to run in.</param>
    private StaticTester(TestContext context)
    {
        this.TestContext = context;
    }

    /// <summary>
    /// Creates a runner and uses the action to execute the test (including TestName overwrite).
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public static new ITestBaseResult Test(Action<IRunnerSystemAndBrowser> action)
        => new StaticTester(StaticTesterContext.CreateStaticTest()).PublicTest(action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test (including TestName overwrite).
    /// </summary>
    /// <param name="context">The test context to run in.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public static ITestBaseResult Test(TestContext context, Action<IRunnerSystemAndBrowser> action)
        => new StaticTester(context).PublicTest(action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public static new ITestBaseResult Test(string? scenario, Action<IRunnerSystemAndBrowser> action)
        => new StaticTester(StaticTesterContext.CreateStaticTest()).PublicTest(scenario, action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="context">The test context to run in.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public static ITestBaseResult Test(string? scenario, TestContext context, Action<IRunnerSystemAndBrowser> action)
        => new StaticTester(context).PublicTest(scenario, action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="strategy">The strategy that runs the test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public static new ITestBaseResult Test(string? scenario, ITestBaseStrategy strategy, Action<IRunnerSystemAndBrowser> action)
        => new StaticTester(StaticTesterContext.CreateStaticTest()).PublicTest(scenario, strategy, action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="context">The test context to run in.</param>
    /// <param name="strategy">The strategy that runs the test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public static ITestBaseResult Test(string? scenario, TestContext context, ITestBaseStrategy strategy, Action<IRunnerSystemAndBrowser> action)
        => new StaticTester(context).PublicTest(scenario, strategy, action);

    /// <summary>
    /// Minimal execution of a maintenance task as action.
    /// It is a safe method that registers the maintenance task for the different statistics.
    /// </summary>
    /// <param name="action">The action to execute safely.</param>
    /// <returns>Test outcome and exception if any.</returns>
    public static new ITestBaseResult Maintenance(Action action)
        => new StaticTester(StaticTesterContext.CreateMaintenance()).PublicMaintenance(action);

    /// <summary>
    /// Minimal execution of a maintenance task as action.
    /// It is a safe method that registers the maintenance task for the different statistics.
    /// </summary>
    /// <param name="context">The test context to run in.</param>
    /// <param name="action">The action to execute safely.</param>
    /// <returns>Test outcome and exception if any.</returns>
    public static ITestBaseResult Maintenance(TestContext context, Action action)
        => new StaticTester(context).PublicMaintenance(action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test (including TestName overwrite).
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public ITestBaseResult PublicTest(Action<IRunnerSystemAndBrowser> action)
        => base.Test(action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public ITestBaseResult PublicTest(string? scenario, Action<IRunnerSystemAndBrowser> action)
        => base.Test(scenario, action);

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="scenario">The name of the scenario.</param>
    /// <param name="strategy">The strategy that runs the test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    public ITestBaseResult PublicTest(string? scenario, ITestBaseStrategy strategy, Action<IRunnerSystemAndBrowser> action)
        => base.Test(scenario, strategy, action);

    /// <summary>
    /// Minimal execution of a maintenance task as action.
    /// It is a safe method that registers the maintenance task for the different statistics.
    /// </summary>
    /// <param name="action">The action to execute safely.</param>
    /// <returns>Test outcome and exception if any.</returns>
    public ITestBaseResult PublicMaintenance(Action action)
        => base.Maintenance(action);
}
