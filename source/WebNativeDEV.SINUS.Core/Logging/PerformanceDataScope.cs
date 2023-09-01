// <copyright file="PerformanceDataScope.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Logging;

using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.Core.ArgumentValidation;
using WebNativeDEV.SINUS.Core.Execution;

/// <summary>
/// Represents a manager class responsible for starting and ending a defined scope of execution
/// and log corresponding meta-data.
/// </summary>
public sealed class PerformanceDataScope : IDisposable
{
    private const string MainMessageBody = "execution performance for block is";
    private readonly ILogger logger;
    private readonly string? prefix;
    private readonly Stopwatch stopwatch;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceDataScope"/> class.
    /// </summary>
    /// <param name="logger">The logger to write the performance data to.</param>
    /// <param name="prefix">The prefix that displays the stage of the run.</param>
    /// <param name="description">The text to log for the start of the section.</param>
    public PerformanceDataScope(ILogger logger, string? prefix = null, string? description = null)
    {
        this.logger = logger;
        this.prefix = prefix;
        this.prefix = prefix == null ? string.Empty : prefix + ": ";

        if (prefix != null || description != null)
        {
            this.logger.LogInformation("{Prefix}{Description}", this.prefix, description ?? string.Empty);
        }

        this.stopwatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="PerformanceDataScope"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    ~PerformanceDataScope()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Checks if a message matches the template of performance messages.
    /// </summary>
    /// <param name="message">The message to check.</param>
    /// <returns>Whether it is a performance message or not.</returns>
    public static bool IsPerformanceMessage(string message)
        => Ensure.NotNull(message).Contains(MainMessageBody, StringComparison.InvariantCulture);

    /// <summary>
    /// Removes all the clutter from a performance message and reduces it to the count of ms only.
    /// </summary>
    /// <param name="message">The message to reduce.</param>
    /// <returns>The reduced message.</returns>
    public static string ReduceLogMessage(string message)
    {
        var idx = Ensure.NotNull(message).IndexOf(MainMessageBody, StringComparison.InvariantCulture) + MainMessageBody.Length;
        var result = "= " + message[idx..].Trim();
        if (result == "= 0 ms")
        {
            // return string.Empty;
        }

        return result;
    }

    /// <summary>
    /// Writes a skip message to logger.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="description">The description.</param>
    /// <param name="actionsCount">Count of actions that are skipped.</param>
    public static void WriteSkip(ILogger<ExecutionEngine> logger, string prefix, object description, int actionsCount)
    {
        var prefixCalculated = prefix == null ? string.Empty : prefix + ": ";

        logger.LogInformation("{Prefix}-> skip {Count} actions ({Description})\n", prefixCalculated, actionsCount, description ?? string.Empty);
    }

    /// <summary>
    /// The Dispose function as defined by IDisposable and corresponding pattern.
    /// </summary>
    public void Dispose()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.stopwatch.Stop();
                this.logger.LogInformation(
                    "{Prefix}" + MainMessageBody + " {Elapsed} ms",
                    this.prefix,
                    this.stopwatch.ElapsedMilliseconds);
            }

            this.disposedValue = true;
        }
    }
}
