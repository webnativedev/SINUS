// <copyright file="LogErrorExtension.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Logging;

using Microsoft.Extensions.Logging;
using System;

/// <summary>
/// Extensions for ILogger interface from microsoft.
/// </summary>
public static class LogErrorExtension
{
    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="logger">The Microsoft.Extensions.Logging.ILogger to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format.
    /// Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void LogErrorRec(this ILogger logger, Exception? exception, string? message, params object?[] args)
        => LogErrorRec(logger, exception, string.Empty, message, args, null);

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="logger">The Microsoft.Extensions.Logging.ILogger to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="prefix">The prefix to differentiate entries.</param>
    /// <param name="message">Format string of the log message in message template format.
    /// Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void LogErrorRec(this ILogger logger, Exception? exception, string prefix, string? message, params object?[] args)
    {
        #pragma warning disable CA2254 // Vorlage muss ein statischer Ausdruck sein
        logger.LogError(exception, message, args);
        LogErrorStackTrace(logger, exception, prefix);

        if (exception is AggregateException aggregateException)
        {
            foreach (Exception innerExc in aggregateException.InnerExceptions)
            {
                LogErrorRec(logger, innerExc, "Inner-", message, args);
            }
        }
        #pragma warning restore CA2254
    }

    /// <summary>
    /// Writes stack trace.
    /// </summary>
    /// <param name="logger">The Microsoft.Extensions.Logging.ILogger to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="prefix">The prefix to differentiate entries.</param>
    public static void LogErrorStackTrace(this ILogger logger, Exception? exception, string prefix = "")
    {
        logger.LogError("{Prefix}Stacktrace:\n{StackTrace}", prefix, exception?.StackTrace ?? string.Empty);
    }
}
