﻿// <copyright file="TestInitializer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.Core.MsTest.Extensions;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Wrapper class used to store the testing context.
/// </summary>
[TestClass]
public class TestInitializer : TestBase
{
    /// <summary>
    /// Method that is called by the MS-Test Framework on assmebly startup.
    /// </summary>
    /// <param name="testContext">The current context of the test execution (assembly level).</param>
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
        TestBase.Setup(
            container =>
            {
            }, testContext);
    }

    /// <summary>
    /// Method that is called by the MS-Test Framework on assmebly cleanup.
    /// </summary>
    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        TestBase.TearDown();
    }

    /// <summary>
    /// Maintenance Test related to the number of output folders.
    /// Fails if too much folders are created.
    /// </summary>
    [TestMethod]
    public void Maintenance_CountOfResultFoldersBelow200()
        => new Action(() => this.CountResultFoldersBelowParameter(max: 200)).Should().NotThrow();

    /// <summary>
    /// Maintenance test related to zombie processes.
    /// Fails if too old processes stay on the machine.
    /// </summary>
    [TestMethod]
    public void Maintenance_ProcessesKilled()
        => new Action(() => this.CountChromeZombieProcesses(maxAgeOfProessInMinutes: 2)).Should().NotThrow();

    /// <summary>
    /// Maintenance Print Meta-Data.
    /// </summary>
    [TestMethod]
    public void Given_ASolidTestBase_When_CheckingVariables_Then_NonShouldBeNull()
    {
        this.Test()
            .Given("solid testbase")
            .When("storing data", data =>
            {
                data["logsDir"] = this.LogsDir;
                data["runDir"] = this.RunDir;
                data["testName"] = this.TestName;
            })
            .Then(
                "all data should be not null",
                data => Assert.IsNotNull(data["logsDir"]),
                data => Assert.IsNotNull(data["runDir"]),
                data => Assert.IsNotNull(data["testName"]))
            .DebugPrint();
    }
}
