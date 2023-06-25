// <copyright file="TestBaseExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Extensions;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebNativeDEV.SINUS.MsTest;
using WebNativeDEV.SINUS.MsTest.Chrome;

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
        ILogger<ChromeTestBase> logger = testBase?.LoggerFactory?.CreateLogger<ChromeTestBase>()
            ?? throw new ArgumentNullException(nameof(testBase), "extended object is null");

        // assumption that each Run has a run-directory below
        // the main TestResults folder (as standard)
        var count = Directory.GetDirectories(Path.Combine(testBase.RunDir, "..")).Length;

        Assert.IsTrue(count < max, $"too many result folders; <{max} wanted, but are {count}");

        logger.LogInformation("Foldercount: {Count} / max:{Max}", count, max);
    }
}
