// <copyright file="PerformanceDataScope.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.Extensions.Logging;
using System.Diagnostics;

internal sealed class PerformanceDataScope : IDisposable
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
    ~PerformanceDataScope()
    {
        this.Dispose(disposing: false);
    }

    public static bool IsPerformanceMessage(string message)
        => message.Contains(MainMessageBody, StringComparison.InvariantCulture);

    public static string ReduceLogMessage(string message)
    {
        var idx = message.IndexOf(MainMessageBody, StringComparison.InvariantCulture) + MainMessageBody.Length;
        var result = "= " + message[idx..].Trim();
        if(result == "= 0 ms")
        {
            return string.Empty;
        }

        return result;
    }

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
