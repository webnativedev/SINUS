// <copyright file="SinusWafUsageStatisticsManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Sut;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.MsTest;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Static class that collects statistical data.
/// </summary>
public static class SinusWafUsageStatisticsManager
{
    /// <summary>
    /// Gets a list of test-identifiers that includes a browser.
    /// </summary>
    public static IList<string> TestsIncludingWaf { get; } = new List<string>();

    /// <summary>
    /// Gets a list of test-identifiers that released the browser after using it.
    /// </summary>
    public static IList<string> TestsDisposingWaf { get; } = new List<string>();

    /// <summary>
    /// Prints the usage statistics of the Web application factory.
    /// </summary>
    /// <param name="filter">A Filter to search for.</param>
    public static void PrintWafUsageStatistic(string? filter = null)
    {
        var including = SinusWafUsageStatisticsManager.TestsIncludingWaf.Where(x => filter == null || x == filter).ToList();
        if (!including.Any())
        {
            return;
        }

        var usageLogger = TestBaseSingletonContainer.CreateLogger<TestBase>();
        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation("| Tests Including Waf: {Count}", including.Count);

        foreach (var testIdsIncludingWaf in including)
        {
            var disposedInfo = SinusWafUsageStatisticsManager.TestsDisposingWaf.Contains(testIdsIncludingWaf)
                                    ? "disposed"
                                    : "leak    ";
            usageLogger.LogInformation("| ({DisposedInfo}) {Id}", disposedInfo, testIdsIncludingWaf);
        }

        usageLogger.LogInformation("+--------------------------------");
        usageLogger.LogInformation(" ");
    }
}
