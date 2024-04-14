// <copyright file="MsTestTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.MsTest;

using FluentAssertions;
using global::WebNativeDEV.SINUS.MsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI.Model;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class MsTestTests : TestBase
{
    [TestMethod]
    public void Given_MinimalTest_When_CheckResult_Then_DataSuccessShouldBePresent()
    {
        this.Test(r => { }).Outcome.Should().Be(TestOutcome.Success);
    }

    [TestMethod]
    public void Given_ASolidTestBase_When_CheckingVariables_Then_NonShouldBeNull()
    {
        this.Test(r => r
            .Given("solid testbase")
            .When("storing data", data =>
            {
                data["logsDir"] = this.TestContext.TestRunResultsDirectory;
                data["runDir"] = this.TestContext.TestRunDirectory;
                data["testName"] = data.TestName;
            })
            .Then(
                "all data should be not null",
                data => data["logsDir"].Should().NotBeNull(),
                data => data["runDir"].Should().NotBeNull(),
                data => data["testName"].Should().NotBeNull())
            .DebugPrint());
    }
}
