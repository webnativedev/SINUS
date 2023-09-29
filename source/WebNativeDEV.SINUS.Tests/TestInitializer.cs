// <copyright file="TestInitializer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1402 // File may only contain a single type

[assembly: Microsoft.VisualStudio.TestTools.UnitTesting.TestDataSourceDiscovery(Microsoft.VisualStudio.TestTools.UnitTesting.TestDataSourceDiscoveryOption.DuringExecution)]

namespace WebNativeDEV.SINUS.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.Assertions;
    using WebNativeDEV.SINUS.Core.MsTest;
    using WebNativeDEV.SINUS.Core.Utils;
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
            => TestBaseSingletonContainer.AssemblyTestContext = testContext;

        /// <summary>
        /// Method that is called by the MS-Test Framework on assmebly cleanup.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            SinusUtils.KillChromeZombieProcesses(MaxAgeOfProessInMinutes);
        }

        /// <summary>
        /// Maintenance Test related to the number of output folders.
        /// Fails if too much folders are created.
        /// </summary>
        [TestMethod]
        public void Maintenance_CountOfResultFoldersBelow200()
        {
            this.Maintenance(() => SinusUtils.CountResultFoldersBelowParameter(this, max: 200)).Should().BeSuccessful();
        }

        /// <summary>
        /// Maintenance test related to zombie processes.
        /// Fails if too old processes stay on the machine.
        /// </summary>
        [TestMethod]
        public void Maintenance_ProcessesKilled()
        {
            this.Maintenance(() => SinusUtils.CountChromeZombieProcesses(MaxAgeOfProessInMinutes)).Should().BeSuccessful();
        }
    }
}

namespace WebNativeDEV.SINUS.Tests.ZZZ
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebNativeDEV.SINUS.Core.Assertions;
    using WebNativeDEV.SINUS.Core.Utils;
    using WebNativeDEV.SINUS.MsTest;

    /// <summary>
    /// This test is named in an odd fashion, because it will then be executed as
    /// the last test by ms-test.
    /// </summary>
    [TestClass]
    public class ZTest : TestBase
    {
        /// <summary>
        /// Last test to be executed (based on name), so that all statistics.
        /// Based on https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=mstest
        /// the naming influences the execution order in mstest.
        /// </summary>
        [TestMethod("Final Summary")]
        public void Maintenance_ZZZ_TestSummary()
        {
            this.Maintenance(() => SinusUtils.AssertOnDataLeak()).Should().BeSuccessful();
        }
    }
}