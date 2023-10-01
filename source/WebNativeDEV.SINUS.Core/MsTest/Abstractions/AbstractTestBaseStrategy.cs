// <copyright file="AbstractTestBaseStrategy.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Abstractions;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.Model;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Abstract base implementation for each strategy.
/// </summary>
public abstract class AbstractTestBaseStrategy : ITestBaseStrategy
{
    /// <inheritdoc/>
    public virtual ITestBaseResult Test(TestBase testBase, string? scenario, Action<IRunnerSystemAndBrowser> action)
    {
        return this.TestImplementation(testBase, scenario, action);
    }

    /// <summary>
    /// Creates a runner and uses the action to execute the test (real implementation).
    /// </summary>
    /// <param name="testBase">Reference to the class containing the test.</param>
    /// <param name="scenario">Scenario name for a test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected virtual ITestBaseResult TestImplementation(TestBase testBase, string? scenario, Action<IRunnerSystemAndBrowser> action)
    {
        TestBaseScopeContainer scope;
        do
        {
            scope = new TestBaseScopeContainer(testBase, scenario);
            this.RunAction(scope, action);
            this.CloseRunner(scope);
            this.InterpretResult(scope);
            this.DisposeRunner(scope);
        }
        while (this.CalculateShouldRepeat());

        return this.CreateTestBaseResult(scope);
    }

    /// <summary>
    /// Runs the actual action.
    /// </summary>
    /// <param name="scope">Object that points to all dependencies related to a test.</param>
    /// <param name="action">The action to execute.</param>
    protected virtual void RunAction(TestBaseScopeContainer scope, Action<IRunnerSystemAndBrowser> action)
    {
        Ensure.NotNull(action).Invoke(Ensure.NotNull(scope).Runner);
    }

    /// <summary>
    /// Closes the runner.
    /// </summary>
    /// <param name="scope">Object that points to all dependencies related to a test.</param>
    protected virtual void CloseRunner(TestBaseScopeContainer scope)
    {
        (Ensure.NotNull(scope).Runner as ICloseable)?.Close();
    }

    /// <summary>
    /// Interprets the result. (In fact sets the default for "should throw".
    /// </summary>
    /// <param name="scope">Object that points to all dependencies related to a test.</param>
    protected virtual void InterpretResult(TestBaseScopeContainer scope)
    {
        this.InterpretResult(scope, true);
    }

    /// <summary>
    /// Interprets the result.
    /// </summary>
    /// <param name="scope">Object that points to all dependencies related to a test.</param>
    /// <param name="shouldThrow">Sets a value that indicates whether assertion exception should be thrown on error.</param>
    protected virtual void InterpretResult(TestBaseScopeContainer scope, bool shouldThrow)
    {
        var logger = Ensure.NotNull(scope).CreateLogger<TestBase>();

        if (scope.IsPreparedOnly)
        {
            logger.LogWarning(
                "The test result is evaluated as inconclusive for test '{TestName}', because it was rated 'only-prepared' when seeing no 'When'-part. (TaskId: {TaskId}, ThreadId: {ThreadId})",
                scope.TestName,
                Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? "<null>",
                Environment.CurrentManagedThreadId);
            if (scope.ExpectedOutcome != TestOutcome.Inconclusive && shouldThrow)
            {
                Assert.Inconclusive($"The test result is evaluated as inconclusive for test '{scope.TestName}', because it was rated 'only-prepared' when seeing no 'When'-part.");
            }

            return;
        }

        if (scope.Exceptions.HasUncheckedElements)
        {
            logger.LogError(
                "The test result is evaluated as failed for test '{TestName}', because exceptions occured.\nCount: {Count}; Types: {Types} (TaskId: {TaskId}, ThreadId: {ThreadId})",
                scope.TestName,
                scope.Exceptions.Count,
                scope.Exceptions.GetContentAsString(),
                Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? "<null>",
                Environment.CurrentManagedThreadId);

            if (scope.ExpectedOutcome != TestOutcome.Failure && shouldThrow)
            {
                Assert.Fail($"The test result is evaluated as failed for test '{scope.TestName}', because exceptions occured. Count: {scope.Exceptions.Count}; Types: {scope.Exceptions.GetContentAsString()}");
            }

            return;
        }

        logger.LogInformation(
            "The test result is evaluated as successful for test '{TestName}'. (Checked Exceptions: {CheckedExceptionCount}, TaskId: {TaskId}, ThreadId: {ThreadId})",
            scope.TestName,
            scope.Exceptions.Count,
            Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? "<null>",
            Environment.CurrentManagedThreadId);
    }

    /// <summary>
    /// Disposes the runner object.
    /// </summary>
    /// <param name="scope">Object that points to all dependencies related to a test.</param>
    protected virtual void DisposeRunner(TestBaseScopeContainer scope)
    {
        Ensure.NotNull(scope).Runner.Dispose();
    }

    /// <summary>
    /// Creates a test outcome object.
    /// </summary>
    /// <param name="scope">Object that points to all dependencies related to a test.</param>
    /// <returns>The test outcome object.</returns>
    protected virtual ITestBaseResult CreateTestBaseResult(TestBaseScopeContainer scope)
    {
        return new TestBaseResult(TestOutcome.Success, scope);
    }

    /// <summary>
    /// Calculates if the test should be executed once more.
    /// </summary>
    /// <returns>The information to loop or not.</returns>
    protected virtual bool CalculateShouldRepeat()
    {
        return false;
    }
}
