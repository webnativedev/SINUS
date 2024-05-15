// <copyright file="SinusUtils.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Utils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Container for some utility methods.
/// </summary>
public static class SinusUtils
{
    private const int SecondsDelay = 3;

    /// <summary>
    /// Prints the usage statistics of the browser objects.
    /// </summary>
    /// <param name="filter">Optional filter to search for.</param>
    public static void PrintUsageStatistic(string? filter = null)
    {
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintBrowserUsageStatistic(filter);
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintWafUsageStatistic(filter);
    }

    /// <summary>
    /// Counts the result folders based on trusted parameters.
    /// This method should avoid unneeded amount of garbage.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="max">Maximal amount of folders accepted.</param>
    public static void CountResultFoldersBelowParameter(TestBase testBase, int max)
    {
        var logger = TestBaseSingletonContainer.CreateLogger<TestBase>();

        // assumption that each Run has a run-directory below
        // the main TestResults folder (as standard)
        string path = Path.Combine(Ensure.NotNull(testBase).TestContext.TestRunDirectory ?? ".", "..");
        int count = Directory.GetDirectories(path).Length;

        if (count >= max)
        {
            logger.LogError("Foldercount: {Count} / max:{Max} for {Path}", count, max, path);
            throw new InvalidOperationException($"too many result folders; <{max} wanted, but are {count}");
        }

        logger.LogInformation("Foldercount: {Count} / max:{Max} for {Path}", count, max, path);
    }

    /// <summary>
    /// Asserts if a browser or a web application factory was not disposed.
    /// </summary>
    /// <returns>Successful check or exception on fail.</returns>
    public static bool AssertOnDataLeak()
    {
#pragma warning disable S1215 // "GC.Collect" should not be called

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();
        Thread.Sleep(TimeSpan.FromSeconds(SecondsDelay));

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintUsageStatistic();

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.CheckAttribute(
            data => (
                        data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserCreated) &&
                        data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserDisposed))
                    || (
                        !data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserCreated) &&
                        !data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeBrowserDisposed)),
            shouldThrow: true).Should().BeTrue();

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.CheckAttribute(
            data => (
                        data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafCreated) &&
                        data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafDisposed))
                    || (
                        !data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafCreated) &&
                        !data.ContainsKey(TestBaseSingletonContainer.TestBaseUsageStatisticsManager.AttributeWafDisposed)),
            shouldThrow: true).Should().BeTrue();

        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintBusinessRequirements();
        TestBaseSingletonContainer.TestBaseUsageStatisticsManager.PrintTechnicalRequirements();
        return true;

#pragma warning restore S1215 // "GC.Collect" should not be called
    }

    /// <summary>
    /// Checks for zombie processes older as defined parameter.
    /// </summary>
    /// <param name="maxAgeOfProessInMinutes">Max age for old a process should be to identify it as zombie.</param>
    public static void CountChromeZombieProcesses(int maxAgeOfProessInMinutes)
    {
        var logger = TestBaseSingletonContainer.CreateLogger<TestBase>();

        var processes = GetChromeDriverProcesses(maxAgeOfProessInMinutes);

        processes.Should().BeEmpty($"zombie drivers should not exist, but count: {processes.Count}");

        logger.LogInformation(
            "Processcount: {Count} processes older than {AgeInMin} min",
            processes.Count,
            maxAgeOfProessInMinutes);
    }

    /// <summary>
    /// Kills zombie processes older as defined parameter.
    /// </summary>
    /// <param name="maxAgeOfProessInMinutes">Max age for old a process should be to identify it as zombie.</param>
    /// <returns>The count of killed processes.</returns>
    public static int KillChromeZombieProcesses(int maxAgeOfProessInMinutes)
    {
        var logger = TestBaseSingletonContainer.CreateLogger<TestBase>();

        var processes = GetChromeDriverProcesses(maxAgeOfProessInMinutes);
        foreach (var process in processes)
        {
            process.Kill(entireProcessTree: true);
        }

        logger.LogInformation(
            "Kill Process: {Count} processes older than {AgeInMin} min killed",
            processes.Count,
            maxAgeOfProessInMinutes);

        return processes.Count;
    }

    /// <summary>
    /// Runs an action if at least one visual studio instance is running.
    /// </summary>
    /// <param name="action">The action to run.</param>
    [ExcludeFromCodeCoverage]
    public static void RunOnlyInsideVisualStudio(Action action)
    {
        if (Process.GetProcessesByName("devenv").Length > 0)
        {
            action?.Invoke();
        }
    }

    /// <summary>
    /// Runs an action if at least one visual studio instance is running.
    /// </summary>
    /// <param name="action">The action to run.</param>
    [ExcludeFromCodeCoverage]
    public static void RunOnlyInsideDebugSession(Action action)
    {
        if (Debugger.IsAttached)
        {
            action?.Invoke();
        }
    }

    private static List<Process> GetChromeDriverProcesses(int maxAgeOfProessInMinutes)
        => Process.GetProcesses().Where(
            x => x.ProcessName.Contains("chromedriver", StringComparison.InvariantCultureIgnoreCase)
            && x.StartTime < DateTime.Now.AddMinutes(-maxAgeOfProessInMinutes))
            .ToList();
}
