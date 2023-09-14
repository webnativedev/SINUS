// <copyright file="MsTestTests.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests.MsTest;

using FluentAssertions;
using global::WebNativeDEV.SINUS.MsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Assertions;
using WebNativeDEV.SINUS.Core.FluentAPI;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.MsTest.Contracts;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'.
#pragma warning disable SA1600 // Elements should be documented

[TestClass]
public class MsTestTests : TestBase
{
    [TestMethod]
    public void Given_AssemblyTestContext_When_Creating_Then_DataShouldBeStored()
        => this.Test(r => r
            .GivenASimpleSystem(new AssemblyTestContext(Substitute.For<TestContext>()))
            .When<IAssemblyTestContext>((sut, data) => data.Actual = sut.TestContext)
            .Then(data => data.Should().ActualBeNotNull()));

    [TestMethod]
    public void Given_MinimalTest_When_CheckResult_Then_DataSuccessShouldBePresent()
    {
        this.Test(r => { }).Success.Should().BeTrue();
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
                data["testName"] = this.TestName;
            })
            .Then(
                "all data should be not null",
                data => Assert.IsNotNull(data["logsDir"]),
                data => Assert.IsNotNull(data["runDir"]),
                data => Assert.IsNotNull(data["testName"]))
            .DebugPrint());
    }
}
