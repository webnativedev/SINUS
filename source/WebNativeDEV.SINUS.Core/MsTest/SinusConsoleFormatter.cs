﻿// <copyright file="SinusConsoleFormatter.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using WebNativeDEV.SINUS.Core.FluentAPI;

/// <summary>
/// Formatter class that is very much custom for the use case and defines "blocks" for
/// given, when, then, debug, dispose.
/// </summary>
public class SinusConsoleFormatter : ConsoleFormatter, IDisposable
{
    private readonly IDisposable? optionsReloadToken;
    private ConsoleFormatterOptions formatterOptions;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="SinusConsoleFormatter"/> class.
    /// </summary>
    /// <param name="options">Options to configure from outside. DateTime Settings ignored.</param>
    public SinusConsoleFormatter(IOptionsMonitor<ConsoleFormatterOptions> options)
        : base("SinusConsoleFormatter")
    {
        (this.optionsReloadToken, this.formatterOptions) =
            (options.OnChange(this.ReloadLoggerOptions), options?.CurrentValue ?? new ConsoleFormatterOptions());
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="SinusConsoleFormatter"/> class.
    /// </summary>
    ~SinusConsoleFormatter()
    {
        this.Dispose(disposing: false);
    }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        string? message =
            logEntry.Formatter?.Invoke(
                logEntry.State, logEntry.Exception);

        if (message is null || textWriter is null)
        {
            return;
        }

        if (PerformanceDataScope.IsPerformanceMessage(message) && this.formatterOptions.IncludeScopes)
        {
            var performanceLogMessage = PerformanceDataScope.ReduceLogMessage(message);
            if (string.IsNullOrWhiteSpace(performanceLogMessage))
            {
                textWriter.WriteLine();
                return;
            }

            textWriter.WriteLine(PerformanceDataScope.ReduceLogMessage(message) + Environment.NewLine);
            return;
        }

        if (RunCategoryIsStartOf(message))
        {
            textWriter.WriteLine(message);
            return;
        }

        textWriter.WriteLine($"    {message}");
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.optionsReloadToken?.Dispose();
            }

            this.disposedValue = true;
        }
    }

    private static bool RunCategoryIsStartOf(string message)
    {
        return Enum.GetValues<RunCategory>()
                   .Select(x => x.ToString())
                   .Any(x => message.StartsWith(x, StringComparison.InvariantCulture));
    }

    private void ReloadLoggerOptions(ConsoleFormatterOptions options) => this.formatterOptions = options;
}