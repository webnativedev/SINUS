// <copyright file="LoggerExtensions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.Extensions;

using Microsoft.Extensions.Logging;

internal static class LoggerExtensions
{
    public static IDisposable CreatePerformanceDataScope(
        this ILogger logger,
        string? prefix = null,
        string? description = null)
    {
        return new PerformanceDataScope(logger, prefix, description);
    }
}
