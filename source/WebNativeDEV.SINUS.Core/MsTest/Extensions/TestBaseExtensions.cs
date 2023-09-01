// <copyright file="TestBaseExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Extensions;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Ioc;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Extension Methods that add value to the test base, but are not required base functionality.
/// </summary>
public static class TestBaseExtensions
{
    /// <summary>
    /// Counts the result folders based on trusted parameters.
    /// This method should avoid unneeded amount of garbage.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="max">Maximal amount of folders accepted.</param>
    public static void CountResultFoldersBelowParameter(this TestBase testBase, int max)
    {
        Ensure.NotNull(testBase);

        ILogger<TestBase> logger = TestBase.Container?.Resolve<ILoggerFactory>()?.CreateLogger<TestBase>()
            ?? throw new ArgumentNullException(nameof(testBase), "extended object is null");

        // assumption that each Run has a run-directory below
        // the main TestResults folder (as standard)
        int count = Directory.GetDirectories(Path.Combine(testBase.RunDir, "..")).Length;

        if (count >= max)
        {
            throw new InvalidOperationException($"too many result folders; <{max} wanted, but are {count}");
        }

        logger.LogInformation("Foldercount: {Count} / max:{Max}", count, max);
    }

    /// <summary>
    /// Checks for zombie processes older as defined parameter.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="maxAgeOfProessInMinutes">Max age for old a process should be to identify it as zombie.</param>
    public static void CountChromeZombieProcesses(this TestBase testBase, int maxAgeOfProessInMinutes)
    {
        Ensure.NotNull(testBase);

        ILogger<TestBase> logger = TestBase.Container.Resolve<ILoggerFactory>().CreateLogger<TestBase>();

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
        ILogger<TestBase> logger = TestBase.Container.Resolve<ILoggerFactory>().CreateLogger<TestBase>();

        var processes = GetChromeDriverProcesses(maxAgeOfProessInMinutes);
        foreach(var process in processes)
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
