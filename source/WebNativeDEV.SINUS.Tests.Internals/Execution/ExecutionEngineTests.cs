// <copyright file="ExecutionEngineTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Internals.Execution;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Reflection;
using WebNativeDEV.SINUS.Core.Execution;
using WebNativeDEV.SINUS.Core.Execution.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class ExecutionEngineTests : TestBase
{
    public static IEnumerable<object?[]> ValidValues
    => new[]
    {
        new object?[]
        {
            new ExecutionParameter()
            {
                Actions = { () => { } },
                TestBase = new ExecutionEngineTests(),
                Namings = new TestNamingConventionManager("Given_X_When_Y_Then_Z"),
            },
            "MinimalTest",
        },
    };

    /// <summary>
    /// Dynamic Data Display Name calculator proxying to TestNamingConventionManager.
    /// This works when the test naming conventions are met.
    /// </summary>
    /// <param name="methodInfo">The method to work on.</param>
    /// <param name="data">The arguments, but with the convention that the last object contains the testname.</param>
    /// <returns>A calculated name of the test.</returns>
    public static string DefaultDataDisplayName(MethodInfo methodInfo, object[] data)
        => TestNamingConventionManager.DynamicDataDisplayNameAddValueFromLastArgument(methodInfo, data);

    [TestMethod]
    [DynamicData(
        nameof(ValidValues),
        DynamicDataDisplayName = nameof(DefaultDataDisplayName))]
    public void Given_AValue_When_CallingArgumentValidationNotNullWithValues_Then_NoExceptionShouldBeThrown(object? value, string scenario)
    => this.Test(scenario, r => r
        .Given(data => data.StoreSut(new ExecutionEngine(Substitute.For<ILoggerFactory>())))
        .When(data => data.StoreActual<ExecutionEngine>(ex => ex.Run(value as ExecutionParameter)))
        .Then(data => data.ReadActual<ExecutionOutput>().Exceptions.Should().BeNullOrEmpty())
        .DebugPrint(new (string, object?)[]
        {
            ("scenario", scenario),
            ("test", 123),
        }));
}
