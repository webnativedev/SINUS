// <copyright file="RunStoreTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.FluentAPI;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class RunStoreTests : TestBase
{
    public static IEnumerable<object?[]> ValidValues
    => new[]
    {
        new object?[] { 1, "IntTest" },
        new object?[] { "str", "StringTest" },
        new object?[] { new object(), "ObjectTest" },
        new object?[] { new DateTime(2023, 1, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc), "DateTimeTest" },
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
    public void Given_ARunStore_When_AddingValues_Then_TheyShouldBeStored(object? value, string scenario)
        => this.Test(scenario, r => r
            .Given()
            .When(data => data.Actual = value)
            .Then(
                data => data.Actual.Should().NotBeNull(),
                data => data.ReadActualObject().Should().NotBeNull(),
                data => data[data.KeyActual].Should().NotBeNull(),
                data => data.Should().ActualBeNotNull(),
                data => data.ReadObject(data.KeyActual).Should().NotBeNull())
            .DebugPrint(RunStorePrintOrder.KeySorted, nameof(scenario), scenario));
}
