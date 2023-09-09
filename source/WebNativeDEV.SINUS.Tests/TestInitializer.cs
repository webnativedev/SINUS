// <copyright file="TestInitializer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1402 // File may only contain a single type

namespace WebNativeDEV.SINUS.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.Ioc;
    using WebNativeDEV.SINUS.Core.MsTest.Extensions;
    using WebNativeDEV.SINUS.MsTest;

    /// <summary>
    /// Standard test class used especially to store the testing context and for some configuration tasks.
    /// </summary>
    [TestClass]
    public class TestInitializer : TestBase
    {
        private const int MaxAgeOfProessInMinutes = 2;

        /// <summary>
        /// Method that is called by the MS-Test Framework on assmebly startup.
        /// </summary>
        /// <param name="testContext">The current context of the test execution (assembly level).</param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
            => Setup(container => { }, testContext);

        /// <summary>
        /// Method that is called by the MS-Test Framework on assmebly cleanup.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
            => TearDown(MaxAgeOfProessInMinutes);

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
            => new Action(() => this.CountChromeZombieProcesses(MaxAgeOfProessInMinutes)).Should().NotThrow();

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
}

namespace WebNativeDEV.SINUS.Tests.ZZZ
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.MsTest.Extensions;
    using WebNativeDEV.SINUS.Core.Sut;
    using WebNativeDEV.SINUS.MsTest;

    /// <summary>
    /// This test is named in an odd fashion, because it will then be executed as
    /// the last test by ms-test.
    /// </summary>
    [TestClass]
    public class ZTest : TestBase
    {
        /// <summary>
        /// Last test to be executed, so that all statistics.
        /// </summary>
        [TestMethod("Final Summary")]
        public void TestSummary()
        {
            // https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=mstest
            // based on the name, this should be the last test in the test-set.
            Assert.IsTrue(true);

            this.AssertOnDataLeak();
        }
    }
}