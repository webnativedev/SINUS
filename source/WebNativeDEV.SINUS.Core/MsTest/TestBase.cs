// <copyright file="TestBase.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.MsTest;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Events;
using WebNativeDEV.SINUS.Core.Events.Contracts;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Contracts;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.FluentAPI.Contracts;
using WebNativeDEV.SINUS.Core.Logging;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.Core.MsTest.Extensions;
using WebNativeDEV.SINUS.Core.Sut;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.Core.UITesting.Contracts;

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
    /// Gets the name of the current test.
    /// </summary>
    public string TestName => this.TestContext.TestName!;

    /// <summary>
    /// Creates a runner and uses the action to execute the test.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The current object for usage as Fluent API.</returns>
    protected ITestBaseResult Test(Action<IBrowserRunner> action)
    {
        Ensure.NotNull(action);

        var scope = new TestBaseScopeContainer(this);

        var runner = new Runner(scope);
        action.Invoke(runner);
        runner.Dispose();

        return new TestBaseResult(true, scope);
    }
}
