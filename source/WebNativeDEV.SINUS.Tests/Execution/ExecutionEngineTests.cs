// <copyright file="ExecutionEngineTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Execution;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Execution;
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
                Actions = new List<Action?>() { () => { } },
                TestBase = new ExecutionEngineTests(),
            },
            "MinimalTest",
        },
    };

    [TestMethod]
    [DynamicData(
    nameof(ValidValues),
    DynamicDataDisplayName = nameof(DefaultDataDisplayName))]
    public void Given_AValue_When_CallingArgumentValidationNotNullWithValues_Then_NoExceptionShouldBeThrown(object? value, string scenario)
    => this.Test(r => r
        .Given(data => data.StoreSut(new ExecutionEngine(Substitute.For<ILoggerFactory>())))
        .When(data => data.StoreActual<ExecutionEngine>(ex => ex.Run(value as ExecutionParameter)))
        .Then(data => data.ReadActual<ExecutionOutput>().Exceptions.Should().BeNullOrEmpty())
        .DebugPrint("scenario", scenario));
}
