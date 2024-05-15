// <copyright file="ITestBaseStrategy.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts.Runner;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Strategy interface defining the method to execute a test.
/// </summary>
public interface ITestBaseStrategy
{
    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="testBase">Reference to the class containing the test.</param>
    /// <param name="scenario">Scenario name for a test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    ITestBaseResult Test(TestBase testBase, string? scenario, Action<IRunnerSystemAndBrowser> action);

    /// <summary>
    /// Creates a runner and uses the action to execute a maintenance task.
    /// </summary>
    /// <param name="testBase">Reference to the class containing the test.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    ITestBaseResult Maintenance(TestBase testBase, Action action);
}
