// <copyright file="LoggerExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Extensions;

using Microsoft.Extensions.Logging;

/// <summary>
/// Extension methods for the logger.
/// </summary>
internal static class LoggerExtensions
{
    /// <summary>
    /// Creates a performance tracking scope, mainly used with a using Block.
    /// </summary>
    /// <param name="logger">The logger to log the start and time to.</param>
    /// <param name="prefix">RunCategory-string or any other custom string.</param>
    /// <param name="description">
    /// A plain text string containing further
    /// information that is also logged.
    /// </param>
    /// <returns>An IDisposable that can be used in a using block.</returns>
    public static IDisposable CreatePerformanceDataScope(
        this ILogger logger,
        string? prefix = null,
        string? description = null)
    {
        return new PerformanceDataScope(logger, prefix, description);
    }
}
