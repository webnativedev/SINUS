// <copyright file="TestInitializer.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using WebNativeDEV.SINUS.Core.MsTest.Chrome.Extensions;
using WebNativeDEV.SINUS.Core.MsTest.Extensions;
using WebNativeDEV.SINUS.MsTest.Chrome;

/// <summary>
/// Wrapper class used to store the testing context.
/// </summary>
[TestClass]
public class TestInitializer : ChromeTestBase
{
    /// <summary>
    /// Method that is called by the MS-Test Framework on assmebly startup.
    /// </summary>
    /// <param name="testContext">The current context of the test execution (assembly level).</param>
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
        => StoreAssemblyTestContext(testContext);

    /// <summary>
    /// Method that is called by the MS-Test Framework on class initialization.
    /// </summary>
    /// <param name="testContext">The current context of the test execution (class level).</param>
    /// <returns>Async context.</returns>
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext testContext)
        => await StoreClassTestContext(testContext);

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        PrintBrowserUsageStatistic();
    }

    /// <summary>
    /// Maintenance Test related to the number of output folders.
    /// Fails if too much folders are created.
    /// </summary>
    [TestMethod]
    public void Maintenance_CountOfResultFoldersBelow200()
        => this.CountResultFoldersBelowParameter(
                max: 200);

    /// <summary>
    /// Maintenance test related to zombie processes.
    /// Fails if too old processes stay on the machine.
    /// </summary>
    [TestMethod]
    public void Maintenance_ProcessesKilled()
        => this.CountZombieProcesses(maxAgeOfProessInMinutes: 2);
}
