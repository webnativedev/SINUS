// <copyright file="EnsureTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.Internals.ArgumentValidation;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.ArgumentValidation.Exceptions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.Requirements;
using WebNativeDEV.SINUS.MsTest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
[BusinessRequirements(
    "ArgumentValidation",
    "B001:Null Should be prohibited")]
[TechnicalRequirements(
    "ArgumentValidation",
    "T001:Non-Null Values of T? should be converted in guaranteed T")]
public class EnsureTests : TestBase
{
    public static IEnumerable<object?[]> ValidValues
        => new[]
        {
            new object?[] { "test", "TestString" },
            new object?[] { new int?(5), "NullableTestInt5" },
            new object?[] { 1, "IntOne" },
            new object?[] { 2.3, "DoubleTwoPointThree" },
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
    public void Given_AValue_When_CallingArgumentValidationNotNullWithNull_Then_AnExceptionShouldHaveBeenThrown()
        => this.Test(r => r
            .Given(data => data["value"] = null)
             .When(data => Ensure.NotNull(data["value"]))
             .ThenShouldHaveFailedWith<ArgumentValidationException>());

    [TestMethod]
    public void Given_AFailingTest_When_Fail_Then_WrongExceptionExpectedShouldThrow()
        => this.Test(r => r
            .Given(data => data["value"] = null)
             .When(data => Ensure.NotNull(data["value"]))
             .ThenShouldHaveFailedWith<IOException>()
            .ExpectFail());

    [TestMethod]
    public void Given_AValue_When_CallingArgumentValidationNotNullWithNull_Then_AGeneralExceptionShouldHaveBeenThrown()
        => this.Test(r => r
            .Given(data => data["value"] = null)
             .When(data => Ensure.NotNull(data["value"]))
             .ThenShouldHaveFailed());

    [TestMethod]
    public void Given_AValue_When_CallingArgumentValidationNotNullWithNull_Then_AnExceptionWithDescriptionShouldHaveBeenThrown()
        => this.Test(r => r
            .Given(data => data["value"] = null)
             .When(data => Ensure.NotNull(data["value"]))
             .ThenShouldHaveFailed("check for not null should identify null value."));

    [TestMethod]
    [DynamicData(
        nameof(ValidValues),
        DynamicDataDisplayName = nameof(DefaultDataDisplayName))]
    public void Given_AValue_When_CallingArgumentValidationNotNullWithValues_Then_NoExceptionShouldBeThrown(object? value, string scenario)
        => this.Test(scenario, r => r
            .Given(data => data["value"] = value)
             .When(data => data["checkedValue"] = Ensure.NotNull(data["value"]))
             .Then(
                   data => data["value"].Should().Be(value),
                   data => Nullable.GetUnderlyingType(data["checkedValue"]?.GetType() ?? typeof(int))
                                .Should().BeNull())
            .DebugPrint("scenario", scenario));
}
