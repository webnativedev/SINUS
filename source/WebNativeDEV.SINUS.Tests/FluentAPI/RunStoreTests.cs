// <copyright file="RunStoreTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.FluentAPI;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.FluentAPI;
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
        new object?[] { DateTime.Now, "DateTimeTest" },
    };

    public static string DataDisplayName(MethodInfo methodInfo, object[] data)
        => TestNamingConventionManager.DynamicDataDisplayNameAddValueFromLastArgument(methodInfo, data);

    [TestMethod]
    [DynamicData(
        nameof(ValidValues),
        DynamicDataDisplayName = nameof(DataDisplayName))]
    public void Given_ARunStore_When_AddingValues_Then_TheyShouldBeStored(object? value, string scenario)
        => this.Test(r => r
            .Given()
            .When(data => data.Actual = value)
            .Then(
                data => data.Actual.Should().NotBeNull(),
                data => data.ReadActualObject().Should().NotBeNull(),
                data => data[RunStore.KeyActual].Should().NotBeNull(),
                data => data.ReadObject(RunStore.KeyActual).Should().NotBeNull())
            .DebugPrint(nameof(scenario), scenario));
}
