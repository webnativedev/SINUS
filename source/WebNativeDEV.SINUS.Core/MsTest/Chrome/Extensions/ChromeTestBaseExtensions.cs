// <copyright file="ChromeTestBaseExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Chrome.Extensions;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WebNativeDEV.SINUS.MsTest.Chrome;

/// <summary>
/// Extension Methods that add value to chrome test base, but are not required base functionality.
/// </summary>
public static class ChromeTestBaseExtensions
{
    /// <summary>
    /// Checks for zombie processes older as defined parameter.
    /// </summary>
    /// <param name="testBase">Reference to the test base object that is extended.</param>
    /// <param name="maxAgeOfProessInMinutes">Max age for old a process should be to identify it as zombie.</param>
    public static void CountZombieProcesses(this ChromeTestBase testBase, int maxAgeOfProessInMinutes)
    {
        ILogger<ChromeTestBase> logger = testBase?.LoggerFactory?.CreateLogger<ChromeTestBase>()
            ?? throw new ArgumentNullException(nameof(testBase), "extended object is null");

        var processes = System.Diagnostics.Process.GetProcesses().Where(
            x => x.ProcessName.Contains("chromedriver", StringComparison.InvariantCultureIgnoreCase)
            && x.StartTime < DateTime.Now.AddMinutes(-maxAgeOfProessInMinutes))
            .ToList();

        Assert.IsTrue(processes.Count == 0, $"zombie drivers exist: {processes.Count}");

        logger.LogInformation(
            "Processcount: {Count} processes older than {AgeInMin} min",
            processes.Count,
            maxAgeOfProessInMinutes);
    }
}
