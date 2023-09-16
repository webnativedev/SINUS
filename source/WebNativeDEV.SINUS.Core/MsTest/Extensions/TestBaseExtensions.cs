// <copyright file="TestBaseExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Extensions;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Sut;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Extension Methods that add value to the test base, but are not required base functionality.
/// </summary>
public static class TestBaseExtensions
{
    private const int SecondsDelay = 3;

    /// <summary>
    /// Prints the usage statistics of the browser objects.
    /// </summary>
    /// <param name="testBase">Required to use the function as extension method.</param>
    /// <param name="filter">Optional filter to search for.</param>
    public static void PrintUsageStatistic(this TestBase testBase, string? filter = null)
    {
        Ensure.NotNull(testBase);
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintBrowserUsageStatistic(filter);
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintWafUsageStatistic(filter);
    }

    /// <summary>
    /// Counts the result folders based on trusted parameters.
    /// This method should avoid unneeded amount of garbage.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="max">Maximal amount of folders accepted.</param>
    public static void CountResultFoldersBelowParameter(this TestBase testBase, int max)
    {
        Ensure.NotNull(testBase);

        var logger = TestBaseSingletonContainer.CreateLogger<TestBase>();

        // assumption that each Run has a run-directory below
        // the main TestResults folder (as standard)
        int count = Directory.GetDirectories(Path.Combine(testBase.TestContext.TestRunDirectory ?? ".", "..")).Length;

        if (count >= max)
        {
            throw new InvalidOperationException($"too many result folders; <{max} wanted, but are {count}");
        }

        logger.LogInformation("Foldercount: {Count} / max:{Max}", count, max);
    }

    /// <summary>
    /// Asserts if a browser or a web application factory was not disposed.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <returns>Successful check or exception on fail.</returns>
    public static bool AssertOnDataLeak(this TestBase testBase)
    {
#pragma warning disable S1215 // "GC.Collect" should not be called

        Ensure.NotNull(testBase);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();
        Thread.Sleep(TimeSpan.FromSeconds(SecondsDelay));

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.CheckAttribute(
            data => data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserCreated) &&
                    data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserDisposed));

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.CheckAttribute(
            data => data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafCreated) &&
                    data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafDisposed));

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintUsageStatistic();
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintBusinessRequirements();
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintTechnicalRequirements();
        return true;

#pragma warning restore S1215 // "GC.Collect" should not be called
    }

    /// <summary>
    /// Checks for zombie processes older as defined parameter.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="maxAgeOfProessInMinutes">Max age for old a process should be to identify it as zombie.</param>
    public static void CountChromeZombieProcesses(this TestBase testBase, int maxAgeOfProessInMinutes)
    {
        Ensure.NotNull(testBase);

        var logger = TestBaseSingletonContainer.CreateLogger<TestBase>();

        var processes = GetChromeDriverProcesses(maxAgeOfProessInMinutes);

        processes.Should().BeEmpty($"zombie drivers should not exist, but count: {processes.Count}");

        logger.LogInformation(
            "{TestName} - Processcount: {Count} processes older than {AgeInMin} min",
            testBase.TestName,
            processes.Count,
            maxAgeOfProessInMinutes);
    }

    /// <summary>
    /// Kills zombie processes older as defined parameter.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="maxAgeOfProessInMinutes">Max age for old a process should be to identify it as zombie.</param>
    public static void KillChromeZombieProcesses(this TestBase testBase, int maxAgeOfProessInMinutes)
    {
        var logger = TestBaseSingletonContainer.CreateLogger<TestBase>();

        var processes = GetChromeDriverProcesses(maxAgeOfProessInMinutes);
        foreach (var process in processes)
        {
            process.Kill(entireProcessTree: true);
        }

        logger.LogInformation(
            "{TestName} - Kill Process: {Count} processes older than {AgeInMin} min killed",
            testBase?.TestName ?? "global",
            processes.Count,
            maxAgeOfProessInMinutes);
    }

    private static IList<Process> GetChromeDriverProcesses(int maxAgeOfProessInMinutes)
        => Process.GetProcesses().Where(
            x => x.ProcessName.Contains("chromedriver", StringComparison.InvariantCultureIgnoreCase)
            && x.StartTime < DateTime.Now.AddMinutes(-maxAgeOfProessInMinutes))
            .ToList();
}
